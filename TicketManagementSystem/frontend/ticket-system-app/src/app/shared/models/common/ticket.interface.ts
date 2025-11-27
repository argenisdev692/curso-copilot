export interface Ticket {
  id: number;
  title: string;
  description: string;
  status: string;
  priority: string;
  createdById: number;
  createdByName: string;
  assignedToId?: number;
  assignedToName?: string;
  createdAt: string;
  updatedAt: string;
}
