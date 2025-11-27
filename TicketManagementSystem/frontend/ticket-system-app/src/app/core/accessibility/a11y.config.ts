import { Directive, ElementRef, HostListener, Input, OnInit, inject } from '@angular/core';

/**
 * Accessibility (a11y) utilities and directives for Angular applications
 */

// Skip link directive for keyboard navigation
@Directive({
  selector: '[appSkipLink]',
  standalone: true
})
export class SkipLinkDirective implements OnInit {
  @Input() skipTo: string = '';

  private element = inject(ElementRef<HTMLElement>);

  ngOnInit(): void {
    this.element.nativeElement.style.cssText = `
      position: absolute;
      top: -40px;
      left: 6px;
      background: #000;
      color: #fff;
      padding: 8px;
      text-decoration: none;
      z-index: 100;
      border-radius: 4px;
    `;

    this.element.nativeElement.addEventListener('focus', () => {
      this.element.nativeElement.style.top = '6px';
    });

    this.element.nativeElement.addEventListener('blur', () => {
      this.element.nativeElement.style.top = '-40px';
    });
  }

  @HostListener('click')
  onClick(): void {
    const target = document.getElementById(this.skipTo);
    if (target) {
      target.focus();
      target.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }
}

// Focus trap directive for modals and dialogs
@Directive({
  selector: '[appFocusTrap]',
  standalone: true
})
export class FocusTrapDirective implements OnInit {
  private element = inject(ElementRef<HTMLElement>);

  ngOnInit(): void {
    // Focus trap logic would be implemented here
    // This is a simplified version
  }
}

// High contrast mode detector
export class HighContrastModeDetector {
  private static mediaQuery = window.matchMedia('(prefers-contrast: high)');

  static isHighContrast(): boolean {
    return this.mediaQuery.matches;
  }

  static onHighContrastChange(callback: (isHighContrast: boolean) => void): () => void {
    const handler = (event: MediaQueryListEvent) => callback(event.matches);
    this.mediaQuery.addEventListener('change', handler);
    return () => this.mediaQuery.removeEventListener('change', handler);
  }
}

// Reduced motion detector
export class ReducedMotionDetector {
  private static mediaQuery = window.matchMedia('(prefers-reduced-motion: reduce)');

  static prefersReducedMotion(): boolean {
    return this.mediaQuery.matches;
  }

  static onReducedMotionChange(callback: (prefersReduced: boolean) => void): () => void {
    const handler = (event: MediaQueryListEvent) => callback(event.matches);
    this.mediaQuery.addEventListener('change', handler);
    return () => this.mediaQuery.removeEventListener('change', handler);
  }
}

// Screen reader utilities
export class ScreenReaderUtils {
  static announce(message: string, priority: 'polite' | 'assertive' = 'polite'): void {
    const announcement = document.createElement('div');
    announcement.setAttribute('aria-live', priority);
    announcement.setAttribute('aria-atomic', 'true');
    announcement.style.position = 'absolute';
    announcement.style.left = '-10000px';
    announcement.style.width = '1px';
    announcement.style.height = '1px';
    announcement.style.overflow = 'hidden';

    document.body.appendChild(announcement);
    announcement.textContent = message;

    setTimeout(() => {
      document.body.removeChild(announcement);
    }, 1000);
  }

  static announcePageLoad(pageTitle: string): void {
    this.announce(`Página cargada: ${pageTitle}`, 'assertive');
  }

  static announceError(message: string): void {
    this.announce(`Error: ${message}`, 'assertive');
  }

  static announceSuccess(message: string): void {
    this.announce(`Éxito: ${message}`, 'polite');
  }
}

// Keyboard navigation utilities
export class KeyboardNavigationUtils {
  static readonly TAB_KEY = 'Tab';
  static readonly ENTER_KEY = 'Enter';
  static readonly SPACE_KEY = ' ';
  static readonly ESCAPE_KEY = 'Escape';
  static readonly ARROW_UP = 'ArrowUp';
  static readonly ARROW_DOWN = 'ArrowDown';
  static readonly ARROW_LEFT = 'ArrowLeft';
  static readonly ARROW_RIGHT = 'ArrowRight';

