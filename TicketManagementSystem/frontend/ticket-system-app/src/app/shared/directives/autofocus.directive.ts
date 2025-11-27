import { Directive, ElementRef, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[appAutofocus]'
})
export class AutofocusDirective implements AfterViewInit {
  constructor(private elementRef: ElementRef) {}

  ngAfterViewInit() {
    // Small delay to ensure the element is fully rendered
    setTimeout(() => {
      this.elementRef.nativeElement.focus();
    }, 100);
  }
}
