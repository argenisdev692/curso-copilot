import { Room } from './room.model';
import { User } from './user.model';

/**
 * Booking model - matches backend BookingDto
 */
export interface Booking {
  id: number;
  title: string;
  description?: string;
  notes?: string;
  startTime: string;
  endTime: string;
  durationMinutes: number;
  status: BookingStatus;
  roomId: number;
  roomName: string;
  room?: Room;
  organizerName: string;
  organizerEmail: string;
  attendeeCount: number;
  userId?: number;
  userName?: string;
  user?: User;
  createdAt: string;
  updatedAt?: string;
}

/**
 * Booking status enum
 */
export enum BookingStatus {
  Pending = 'Pending',
  Confirmed = 'Confirmed',
  Cancelled = 'Cancelled',
  Completed = 'Completed',
}

/**
 * Create booking request DTO - matches backend CreateBookingDto
 */
export interface CreateBookingRequest {
  title: string;
  description?: string;
  startTime: string;
  endTime: string;
  roomId: number;
  organizerName: string;
  organizerEmail: string;
  attendeeCount: number;
  notes?: string;
}

/**
 * Update booking request DTO - matches backend UpdateBookingDto
 */
export interface UpdateBookingRequest {
  title: string;
  description?: string;
  startTime: string;
  endTime: string;
  attendeeCount: number;
  notes?: string;
}

/**
 * Booking query parameters
 */
export interface BookingQueryParams {
  roomId?: number;
  startDate?: string;
  endDate?: string;
  status?: BookingStatus;
}
