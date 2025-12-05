import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import {
  ApiResponse,
  PaginatedResponse,
  PaginationParams,
  Booking,
  CreateBookingRequest,
  UpdateBookingRequest,
  BookingQueryParams,
} from '@core/models';

/**
 * Booking service for handling booking CRUD operations
 */
@Injectable({
  providedIn: 'root',
})
export class BookingService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/bookings`;

  /**
   * Get all bookings with pagination and filters
   * @param params Pagination parameters
   * @param queryParams Query filters
   * @returns Observable with paginated bookings
   */
  getBookings(
    params?: Partial<PaginationParams>,
    queryParams?: BookingQueryParams
  ): Observable<ApiResponse<PaginatedResponse<Booking>>> {
    let httpParams = new HttpParams();

    if (params) {
      if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
      if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
      if (params.sortDirection) httpParams = httpParams.set('sortDirection', params.sortDirection);
    }

    if (queryParams) {
      if (queryParams.roomId) httpParams = httpParams.set('roomId', queryParams.roomId.toString());
      if (queryParams.startDate) httpParams = httpParams.set('startDate', queryParams.startDate);
      if (queryParams.endDate) httpParams = httpParams.set('endDate', queryParams.endDate);
      if (queryParams.status) httpParams = httpParams.set('status', queryParams.status);
    }

    return this.http.get<ApiResponse<PaginatedResponse<Booking>>>(this.apiUrl, { params: httpParams });
  }

  /**
   * Get booking by ID
   * @param id Booking ID
   * @returns Observable with booking data
   */
  getBookingById(id: number): Observable<ApiResponse<Booking>> {
    return this.http.get<ApiResponse<Booking>>(`${this.apiUrl}/${id}`);
  }

  /**
   * Get current user's bookings
   * @returns Observable with user's bookings
   */
  getMyBookings(): Observable<ApiResponse<Booking[]>> {
    return this.http.get<ApiResponse<Booking[]>>(`${this.apiUrl}/my`);
  }

  /**
   * Create new booking
   * @param booking Booking data
   * @returns Observable with created booking
   */
  createBooking(booking: CreateBookingRequest): Observable<ApiResponse<Booking>> {
    return this.http.post<ApiResponse<Booking>>(this.apiUrl, booking);
  }

  /**
   * Update existing booking
   * @param id Booking ID
   * @param booking Updated booking data
   * @returns Observable with updated booking
   */
  updateBooking(id: number, booking: UpdateBookingRequest): Observable<ApiResponse<Booking>> {
    return this.http.put<ApiResponse<Booking>>(`${this.apiUrl}/${id}`, booking);
  }

  /**
   * Cancel booking
   * @param id Booking ID
   * @returns Observable with void response
   */
  cancelBooking(id: number): Observable<ApiResponse<void>> {
    return this.http.post<ApiResponse<void>>(`${this.apiUrl}/${id}/cancel`, {});
  }

  /**
   * Delete booking
   * @param id Booking ID
   * @returns Observable with void response
   */
  deleteBooking(id: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }
}