  static isNavigationKey(key: string): boolean {
    return [this.TAB_KEY, this.ARROW_UP, this.ARROW_DOWN, this.ARROW_LEFT, this.ARROW_RIGHT].includes(key);
  }

  static isActivationKey(key: string): boolean {
    return [this.ENTER_KEY, this.SPACE_KEY].includes(key);
  }

  static preventDefault(event: KeyboardEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  static focusNextElement(currentElement: HTMLElement, direction: 'forward' | 'backward' = 'forward'): void {
    const focusableElements = this.getFocusableElements(document.body);
    const currentIndex = focusableElements.indexOf(currentElement);

    if (currentIndex === -1) return;

    let nextIndex: number;
    if (direction === 'forward') {
      nextIndex = currentIndex + 1 >= focusableElements.length ? 0 : currentIndex + 1;
    } else {
      nextIndex = currentIndex - 1 < 0 ? focusableElements.length - 1 : currentIndex - 1;
    }

    focusableElements[nextIndex]?.focus();
  }

  static getFocusableElements(container: HTMLElement = document.body): HTMLElement[] {
    const focusableSelectors = [
      'a[href]',
      'button:not([disabled])',
      'textarea:not([disabled])',
      'input:not([disabled])',
      'select:not([disabled])',
      '[tabindex]:not([tabindex="-1"])',
      '[contenteditable="true"]'
    ];

    return Array.from(container.querySelectorAll(focusableSelectors.join(', ')))
      .filter((element: any) => {
        const rect = element.getBoundingClientRect();
        return rect.width > 0 && rect.height > 0 && window.getComputedStyle(element).visibility !== 'hidden';
      }) as HTMLElement[];
  }
}

// ARIA utilities
export class AriaUtils {
  static setAriaLabel(element: HTMLElement, label: string): void {
    element.setAttribute('aria-label', label);
  }

  static setAriaDescribedBy(element: HTMLElement, descriptionId: string): void {
    element.setAttribute('aria-describedby', descriptionId);
  }

  static setAriaExpanded(element: HTMLElement, expanded: boolean): void {
    element.setAttribute('aria-expanded', expanded.toString());
  }

  static setAriaHidden(element: HTMLElement, hidden: boolean): void {
    element.setAttribute('aria-hidden', hidden.toString());
  }

  static setRole(element: HTMLElement, role: string): void {
    element.setAttribute('role', role);
  }

