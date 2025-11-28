import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/authentication/services/auth.service';
import { AuthState } from '../../../../core/authentication/state/auth.state';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <header class="sticky top-0 z-50 backdrop-blur-xl bg-white/5 border-b border-white/10">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between items-center h-16">
          <!-- Logo -->
          <div class="flex items-center space-x-3">
            <div class="w-10 h-10 bg-gradient-to-br from-primary-500 to-primary-700 rounded-xl flex items-center justify-center shadow-lg shadow-primary-500/30">
              <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 5v2m0 4v2m0 4v2M5 5a2 2 0 00-2 2v3a2 2 0 110 4v3a2 2 0 002 2h14a2 2 0 002-2v-3a2 2 0 110-4V7a2 2 0 00-2-2H5z"></path>
              </svg>
            </div>
            <div>
              <h1 class="text-xl font-bold bg-gradient-to-r from-white via-primary-200 to-primary-400 bg-clip-text text-transparent">
                TicketSystem
              </h1>
              <p class="text-xs text-white/40 -mt-0.5">Management Portal</p>
            </div>
          </div>

          <!-- User menu -->
          <div class="flex items-center space-x-4">
            <!-- Notifications placeholder -->
            <button class="relative p-2 text-white/60 hover:text-white hover:bg-white/10 rounded-xl transition-all duration-300">
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9"></path>
              </svg>
              <span class="absolute top-1 right-1 w-2 h-2 bg-primary-500 rounded-full animate-pulse"></span>
            </button>

            <!-- User profile -->
            <div class="flex items-center space-x-3 px-3 py-2 rounded-xl bg-white/5 border border-white/10 hover:bg-white/10 transition-all duration-300">
              <div class="w-9 h-9 bg-gradient-to-br from-primary-400 to-primary-600 rounded-xl flex items-center justify-center ring-2 ring-primary-400/30">
                <span class="text-white text-sm font-bold">
                  {{ authState.userInitials() }}
                </span>
              </div>
              <div class="hidden sm:block">
                <p class="text-sm font-medium text-white/90">
                  {{ authState.userName() }}
                </p>
                <p class="text-xs text-white/50">
                  {{ authState.userRole() }}
                </p>
              </div>
            </div>

            <!-- Logout button -->
            <button
              (click)="logout()"
              data-cy="logout-button"
              class="group inline-flex items-center px-4 py-2.5 rounded-xl text-sm font-medium text-white/80 bg-white/5 border border-white/10 hover:bg-red-500/20 hover:border-red-500/30 hover:text-red-300 focus:outline-none focus:ring-2 focus:ring-red-500/50 transition-all duration-300"
            >
              <svg class="w-4 h-4 mr-2 group-hover:translate-x-0.5 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path>
              </svg>
              <span class="hidden sm:inline">Logout</span>
            </button>
          </div>
        </div>
      </div>
    </header>
  `,
  styles: []
})
export class HeaderComponent {
  private authService = inject(AuthService);
  protected authState = inject(AuthState);

  logout() {
    this.authService.logout();
  }
}
