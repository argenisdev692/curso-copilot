import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-tickets-container',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  template: `
    <div class="space-y-6">
      <div class="bg-white rounded-lg shadow-sm p-6">
        <h1 class="text-3xl font-bold text-gray-900 mb-2">Tickets Management</h1>
        <p class="text-gray-600">Manage and track all support tickets</p>
      </div>

      <!-- Router outlet for child routes -->
      <router-outlet></router-outlet>
    </div>
  `
})
export class TicketsContainer implements OnInit {
  constructor() {}

  ngOnInit(): void {
    // Initialize tickets container
  }
}
