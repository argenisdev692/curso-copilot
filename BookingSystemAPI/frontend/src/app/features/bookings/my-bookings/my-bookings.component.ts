import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { map } from 'rxjs';
import { BookingService } from '@core/services';
import { Booking, BookingStatus } from '@core/models';

/**
 * Componente de mis reservas.
 * Muestra las reservas del usuario actual.
 */
@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatSnackBarModule,
  ],
  templateUrl: './my-bookings.component.html',
  styleUrl: './my-bookings.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MyBookingsComponent implements OnInit {
  private readonly bookingService = inject(BookingService);
  private readonly snackBar = inject(MatSnackBar);

  /** Lista de mis reservas */
  bookings = signal<Booking[]>([]);

  /** Estado de carga */
  isLoading = signal(true);

  /** Error */
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadMyBookings();
  }

  /**
   * Carga las reservas del usuario.
   */
  loadMyBookings(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.bookingService
      .getMyBookings()
      .pipe(map((response) => response.data ?? []))
      .subscribe({
        next: (bookings) => {
          this.bookings.set(bookings);
          this.isLoading.set(false);
        },
        error: (err) => {
          this.error.set(err.message || 'Error al cargar tus reservas');
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
   * Verifica si una reserva puede ser cancelada.
   * @param booking Reserva a verificar
   */
  canCancel(booking: Booking): boolean {
    return booking.status !== BookingStatus.Cancelled && new Date(booking.startTime) > new Date();
  }

  /**
   * Cancela una reserva.
   * @param booking Reserva a cancelar
   */
  cancelBooking(booking: Booking): void {
    if (confirm('¿Estás seguro de cancelar esta reserva?')) {
      this.bookingService.cancelBooking(booking.id).subscribe({
        next: () => {
          this.snackBar.open('Reserva cancelada exitosamente', 'Cerrar', {
            duration: 3000,
            panelClass: 'success-snackbar',
          });
          this.loadMyBookings();
        },
        error: (err) => {
          this.snackBar.open(err.message || 'Error al cancelar la reserva', 'Cerrar', {
            duration: 3000,
            panelClass: 'error-snackbar',
          });
        },
      });
    }
  }
}