  static createAriaDescription(description: string): string {
    const id = `aria-desc-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
    const div = document.createElement('div');
    div.id = id;
    div.textContent = description;
    div.style.position = 'absolute';
    div.style.left = '-10000px';
    div.style.width = '1px';
    div.style.height = '1px';
    div.style.overflow = 'hidden';

    document.body.appendChild(div);
    return id;
  }

  static updateLiveRegion(message: string, priority: 'polite' | 'assertive' = 'polite'): void {
    let liveRegion = document.getElementById('aria-live-region');
    if (!liveRegion) {
      liveRegion = document.createElement('div');
      liveRegion.id = 'aria-live-region';
      liveRegion.setAttribute('aria-live', priority);
      liveRegion.setAttribute('aria-atomic', 'true');
      liveRegion.style.position = 'absolute';
      liveRegion.style.left = '-10000px';
      liveRegion.style.width = '1px';
      liveRegion.style.height = '1px';
      liveRegion.style.overflow = 'hidden';
      document.body.appendChild(liveRegion);
    }

    liveRegion.textContent = message;
  }
}

// Color contrast utilities
export class ColorContrastUtils {
  static readonly WCAG_AA_NORMAL_TEXT = 4.5;
  static readonly WCAG_AA_LARGE_TEXT = 3.0;
  static readonly WCAG_AAA_NORMAL_TEXT = 7.0;
  static readonly WCAG_AAA_LARGE_TEXT = 4.5;

  static calculateContrastRatio(color1: string, color2: string): number {
    const lum1 = this.getLuminance(color1);
    const lum2 = this.getLuminance(color2);
    const brightest = Math.max(lum1, lum2);
    const darkest = Math.min(lum1, lum2);
    return (brightest + 0.05) / (darkest + 0.05);
  }

  static passesWCAG(color1: string, color2: string, level: 'AA' | 'AAA' = 'AA', isLargeText: boolean = false): boolean {
    const ratio = this.calculateContrastRatio(color1, color2);
    const threshold = level === 'AA'
      ? (isLargeText ? this.WCAG_AA_LARGE_TEXT : this.WCAG_AA_NORMAL_TEXT)
      : (isLargeText ? this.WCAG_AAA_LARGE_TEXT : this.WCAG_AAA_NORMAL_TEXT);

    return ratio >= threshold;
  }

  private static getLuminance(color: string): number {
    // Convert hex to RGB
    const hex = color.replace('#', '');
    const r = parseInt(hex.substr(0, 2), 16) / 255;
    const g = parseInt(hex.substr(2, 2), 16) / 255;
    const b = parseInt(hex.substr(4, 2), 16) / 255;

    // Calculate luminance
    const [rs, gs, bs] = [r, g, b].map(c =>
      c <= 0.03928 ? c / 12.92 : Math.pow((c + 0.055) / 1.055, 2.4)
    );

    return 0.2126 * rs + 0.7152 * gs + 0.0722 * bs;
  }

  static suggestAccessibleColor(backgroundColor: string, textColor: string): string {
    const ratio = this.calculateContrastRatio(backgroundColor, textColor);

    if (ratio < this.WCAG_AA_NORMAL_TEXT) {
      // Suggest a darker or lighter color
      return textColor; // Placeholder - would implement color adjustment logic
    }

    return textColor;
  }
}

// Form accessibility utilities
export class FormAccessibilityUtils {
  static associateLabel(inputElement: HTMLInputElement, labelElement: HTMLLabelElement): void {
    const inputId = inputElement.id || `input-${Date.now()}`;
    inputElement.id = inputId;
    labelElement.htmlFor = inputId;
  }

  static setErrorMessage(inputElement: HTMLInputElement, message: string): void {
    const errorId = `error-${inputElement.id || Date.now()}`;
    inputElement.setAttribute('aria-describedby', errorId);
    inputElement.setAttribute('aria-invalid', 'true');

    let errorElement = document.getElementById(errorId);
    if (!errorElement) {
      errorElement = document.createElement('div');
      errorElement.id = errorId;
      errorElement.setAttribute('role', 'alert');
      errorElement.style.color = 'red';
      errorElement.style.fontSize = '0.875rem';
      inputElement.parentNode?.insertBefore(errorElement, inputElement.nextSibling);
    }

    errorElement.textContent = message;
  }

  static clearErrorMessage(inputElement: HTMLInputElement): void {
    const errorId = inputElement.getAttribute('aria-describedby');
    if (errorId) {
      const errorElement = document.getElementById(errorId);
      if (errorElement) {
        errorElement.remove();
      }
      inputElement.removeAttribute('aria-describedby');
      inputElement.setAttribute('aria-invalid', 'false');
    }
  }

  static validateField(inputElement: HTMLInputElement, validators: ((value: string) => string | null)[]): boolean {
    const value = inputElement.value;
    let isValid = true;

    for (const validator of validators) {
      const error = validator(value);
      if (error) {
        this.setErrorMessage(inputElement, error);
        isValid = false;
        break;
      }
    }

    if (isValid) {
      this.clearErrorMessage(inputElement);
    }

    return isValid;
  }
}
