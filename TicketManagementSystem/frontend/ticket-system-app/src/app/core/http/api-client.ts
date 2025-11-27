// Este archivo será generado por NSwag. Ejecutar 'npm run generate:api' después de iniciar el backend.
// Comando: npm run generate:api

// Código generado por NSwag - Placeholder hasta que se genere
export class ApiClient {
  // Clases y métodos generados por NSwag aparecerán aquí
}

// Interfaces DTO generadas
export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  email: string;
  password: string;
  fullName: string;
  role: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface TicketDto {
  id?: number;
  title: string;
  description: string;
  status: string;
  priority: string;
  createdAt?: string;
  updatedAt?: string;
}

// Servicios generados
export class AuthClient extends ApiClient {
  async login(_dto: LoginDto): Promise<any> {
    throw new Error('AuthClient.login not implemented. Run npm run generate:api first.');
  }

  async register(_dto: RegisterDto): Promise<any> {
    throw new Error('AuthClient.register not implemented. Run npm run generate:api first.');
  }

  async refresh(_dto: RefreshTokenRequest): Promise<any> {
    throw new Error('AuthClient.refresh not implemented. Run npm run generate:api first.');
  }
}

export class TicketsClient extends ApiClient {
  async getTickets(): Promise<TicketDto[]> {
    throw new Error('TicketsClient.getTickets not implemented. Run npm run generate:api first.');
  }

  async createTicket(_dto: Omit<TicketDto, 'id' | 'createdAt' | 'updatedAt'>): Promise<TicketDto> {
    throw new Error('TicketsClient.createTicket not implemented. Run npm run generate:api first.');
  }
}

// Provider para Angular
export class ApiClientBase {
  protected getBaseUrl(): string {
    return 'http://localhost:5201/api';
  }
}
