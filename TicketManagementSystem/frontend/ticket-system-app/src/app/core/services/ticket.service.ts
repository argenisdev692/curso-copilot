import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Ticket } from '../../shared/models/common/ticket.interface';

export interface GetTicketsParams {
  page?: number;
  pageSize?: number;
  status?: string;
  priority?: string;
  assignedTo?: number;
  search?: string;
  sortBy?: string;
  sortOrder?: string;
}

export interface PagedResponse<T> {
  items: T[];
  totalItems: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

@Injectable({
  providedIn: 'root'
})
/**
 * Service for managing ticket operations.
 * Handles fetching, creating, updating, and deleting tickets.
 */
export class TicketService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/tickets';

  /**
   * Retrieves a paginated list of tickets based on filter criteria.
   * @param params Filter and pagination parameters
   * @returns Observable of PagedResponse containing tickets
   */
  getTickets(params: GetTicketsParams = {}): Observable<PagedResponse<Ticket>> {
    let httpParams = new HttpParams();

    if (params.page) httpParams = httpParams.set('page', params.page.toString());
    if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
    if (params.status) httpParams = httpParams.set('status', params.status);
    if (params.priority) httpParams = httpParams.set('priority', params.priority);
    if (params.assignedTo) httpParams = httpParams.set('assignedTo', params.assignedTo.toString());
    if (params.search) httpParams = httpParams.set('search', params.search);
    if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
    if (params.sortOrder) httpParams = httpParams.set('sortOrder', params.sortOrder);

    return this.http.get<PagedResponse<Ticket>>(this.apiUrl, { params: httpParams });
  }
}
