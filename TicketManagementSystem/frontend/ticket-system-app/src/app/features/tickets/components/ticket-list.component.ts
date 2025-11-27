import { Component, OnInit, ChangeDetectionStrategy, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScrollingModule } from '@angular/cdk/scrolling';

interface Ticket {
  id: number;
  title: string;
  status: string;
  priority: string;
}

/**
 * Component for displaying a list of tickets.
 * Uses virtual scrolling for performance optimization.
 */
@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [CommonModule, ScrollingModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="ticket-list">
      <h2>Tickets ({{ ticketCount() }})</h2>
      <cdk-virtual-scroll-viewport itemSize="80" class="example-viewport">
        <div *cdkVirtualFor="let ticket of tickets(); trackBy: trackByTicketId" class="ticket-item">
          <h3>{{ ticket.title }}</h3>
          <p>Status: {{ ticket.status }}</p>
          <p>Priority: {{ ticket.priority }}</p>
        </div>
      </cdk-virtual-scroll-viewport>
    </div>
  `,
  styles: [`
    .ticket-list {
      padding: 20px;
    }
    .example-viewport {
      height: 400px;
      width: 100%;
      border: 1px solid black;
    }
    .ticket-item {
      height: 80px;
      border-bottom: 1px solid #ccc;
      padding: 10px;
      box-sizing: border-box;
    }
  `]
})
export class TicketListComponent implements OnInit {
  // Signals for reactive state
  private ticketsSignal = signal<Ticket[]>([]);

  // Computed signals for derived values
  tickets = this.ticketsSignal.asReadonly();
  ticketCount = computed(() => this.ticketsSignal().length);
  openTicketsCount = computed(() => this.ticketsSignal().filter(t => t.status === 'Open').length);

  ngOnInit(): void {
    // Simulate loading tickets (in real app, use service)
    const simulatedTickets = Array.from({ length: 1000 }).map((_, i) => ({
      id: i + 1,
      title: `Ticket #${i + 1}`,
      status: i % 2 === 0 ? 'Open' : 'Closed',
      priority: i % 3 === 0 ? 'High' : 'Low'
    }));

    this.ticketsSignal.set(simulatedTickets);
  }

  /**
   * TrackBy function for ngFor loops
   * @param index The index of the item
   * @param ticket The ticket item
   * @returns The unique identifier of the ticket
   */
  trackByTicketId(_index: number, ticket: Ticket): number {
    return ticket.id;
  }

  // Methods for potential future use (e.g., filtering)
  deleteTicket(_id: number): void {
    // Update signal immutably
    this.ticketsSignal.update(tickets => tickets.filter(t => t.id !== _id));
  }

  filterByStatus(status: string): void {
    // In real app, filter from service or update signal
    // For demo, just log
    console.log(`Filtering by status: ${status}`);
  }
}
