import { Component, inject, signal, computed, OnInit, DestroyRef, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { debounceTime, distinctUntilChanged, switchMap, of } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { TicketService, GetTicketsParams, PagedResponse } from '../../../core/services/ticket.service';
import { Ticket } from '../../../shared/models/common/ticket.interface';
import { TicketStatus, TicketPriority } from '../../../shared/models/common/enums';

@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, ScrollingModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.scss']
})
export class TicketListComponent implements OnInit {
  private readonly ticketService = inject(TicketService);
  public readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

  // Signals
  tickets = signal<Ticket[]>([]);
  totalCount = signal(0);
  currentPage = signal(1);
  pageSize = signal(10);
  isLoading = signal(false);

  // Computed
  totalPages = computed(() => Math.ceil(this.totalCount() / this.pageSize()));

  // Form for filters
  filterForm: FormGroup = this.fb.group({
    searchTerm: [''],
    status: [''],
    priority: ['']
  });

  ngOnInit(): void {
    this.loadTickets();

    // Setup search with debounce
    this.filterForm.get('searchTerm')!.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(() => {
      this.currentPage.set(1);
      this.loadTickets();
    });
  }

  loadTickets(): void {
    this.isLoading.set(true);
    const params: GetTicketsParams = {
      page: this.currentPage(),
      pageSize: this.pageSize(),
      search: this.filterForm.get('searchTerm')!.value || undefined,
      status: this.filterForm.get('status')!.value || undefined,
      priority: this.filterForm.get('priority')!.value || undefined
    };

    this.ticketService.getTickets(params).pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: (response: PagedResponse<Ticket>) => {
        this.tickets.set(response.items);
        this.totalCount.set(response.totalItems);
        this.isLoading.set(false);
      },
      error: (error: any) => {
        console.error('Error loading tickets:', error);
        this.isLoading.set(false);
      }
    });
  }

  onFilterChange(): void {
    this.currentPage.set(1);
    this.loadTickets();
  }

  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
      this.loadTickets();
    }
  }

  onPageSizeChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.pageSize.set(Number(target.value));
    this.currentPage.set(1);
    this.loadTickets();
  }

  trackByTicketId(_index: number, ticket: Ticket): number {
    return ticket.id;
  }

  getStatusBadgeClass(status: string): string {
    switch (status) {
      case TicketStatus.Open: return 'badge-open';
      case TicketStatus.InProgress: return 'badge-inprogress';
      case TicketStatus.Resolved: return 'badge-resolved';
      case TicketStatus.Closed: return 'badge-closed';
      default: return '';
    }
  }
}
