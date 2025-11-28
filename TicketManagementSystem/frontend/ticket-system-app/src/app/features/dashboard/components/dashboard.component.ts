import { Component, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthState } from '@core/authentication/state/auth.state';

/**
 * Main dashboard component displaying welcome message
 */
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <!-- Dashboard Content - Glassmorphism Style -->
    <div class="space-y-6" data-cy="dashboard">
      <!-- Welcome section -->
      <div class="backdrop-blur-xl bg-white/5 border border-white/10 rounded-2xl p-8 hover:bg-white/[0.07] transition-all duration-300">
        <div class="flex items-start justify-between">
          <div>
            <h1 class="text-4xl font-bold text-white mb-2">
              Welcome back, <span class="bg-gradient-to-r from-primary-400 via-pink-400 to-cyan-400 bg-clip-text text-transparent">{{ currentUser()?.fullName || 'User' }}</span>! 👋
            </h1>
            <p class="text-white/60 text-lg">
              You're successfully logged in to the Ticket Management System.
            </p>
          </div>
          <div class="hidden lg:flex items-center space-x-2 px-4 py-2 rounded-xl bg-emerald-500/20 border border-emerald-500/30">
            <span class="w-2 h-2 bg-emerald-400 rounded-full animate-pulse"></span>
            <span class="text-emerald-400 text-sm font-medium">Online</span>
          </div>
        </div>

        <div class="mt-6 p-4 bg-primary-500/10 border border-primary-500/20 rounded-xl">
          <div class="flex items-center space-x-4">
            <div class="w-14 h-14 bg-gradient-to-br from-primary-400 to-primary-600 rounded-xl flex items-center justify-center shadow-lg shadow-primary-500/30">
              <span class="text-white text-xl font-bold">{{ getInitials() }}</span>
            </div>
            <div>
              <p class="text-white/90 font-medium">{{ currentUser()?.email }}</p>
              <span class="inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold bg-primary-500/30 text-primary-300 border border-primary-500/30 mt-1">
                {{ currentUser()?.role }}
              </span>
            </div>
          </div>
        </div>
      </div>

      <!-- Quick Stats Cards -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <!-- Tickets Card -->
        <div class="group backdrop-blur-xl bg-white/5 border border-white/10 rounded-2xl p-6 hover:bg-white/[0.07] hover:border-primary-500/30 transition-all duration-300 hover:shadow-lg hover:shadow-primary-500/10">
          <div class="flex items-center justify-between mb-4">
            <div class="w-12 h-12 bg-primary-500/20 rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform duration-300">
              <svg class="w-6 h-6 text-primary-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 5v2m0 4v2m0 4v2M5 5a2 2 0 00-2 2v3a2 2 0 110 4v3a2 2 0 002 2h14a2 2 0 002-2v-3a2 2 0 110-4V7a2 2 0 00-2-2H5z"></path>
              </svg>
            </div>
            <span class="text-xs font-medium text-emerald-400 bg-emerald-500/20 px-2 py-1 rounded-full">+12%</span>
          </div>
          <p class="text-white/50 text-sm font-medium mb-1">Total Tickets</p>
          <p class="text-3xl font-bold text-white">128</p>
        </div>

        <!-- Active Card -->
        <div class="group backdrop-blur-xl bg-white/5 border border-white/10 rounded-2xl p-6 hover:bg-white/[0.07] hover:border-emerald-500/30 transition-all duration-300 hover:shadow-lg hover:shadow-emerald-500/10">
          <div class="flex items-center justify-between mb-4">
            <div class="w-12 h-12 bg-emerald-500/20 rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform duration-300">
              <svg class="w-6 h-6 text-emerald-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
              </svg>
            </div>
            <span class="text-xs font-medium text-emerald-400 bg-emerald-500/20 px-2 py-1 rounded-full">Active</span>
          </div>
          <p class="text-white/50 text-sm font-medium mb-1">Resolved Today</p>
          <p class="text-3xl font-bold text-white">24</p>
        </div>

        <!-- Pending Card -->
        <div class="group backdrop-blur-xl bg-white/5 border border-white/10 rounded-2xl p-6 hover:bg-white/[0.07] hover:border-amber-500/30 transition-all duration-300 hover:shadow-lg hover:shadow-amber-500/10">
          <div class="flex items-center justify-between mb-4">
            <div class="w-12 h-12 bg-amber-500/20 rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform duration-300">
              <svg class="w-6 h-6 text-amber-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path>
              </svg>
            </div>
            <span class="text-xs font-medium text-amber-400 bg-amber-500/20 px-2 py-1 rounded-full">Pending</span>
          </div>
          <p class="text-white/50 text-sm font-medium mb-1">In Progress</p>
          <p class="text-3xl font-bold text-white">18</p>
        </div>

        <!-- Users Card -->
        <div class="group backdrop-blur-xl bg-white/5 border border-white/10 rounded-2xl p-6 hover:bg-white/[0.07] hover:border-cyan-500/30 transition-all duration-300 hover:shadow-lg hover:shadow-cyan-500/10">
          <div class="flex items-center justify-between mb-4">
            <div class="w-12 h-12 bg-cyan-500/20 rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform duration-300">
              <svg class="w-6 h-6 text-cyan-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"></path>
              </svg>
            </div>
            <span class="text-xs font-medium text-cyan-400 bg-cyan-500/20 px-2 py-1 rounded-full">Team</span>
          </div>
          <p class="text-white/50 text-sm font-medium mb-1">Active Users</p>
          <p class="text-3xl font-bold text-white">42</p>
        </div>
      </div>

      <!-- Getting Started Section -->
      <div class="backdrop-blur-xl bg-white/5 border border-white/10 rounded-2xl p-8">
        <h2 class="text-2xl font-bold text-white mb-6 flex items-center">
          <span class="w-8 h-8 bg-primary-500/30 rounded-lg flex items-center justify-center mr-3">
            <svg class="w-4 h-4 text-primary-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"></path>
            </svg>
          </span>
          Quick Actions
        </h2>

        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <!-- Create Tickets -->
          <div class="group p-6 bg-white/5 border border-white/10 rounded-xl hover:bg-primary-500/10 hover:border-primary-500/30 transition-all duration-300 cursor-pointer">
            <div class="w-12 h-12 bg-primary-500/20 rounded-xl flex items-center justify-center mb-4 group-hover:scale-110 group-hover:bg-primary-500/30 transition-all duration-300">
              <svg class="w-6 h-6 text-primary-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path>
              </svg>
            </div>
            <h3 class="text-lg font-semibold text-white mb-2">Create Ticket</h3>
            <p class="text-sm text-white/50">Submit and track support tickets for your team.</p>
          </div>

          <!-- Manage Tickets -->
          <div class="group p-6 bg-white/5 border border-white/10 rounded-xl hover:bg-emerald-500/10 hover:border-emerald-500/30 transition-all duration-300 cursor-pointer">
            <div class="w-12 h-12 bg-emerald-500/20 rounded-xl flex items-center justify-center mb-4 group-hover:scale-110 group-hover:bg-emerald-500/30 transition-all duration-300">
              <svg class="w-6 h-6 text-emerald-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"></path>
              </svg>
            </div>
            <h3 class="text-lg font-semibold text-white mb-2">Manage Tickets</h3>
            <p class="text-sm text-white/50">View, update, and resolve existing tickets.</p>
          </div>

          <!-- Team Collaboration -->
          <div class="group p-6 bg-white/5 border border-white/10 rounded-xl hover:bg-cyan-500/10 hover:border-cyan-500/30 transition-all duration-300 cursor-pointer">
            <div class="w-12 h-12 bg-cyan-500/20 rounded-xl flex items-center justify-center mb-4 group-hover:scale-110 group-hover:bg-cyan-500/30 transition-all duration-300">
              <svg class="w-6 h-6 text-cyan-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"></path>
              </svg>
            </div>
            <h3 class="text-lg font-semibold text-white mb-2">Team Collaboration</h3>
            <p class="text-sm text-white/50">Work together with your team members.</p>
          </div>
        </div>
      </div>

      <!-- Status Footer -->
      <div class="text-center py-4">
        <p class="text-white/40 text-sm flex items-center justify-center space-x-2">
          <span class="w-2 h-2 bg-emerald-400 rounded-full animate-pulse"></span>
          <span>System is running smoothly</span>
          <span class="text-white/20">•</span>
          <span>Last login: {{ formatCurrentTime() }}</span>
        </p>
      </div>
    </div>
  `,
  styles: []
})
export class DashboardComponent {
  currentUser = computed(() => this.authState.currentUser());

  constructor(private authState: AuthState) {
    console.log('✅ Dashboard loaded successfully');
    console.log('👤 Current user:', this.currentUser());
  }

  getInitials(): string {
    const name = this.currentUser()?.fullName || 'U';
    return name.split(' ').map(n => n[0]).join('').toUpperCase().slice(0, 2);
  }

  formatCurrentTime(): string {
    return new Date().toLocaleString();
  }
}
