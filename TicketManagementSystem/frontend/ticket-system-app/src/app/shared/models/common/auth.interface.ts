/**
 * Interfaces TypeScript para autenticación basadas en el backend .NET TicketManagementSystem
 */

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  expiresAt: string; // ISO string
  user: UserBasicDto;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  role: string;
}

export interface RegisterResponse {
  message: string;
  user: UserBasicDto;
}

export interface UserBasicDto {
  id: number;
  email: string;
  fullName: string;
  role: string;
}

export interface ProblemDetails {
  title?: string;
  detail?: string;
  status?: number;
  [key: string]: any;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface RefreshTokenResponse {
  token: string;
  refreshToken: string;
  expiresAt: string;
}
