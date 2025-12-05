import { Directive, ElementRef, HostListener, inject, input } from '@angular/core';

/**
 * Directive to highlight element on hover
 */
@Directive({
  selector: '[appHighlight]',
  standalone: true,
})
export class HighlightDirective {
  private readonly el = inject(ElementRef);

  /** Highlight color */
  readonly highlightColor = input<string>('rgba(0, 0, 0, 0.04)');

  /** Default background color */
  readonly defaultColor = input<string>('transparent');

  @HostListener('mouseenter')
  onMouseEnter(): void {
    this.highlight(this.highlightColor());
  }

  @HostListener('mouseleave')
  onMouseLeave(): void {
    this.highlight(this.defaultColor());
  }

  private highlight(color: string): void {
    this.el.nativeElement.style.backgroundColor = color;
  }
}
