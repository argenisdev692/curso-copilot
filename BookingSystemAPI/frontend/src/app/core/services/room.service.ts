import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import {
  ApiResponse,
  PaginatedResponse,
  PaginationParams,
  Room,
  CreateRoomRequest,
  UpdateRoomRequest,
} from '@core/models';

/**
 * Room service for handling room CRUD operations
 */
@Injectable({
  providedIn: 'root',
})
export class RoomService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/rooms`;

  /**
   * Get all rooms with pagination
   * @param params Pagination parameters
   * @returns Observable with paginated rooms
   */
  getRooms(params?: Partial<PaginationParams>): Observable<ApiResponse<PaginatedResponse<Room>>> {
    let httpParams = new HttpParams();

    if (params) {
      if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
      if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
      if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
      if (params.sortDirection) httpParams = httpParams.set('sortDirection', params.sortDirection);
      if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    }

    return this.http.get<ApiResponse<PaginatedResponse<Room>>>(this.apiUrl, { params: httpParams });
  }

  /**
   * Get room by ID
   * @param id Room ID
   * @returns Observable with room data
   */
  getRoomById(id: number): Observable<ApiResponse<Room>> {
    return this.http.get<ApiResponse<Room>>(`${this.apiUrl}/${id}`);
  }

  /**
   * Create new room
   * @param room Room data
   * @returns Observable with created room
   */
  createRoom(room: CreateRoomRequest): Observable<ApiResponse<Room>> {
    return this.http.post<ApiResponse<Room>>(this.apiUrl, room);
  }

  /**
   * Update existing room
   * @param id Room ID
   * @param room Updated room data
   * @returns Observable with updated room
   */
  updateRoom(id: number, room: UpdateRoomRequest): Observable<ApiResponse<Room>> {
    return this.http.put<ApiResponse<Room>>(`${this.apiUrl}/${id}`, room);
  }

  /**
   * Delete room
   * @param id Room ID
   * @returns Observable with void response
   */
  deleteRoom(id: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }

  /**
   * Get available rooms for a time range
   * @param startTime Start time
   * @param endTime End time
   * @returns Observable with available rooms
   */
  getAvailableRooms(startTime: string, endTime: string): Observable<ApiResponse<Room[]>> {
    const params = new HttpParams()
      .set('startTime', startTime)
      .set('endTime', endTime);

    return this.http.get<ApiResponse<Room[]>>(`${this.apiUrl}/available`, { params });
  }
}
