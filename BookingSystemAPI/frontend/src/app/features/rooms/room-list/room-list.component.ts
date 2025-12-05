import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatChipsModule } from '@angular/material/chips';
import { map } from 'rxjs';
import { RoomService } from '@core/services';
import { Room } from '@core/models';

/**
 * Componente de listado de salas.
 * Muestra todas las salas disponibles para reserva.
 */
@Component({
  selector: 'app-room-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatSnackBarModule,
    MatChipsModule,
  ],
  templateUrl: './room-list.component.html',
  styleUrl: './room-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoomListComponent implements OnInit {
  private readonly roomService = inject(RoomService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  /** Lista de salas */
  rooms = signal<Room[]>([]);

  /** Estado de carga */
  isLoading = signal(true);

  /** Error */
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadRooms();
  }

  /**
   * Carga la lista de salas.
   */
  loadRooms(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.roomService
      .getRooms()
      .pipe(map((response) => response.data?.items ?? []))
      .subscribe({
        next: (rooms) => {
          this.rooms.set(rooms);
          this.isLoading.set(false);
        },
        error: (err) => {
          this.error.set(err.message || 'Error al cargar las salas');
          this.isLoading.set(false);
        },
      });
  }

  /**
   * Abre el diálogo para crear una reserva.
   * @param room Sala seleccionada
   */
  openBookingDialog(room: Room): void {
    // TODO: Implementar diálogo de reserva
    this.snackBar.open(`Funcionalidad de reserva para ${room.name} próximamente`, 'Cerrar', {
      duration: 3000,
    });
  }
}
