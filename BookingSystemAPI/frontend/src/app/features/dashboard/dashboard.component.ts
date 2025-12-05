import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { forkJoin, map } from 'rxjs';
import { AuthService, BookingService, RoomService } from '@core/services';
import { Booking, Room } from '@core/models';

/**
 * Componente del Dashboard principal.
 * Muestra un resumen de las estadísticas y accesos rápidos del sistema.
 */
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    RouterLink,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly bookingService = inject(BookingService);
  private readonly roomService = inject(RoomService);

  /** Usuario actual */
  currentUser = this.authService.currentUser;

  /** Estadísticas del dashboard */
  stats = signal<{
    totalRooms: number;
    availableRooms: number;
    myBookings: number;
    upcomingBookings: number;
  }>({
    totalRooms: 0,
    availableRooms: 0,
    myBookings: 0,
    upcomingBookings: 0,
  });

  /** Próximas reservas */
  upcomingBookings = signal<Booking[]>([]);

  /** Salas disponibles */
  availableRooms = signal<Room[]>([]);

  /** Estado de carga */
  isLoading = signal(true);

  ngOnInit(): void {
    this.loadDashboardData();
  }

  /**
   * Carga los datos del dashboard.
   */
  private loadDashboardData(): void {
    forkJoin({
      rooms: this.roomService.getRooms().pipe(map((r) => r.data?.items ?? [])),
      bookings: this.bookingService.getMyBookings().pipe(map((r) => r.data ?? [])),
    }).subscribe({
      next: ({ rooms, bookings }) => {
        const available = rooms.filter((r) => r.status === 'Available');
        this.availableRooms.set(available.slice(0, 4));

        const upcoming = bookings.filter((b) => new Date(b.startTime) > new Date());
        this.upcomingBookings.set(upcoming.slice(0, 5));

        this.stats.set({
          totalRooms: rooms.length,
          availableRooms: available.length,
          myBookings: bookings.length,
          upcomingBookings: upcoming.length,
        });

        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      },
    });
  }
}
