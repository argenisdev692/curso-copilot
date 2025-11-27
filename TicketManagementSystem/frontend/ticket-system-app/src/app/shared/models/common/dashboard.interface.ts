/**
 * Interfaces para el sistema de estadísticas del dashboard
 */

export interface DashboardStats {
  totalTickets: number;
  pendingTickets: number;
  resolvedTickets: number;
  criticalTickets: number;
  totalUsers: number;
  activeUsers: number;
}

export interface ActivityItem {
  id: number;
  type: 'ticket_created' | 'ticket_resolved' | 'ticket_updated' | 'user_registered';
  description: string;
  timestamp: string;
  user?: string;
  ticketId?: number;
}

export interface DashboardData {
  stats: DashboardStats;
  recentActivity: ActivityItem[];
  userRole: string;
}
