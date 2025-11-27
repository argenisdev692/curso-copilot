// Migrated TypeScript code from legacy.js

enum Priority {
  Low = 'low',
  Medium = 'medium',
  High = 'high'
}

interface Ticket {
  id: number;
  title: string;
  priority: Priority;
  assignedTo?: string;
  createdAt: Date;
}

interface AppConfig {
  apiUrl: string;
  timeout: number;
}

const userName: string = 'John';
let tickets: Ticket[] = [];
const config: AppConfig = {
  apiUrl: 'https://api.example.com',
  timeout: 5000
};

function createTicket(
  title: string,
  priority: Priority,
  assignedTo?: string
): Ticket {
  return {
    id: Math.random(),
    title,
    priority,
    assignedTo,
    createdAt: new Date()
  };
}

const ticketsArray: Ticket[] = [
  { id: 1, title: 'Bug', priority: Priority.High, createdAt: new Date() },
  { id: 2, title: 'Feature', priority: Priority.Low, createdAt: new Date() }
];

function getHighPriorityTickets(tickets: Ticket[]): Ticket[] {
  return tickets.filter(t => t.priority === Priority.High);
}

async function fetchTickets(): Promise<Ticket[]> {
  try {
    const response = await fetch('/api/tickets');

    if (!response.ok) {
      throw new Error(`HTTP ${response.status}: ${response.statusText}`);
    }

    const data: Ticket[] = await response.json();
    return data;
  } catch (error) {
    console.error('Error fetching tickets:', error);
    throw error;
  }
}

class TicketService {
  private readonly apiUrl: string;
  private cache: Map<string, Ticket[]>;

  constructor(apiUrl: string) {
    this.apiUrl = apiUrl;
    this.cache = new Map();
  }

  public async getAll(): Promise<Ticket[]> {
    const cached = this.cache.get('tickets');
    if (cached) {
      return cached;
    }

    const tickets = await this.fetchFromApi();
    this.cache.set('tickets', tickets);
    return tickets;
  }

  private async fetchFromApi(): Promise<Ticket[]> {
    const response = await fetch(this.apiUrl);

    if (!response.ok) {
      throw new Error(`API error: ${response.status}`);
    }

    return response.json();
  }

  public clearCache(): void {
    this.cache.clear();
  }
}

interface Entity {
  id: number;
}

function findById<T extends Entity>(items: T[], id: number): T | undefined {
  return items.find(item => item.id === id);
}

interface SuccessResponse<T> {
  success: true;
  data: T;
}

interface ErrorResponse {
  success: false;
  error: string;
}

type ApiResponse<T> = SuccessResponse<T> | ErrorResponse;

function isSuccess<T>(response: ApiResponse<T>): response is SuccessResponse<T> {
  return response.success === true;
}

function processResponse<T>(response: ApiResponse<T>): T | null {
  if (isSuccess(response)) {
    return response.data;
  }

  console.error(response.error);
  return null;
}