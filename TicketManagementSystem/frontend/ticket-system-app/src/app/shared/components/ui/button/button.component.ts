import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-button',
  template: `
    <button
      [type]="type"
      [disabled]="disabled || loading"
      [class]="buttonClass"
      (click)="onClick.emit($event)">
      <span *ngIf="loading" class="spinner"></span>
      <ng-content></ng-content>
    </button>
  `,
  styles: [`
    button {
      @apply px-4 py-2 rounded-md font-medium transition-colors duration-200;
      @apply disabled:opacity-50 disabled:cursor-not-allowed;
      @apply focus:outline-none focus:ring-2 focus:ring-offset-2;
    }

    .primary {
      @apply bg-blue-600 text-white hover:bg-blue-700;
      @apply focus:ring-blue-500;
    }

    .secondary {
      @apply bg-gray-200 text-gray-900 hover:bg-gray-300;
      @apply focus:ring-gray-500;
    }

    .danger {
      @apply bg-red-600 text-white hover:bg-red-700;
      @apply focus:ring-red-500;
    }

    .success {
      @apply bg-green-600 text-white hover:bg-green-700;
      @apply focus:ring-green-500;
    }

    .spinner {
      @apply inline-block w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin mr-2;
    }
  `]
})
export class ButtonComponent {
  @Input() type: 'button' | 'submit' = 'button';
  @Input() variant: 'primary' | 'secondary' | 'danger' | 'success' = 'primary';
  @Input() disabled = false;
  @Input() loading = false;
  @Input() size: 'sm' | 'md' | 'lg' = 'md';

  @Output() onClick = new EventEmitter<MouseEvent>();

  get buttonClass(): string {
    let classes = this.variant;

    // Size classes
    switch (this.size) {
      case 'sm':
        classes += ' text-sm px-3 py-1.5';
        break;
      case 'lg':
        classes += ' text-lg px-6 py-3';
        break;
      default:
        classes += ' text-base px-4 py-2';
    }

    return classes;
  }
}
