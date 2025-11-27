import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading',
  template: `
    <div class="loading-container" [class.overlay]="overlay">
      <div class="loading-spinner" [class]="sizeClass">
        <div class="spinner-ring"></div>
        <div class="spinner-ring"></div>
        <div class="spinner-ring"></div>
        <div class="spinner-ring"></div>
      </div>

      <div *ngIf="message" class="loading-message">
        {{ message }}
      </div>
    </div>
  `,
  styles: [`
    .loading-container {
      @apply flex flex-col items-center justify-center;
    }

    .loading-container.overlay {
      @apply fixed inset-0 bg-black bg-opacity-50 z-50;
      @apply flex items-center justify-center;
    }

    .loading-spinner {
      @apply relative;
    }

    .loading-spinner.sm {
      @apply w-4 h-4;
    }

    .loading-spinner.md {
      @apply w-8 h-8;
    }

    .loading-spinner.lg {
      @apply w-12 h-12;
    }

    .loading-spinner.xl {
      @apply w-16 h-16;
    }

    .spinner-ring {
      @apply absolute border-2 border-transparent rounded-full;
      @apply animate-spin;
    }

    .spinner-ring:nth-child(1) {
      @apply border-t-blue-600;
      animation-delay: -0.45s;
    }

    .spinner-ring:nth-child(2) {
      @apply border-t-green-600;
      animation-delay: -0.3s;
    }

    .spinner-ring:nth-child(3) {
      @apply border-t-yellow-600;
      animation-delay: -0.15s;
    }

    .spinner-ring:nth-child(4) {
      @apply border-t-red-600;
      animation-delay: 0s;
    }

    .loading-message {
      @apply mt-2 text-sm text-gray-600 text-center;
    }

    .loading-container.overlay .loading-message {
      @apply text-white;
    }
  `]
})
export class LoadingComponent {
  @Input() size: 'sm' | 'md' | 'lg' | 'xl' = 'md';
  @Input() message = '';
  @Input() overlay = false;

  get sizeClass(): string {
    return this.size;
  }
}
