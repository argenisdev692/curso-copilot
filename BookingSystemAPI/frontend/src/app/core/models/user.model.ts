/**
 * User model - matches backend UserDto
 */
export interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role?: UserRole;
  isActive?: boolean;
  createdAt?: string;
  updatedAt?: string;
}

/**
 * User roles enum
 */
export enum UserRole {
  Admin = 'Admin',
  User = 'User',
}

/**
 * Login request DTO - matches backend LoginDto
 */
export interface LoginRequest {
  email: string;
  password: string;
}

/**
 * Login response DTO - matches backend AuthResponseDto
 */
export interface LoginResponse {
  token: string;
  refreshToken: string;
  expiresAt: string;
  user: User;
}

/**
 * Register request DTO - matches backend RegisterDto
 */
export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
}

/**
 * Token refresh request DTO
 */
export interface RefreshTokenRequest {
  refreshToken: string;
}
