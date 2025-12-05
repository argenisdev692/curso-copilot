/**
 * Room model - matches backend RoomDto
 */
export interface Room {
  id: number;
  name: string;
  capacity: number;
  equipment: string[];
  location: string;
  status: RoomStatus;
  createdAt: string;
  updatedAt?: string;
}

/**
 * Room status enum
 */
export enum RoomStatus {
  Available = 'Available',
  Maintenance = 'Maintenance',
  Occupied = 'Occupied',
}

/**
 * Create room request DTO - matches backend CreateRoomDto
 */
export interface CreateRoomRequest {
  name: string;
  capacity: number;
  equipment: string[];
  location: string;
}

/**
 * Update room request DTO - matches backend UpdateRoomDto
 */
export interface UpdateRoomRequest {
  name: string;
  capacity: number;
  equipment: string[];
  location: string;
  status: string;
}
