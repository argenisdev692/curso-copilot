import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-users-container',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  template: `
    <div class="space-y-6">
      <div class="bg-white rounded-lg shadow-sm p-6">
        <h1 class="text-3xl font-bold text-gray-900 mb-2">Users Management</h1>
        <p class="text-gray-600">Manage system users and their permissions</p>
      </div>

      <!-- Router outlet for child routes -->
      <router-outlet></router-outlet>
    </div>
  `
})
export class UsersContainer implements OnInit {
  constructor() {}

  ngOnInit(): void {
    // Initialize users container
  }
}
