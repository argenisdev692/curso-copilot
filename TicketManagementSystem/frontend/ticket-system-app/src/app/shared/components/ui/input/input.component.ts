import { Component, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-input',
  template: `
    <div class="input-container">
      <label *ngIf="label" [for]="inputId" class="label">
        {{ label }}
        <span *ngIf="required" class="required">*</span>
      </label>

      <div class="relative">
        <input
          [id]="inputId"
          [type]="type"
          [placeholder]="placeholder"
          [disabled]="disabled"
          [readonly]="readonly"
          [value]="value"
          (input)="onInput($event)"
          (blur)="onBlur()"
          (focus)="onFocus()"
          class="input-field"
          [class.error]="hasError"
          [class.success]="hasSuccess"
        />

        <div *ngIf="icon" class="icon-container">
          <i [class]="icon"></i>
        </div>
      </div>

      <div *ngIf="hint && !hasError" class="hint">
        {{ hint }}
      </div>

      <div *ngIf="hasError" class="error-message">
        {{ errorMessage }}
      </div>
    </div>
  `,
  styles: [`
    .input-container {
      @apply mb-4;
    }

    .label {
      @apply block text-sm font-medium text-gray-700 mb-1;
    }

    .required {
      @apply text-red-500 ml-1;
    }

    .input-field {
      @apply w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm;
      @apply placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500;
      @apply disabled:bg-gray-50 disabled:text-gray-500 disabled:cursor-not-allowed;
      @apply transition-colors duration-200;
    }

    .input-field.error {
      @apply border-red-300 focus:ring-red-500 focus:border-red-500;
    }

    .input-field.success {
      @apply border-green-300 focus:ring-green-500 focus:border-green-500;
    }

    .icon-container {
      @apply absolute inset-y-0 right-0 pr-3 flex items-center pointer-events-none;
    }

    .icon-container i {
      @apply text-gray-400;
    }

    .hint {
      @apply mt-1 text-sm text-gray-500;
    }

    .error-message {
      @apply mt-1 text-sm text-red-600;
    }
  `],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true
    }
  ]
})
export class InputComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() placeholder = '';
  @Input() type: 'text' | 'email' | 'password' | 'number' | 'tel' = 'text';
  @Input() disabled = false;
  @Input() readonly = false;
  @Input() required = false;
  @Input() hint = '';
  @Input() errorMessage = '';
  @Input() icon = '';
  @Input() hasError = false;
  @Input() hasSuccess = false;

  @Output() blur = new EventEmitter<void>();
  @Output() focus = new EventEmitter<void>();

  inputId = `input-${Math.random().toString(36).substr(2, 9)}`;
  value = '';

  private onChange = (value: any) => {};
  private onTouched = () => {};

  writeValue(value: any): void {
    this.value = value || '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.value = target.value;
    this.onChange(this.value);
  }

  onBlur(): void {
    this.onTouched();
    this.blur.emit();
  }

  onFocus(): void {
    this.focus.emit();
  }
}
