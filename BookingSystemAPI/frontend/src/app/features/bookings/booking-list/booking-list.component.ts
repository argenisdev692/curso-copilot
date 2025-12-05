import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { map } from 'rxjs';
import { BookingService } from '@core/services';
import { Booking, BookingStatus } from '@core/models';

/**
 * Componente de listado de todas las reservas.
 * Muestra todas las reservas del sistema (para administradores).
 */
@Component({
  selector: 'app-booking-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatPaginatorModule,
    MatTooltipModule,
  ],
  templateUrl: './booking-list.component.html',
  styleUrl: './booking-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BookingListComponent implements OnInit {
  private readonly bookingService = inject(BookingService);

  /** Lista de reservas */
  bookings = signal<Booking[]>([]);

  /** Columnas a mostrar */
  displayedColumns = ['roomName', 'userName', 'startTime', 'endTime', 'status', 'actions'];

  /** Estado de carga */
  isLoading = signal(true);

  /** Error */
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadBookings();
  }

  /**
   * Carga la lista de reservas.
   */
  loadBookings(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.bookingService
      .getBookings()
      .pipe(map((response) => response.data?.items ?? []))
      .subscribe({
        next: (bookings) => {
          this.bookings.set(bookings);
          this.isLoading.set(false);
        },
        error: (err) => {
          this.error.set(err.message || 'Error al cargar las reservas');
          this.isLoading.set(false);
        },
      });
  }

  /**
   * Obtiene el color del chip según el estado.
   * @param status Estado de la reserva
   */
  getStatusColor(status: BookingStatus | string): string {
    switch (status) {
      case BookingStatus.Confirmed:
        return 'bg-green-100 text-green-800';
      case BookingStatus.Pending:
        return 'bg-yellow-100 text-yellow-800';
      case BookingStatus.Cancelled:
        return 'bg-red-100 text-red-800';
      case BookingStatus.Completed:
        return 'bg-blue-100 text-blue-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  }

  /**
   * Cancela una reserva.
   * @param booking Reserva a cancelar
   */
  cancelBooking(booking: Booking): void {
    if (confirm('¿Estás seguro de cancelar esta reserva?')) {
      this.bookingService.cancelBooking(booking.id).subscribe({
        next: () => {
          this.loadBookings();
        },
      });
    }
  }
}
