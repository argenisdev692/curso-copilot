import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject, of, from, throwError } from 'rxjs';
import {
  debounceTime,
  distinctUntilChanged,
  switchMap,
  retryWhen,
  catchError,
  tap,
  shareReplay,
  delay,
  take,
  mergeMap
} from 'rxjs/operators';
import { Ticket } from '../../shared/models/common/ticket.interface';

/**
 * Service for real-time ticket search with advanced RxJS operators
 * Features: debounce, caching, retry with exponential backoff, loading states
 */
@Injectable({
  providedIn: 'root'
})
export class TicketSearchService {
  private readonly http = inject(HttpClient);

  // Reactive state using Angular signals
  /** Loading state signal */
  readonly isLoading = signal<boolean>(false);

  /** Last error signal */
  readonly lastError = signal<string | null>(null);

  /** Computed signal for current results count */
  readonly resultsCount = computed(() => this.currentResultsSignal().length);

  // Internal state
  private cache = new Map<string, Ticket[]>();
  private searchSubject = new BehaviorSubject<string>('');
  private currentResultsSignal = signal<Ticket[]>([]);
  private readonly RECENT_SEARCHES_KEY = 'ticket-search-recent';
  private readonly MAX_RECENT_SEARCHES = 5;

  /**
   * Observable for search results with all RxJS operators applied
   */
  private searchResults$ = this.searchSubject.pipe(
    debounceTime(400),
    distinctUntilChanged(),
    switchMap(term => {
      if (!term.trim()) {
        this.isLoading.set(false);
        this.lastError.set(null);
        this.currentResultsSignal.set([]);
        return of([]);
      }

      // Check cache first
      if (this.cache.has(term)) {
        this.isLoading.set(false);
        this.lastError.set(null);
        this.currentResultsSignal.set(this.cache.get(term)!);
        this.addToRecentSearches(term);
        return of(this.cache.get(term)!);
      }

      // Set loading state
      this.isLoading.set(true);
      this.lastError.set(null);

      // Make HTTP request
      return this.http.get<Ticket[]>(`/api/tickets?search=${encodeURIComponent(term)}`).pipe(
        retryWhen(errors =>
          errors.pipe(
            mergeMap((error, index) => {
              if (index >= 3) {
                return throwError(error);
              }
              const delayMs = Math.pow(2, index) * 1000; // 1s, 2s, 4s
              return of(error).pipe(delay(delayMs));
            })
          )
        ),
        tap(results => {
          // Cache the results
          this.cache.set(term, results);
          this.currentResultsSignal.set(results);
          this.addToRecentSearches(term);
          this.isLoading.set(false);
        }),
        catchError(error => {
          this.isLoading.set(false);
          this.lastError.set(this.getErrorMessage(error));
          this.currentResultsSignal.set([]);
          return of([]);
        })
      );
    }),
    shareReplay(1)
  );

  /**
   * Search for tickets by term
   * @param term Search term
   * @returns Observable of ticket array
   */
  searchTickets(term: string): Observable<Ticket[]> {
    this.searchSubject.next(term);
    return this.searchResults$;
  }

  /**
   * Clear the search cache
   */
  clearCache(): void {
    this.cache.clear();
    this.currentResultsSignal.set([]);
  }

  /**
   * Get recent searches from localStorage
   * @returns Array of recent search terms
   */
  getRecentSearches(): string[] {
    try {
      const stored = localStorage.getItem(this.RECENT_SEARCHES_KEY);
      return stored ? JSON.parse(stored) : [];
    } catch {
      return [];
    }
  }

  /**
   * Clear recent searches from localStorage
   */
  clearRecentSearches(): void {
    localStorage.removeItem(this.RECENT_SEARCHES_KEY);
  }

  /**
   * Add term to recent searches, maintaining max limit
   * @param term Search term to add
   */
  private addToRecentSearches(term: string): void {
    const recent = this.getRecentSearches();
    const filtered = recent.filter(t => t !== term);
    filtered.unshift(term);
    const limited = filtered.slice(0, this.MAX_RECENT_SEARCHES);

    try {
      localStorage.setItem(this.RECENT_SEARCHES_KEY, JSON.stringify(limited));
    } catch {
      // Ignore localStorage errors
    }
  }

  /**
   * Extract error message from HttpErrorResponse
   * @param error HTTP error response
   * @returns Error message string
   */
  private getErrorMessage(error: HttpErrorResponse | any): string {
    if (error instanceof HttpErrorResponse) {
      return error.error?.message || error.message || 'An error occurred';
    }
    return error?.message || 'Unknown error';
  }
}
