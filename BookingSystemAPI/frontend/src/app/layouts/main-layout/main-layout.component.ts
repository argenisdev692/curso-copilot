import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { AuthService } from '@core/services';

/**
 * Navigation item interface
 */
interface NavItem {
  label: string;
  route: string;
  icon: string;
}

/**
 * Main layout component with header, sidebar and content area
 */
@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule,
    MatMenuModule,
    MatDividerModule,
  ],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainLayoutComponent {
  private readonly authService = inject(AuthService);

  /** Current user */
  readonly currentUser = this.authService.currentUser;

  /** Sidebar open state */
  readonly isSidenavOpen = signal(true);

  /** Navigation items */
  readonly navItems: NavItem[] = [
    { label: 'Dashboard', route: '/dashboard', icon: 'dashboard' },
    { label: 'Rooms', route: '/rooms', icon: 'meeting_room' },
    { label: 'Bookings', route: '/bookings', icon: 'event' },
    { label: 'My Bookings', route: '/my-bookings', icon: 'calendar_today' },
  ];

  /** Admin navigation items */
  readonly adminNavItems: NavItem[] = [
    { label: 'Reports', route: '/reports', icon: 'analytics' },
    { label: 'Users', route: '/admin/users', icon: 'people' },
  ];

  /**
   * Toggle sidebar open/close
   */
  toggleSidenav(): void {
    this.isSidenavOpen.update((value) => !value);
  }

  /**
   * Logout user
   */
  logout(): void {
    this.authService.logout();
  }
}
