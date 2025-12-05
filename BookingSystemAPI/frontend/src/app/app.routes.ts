import { Routes } from '@angular/router';
import { authGuard, noAuthGuard } from '@core/guards';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: 'auth',
    canActivate: [noAuthGuard],
    loadComponent: () =>
      import('@layouts/auth-layout').then((m) => m.AuthLayoutComponent),
    children: [
      {
        path: 'login',
        loadComponent: () =>
          import('@features/auth/login/login.component').then((m) => m.LoginComponent),
      },
      {
        path: 'register',
        loadComponent: () =>
          import('@features/auth/register/register.component').then((m) => m.RegisterComponent),
      },
      {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full',
      },
    ],
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () =>
      import('@layouts/main-layout').then((m) => m.MainLayoutComponent),
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('@features/dashboard/dashboard.component').then((m) => m.DashboardComponent),
      },
      {
        path: 'rooms',
        loadComponent: () =>
          import('@features/rooms/room-list/room-list.component').then((m) => m.RoomListComponent),
      },
      {
        path: 'bookings',
        loadComponent: () =>
          import('@features/bookings/booking-list/booking-list.component').then(
            (m) => m.BookingListComponent
          ),
      },
      {
        path: 'my-bookings',
        loadComponent: () =>
          import('@features/bookings/my-bookings/my-bookings.component').then(
            (m) => m.MyBookingsComponent
          ),
      },
    ],
  },
  {
    path: '**',
    redirectTo: 'dashboard',
  },
];
