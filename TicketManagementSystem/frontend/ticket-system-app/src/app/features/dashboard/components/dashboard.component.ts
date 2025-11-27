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
    <!-- Dashboard Content -->
    <div class="min-h-screen bg-gray-50 p-8" data-cy="dashboard">
      <!-- Welcome section -->
      <div class="bg-white rounded-lg shadow-md p-8 mb-8">
        <h1 class="text-4xl font-bold text-gray-900 mb-2">
          Welcome, {{ currentUser()?.fullName || 'User' }}! 👋
        </h1>
        <p class="text-gray-600 text-lg">
          You're successfully logged in to the Ticket Management System.
        </p>
        <div class="mt-4 p-4 bg-blue-50 border border-blue-200 rounded-lg">
          <p class="text-sm text-blue-800">
            <strong>User Details:</strong><br>
            Email: {{ currentUser()?.email }}<br>
            Role: <span class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
              {{ currentUser()?.role }}
            </span>
          </p>
        </div>
      </div>

      <!-- Quick Stats Cards -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <div class="bg-white rounded-lg shadow-md p-6 border-l-4 border-blue-500">
          <div class="flex items-center">
            <div class="p-3 bg-blue-100 rounded-full">
              <svg class="w-8 h-8 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path>
              </svg>
            </div>
            <div class="ml-4">
              <p class="text-sm font-medium text-gray-600">Dashboard</p>
              <p class="text-2xl font-semibold text-gray-900">Ready</p>
            </div>
          </div>
        </div>

        <div class="bg-white rounded-lg shadow-md p-6 border-l-4 border-green-500">
          <div class="flex items-center">
            <div class="p-3 bg-green-100 rounded-full">
              <svg class="w-8 h-8 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
              </svg>
            </div>
            <div class="ml-4">
              <p class="text-sm font-medium text-gray-600">Auth Status</p>
              <p class="text-2xl font-semibold text-gray-900">Active</p>
            </div>
          </div>
        </div>

        <div class="bg-white rounded-lg shadow-md p-6 border-l-4 border-purple-500">
          <div class="flex items-center">
            <div class="p-3 bg-purple-100 rounded-full">
              <svg class="w-8 h-8 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path>
              </svg>
            </div>
            <div class="ml-4">
              <p class="text-sm font-medium text-gray-600">Your Role</p>
              <p class="text-2xl font-semibold text-gray-900">{{ currentUser()?.role }}</p>
            </div>
          </div>
        </div>

        <div class="bg-white rounded-lg shadow-md p-6 border-l-4 border-yellow-500">
          <div class="flex items-center">
            <div class="p-3 bg-yellow-100 rounded-full">
              <svg class="w-8 h-8 text-yellow-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"></path>
              </svg>
            </div>
            <div class="ml-4">
              <p class="text-sm font-medium text-gray-600">System</p>
              <p class="text-2xl font-semibold text-gray-900">Online</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Info Section -->
      <div class="bg-white rounded-lg shadow-md p-6">
        <h2 class="text-2xl font-semibold text-gray-900 mb-4">Getting Started</h2>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
          <div class="p-4 border border-gray-200 rounded-lg hover:shadow-md transition-shadow">
            <div class="text-blue-600 mb-2">
              <svg class="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path>
              </svg>
            </div>
            <h3 class="text-lg font-semibold text-gray-900 mb-2">Create Tickets</h3>
            <p class="text-sm text-gray-600">Submit and track support tickets for your team.</p>
          </div>

          <div class="p-4 border border-gray-200 rounded-lg hover:shadow-md transition-shadow">
            <div class="text-green-600 mb-2">
              <svg class="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"></path>
              </svg>
            </div>
            <h3 class="text-lg font-semibold text-gray-900 mb-2">Manage Tickets</h3>
            <p class="text-sm text-gray-600">View, update, and resolve existing tickets.</p>
          </div>

          <div class="p-4 border border-gray-200 rounded-lg hover:shadow-md transition-shadow">
            <div class="text-purple-600 mb-2">
              <svg class="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"></path>
              </svg>
            </div>
            <h3 class="text-lg font-semibold text-gray-900 mb-2">Team Collaboration</h3>
            <p class="text-sm text-gray-600">Work together with your team members.</p>
          </div>
        </div>
      </div>

      <!-- Status Footer -->
      <div class="mt-8 text-center text-sm text-gray-500">
        <p>✨ System is running smoothly • Last login: {{ formatCurrentTime() }}</p>
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

  formatCurrentTime(): string {
    return new Date().toLocaleString();
  }
}
