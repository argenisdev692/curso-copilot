import { ChangeDetectionStrategy, Component, OnInit, OnDestroy, signal, computed, effect, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, takeUntil, debounceTime, distinctUntilChanged } from 'rxjs';

/**
 * Performance utilities and patterns for Angular applications
 */

// TrackBy functions for optimal *ngFor performance
export class TrackByUtils {
  static trackById = (index: number, item: any): any => item?.id ?? index;

  static trackByIndex = (index: number): number => index;

  static trackByProperty = (property: string) => (index: number, item: any): any => item?.[property] ?? index;

  static trackByValue = (index: number, item: any): any => item;
}

// Memoization decorator for expensive computations
export function memoize(target: any, propertyName: string, descriptor: PropertyDescriptor) {
  const method = descriptor.value;
  const cacheKey = `__memoize_cache_${propertyName}`;

  descriptor.value = function(...args: any[]) {
    if (!this[cacheKey]) {
      this[cacheKey] = new Map();
    }

    const key = JSON.stringify(args);
    if (!this[cacheKey].has(key)) {
      this[cacheKey].set(key, method.apply(this, args));
    }

    return this[cacheKey].get(key);
  };

  return descriptor;
}

// Performance monitoring utilities
export class PerformanceMonitor {
  private static marks = new Map<string, number>();
  private static measures = new Map<string, number>();

  static startMark(name: string): void {
    this.marks.set(name, performance.now());
  }

  static endMark(name: string): number {
    const startTime = this.marks.get(name);
    if (!startTime) {
      console.warn(`Performance mark '${name}' not found`);
      return 0;
    }

    const duration = performance.now() - startTime;
    this.measures.set(name, duration);
    console.log(`⏱️ ${name}: ${duration.toFixed(2)}ms`);
    return duration;
  }

  static measureFunction<T>(name: string, fn: () => T): T {
    this.startMark(name);
    try {
      const result = fn();
      this.endMark(name);
      return result;
    } catch (error) {
      this.endMark(name);
      throw error;
    }
  }

  static async measureAsyncFunction<T>(name: string, fn: () => Promise<T>): Promise<T> {
    this.startMark(name);
    try {
      const result = await fn();
      this.endMark(name);
      return result;
    } catch (error) {
      this.endMark(name);
      throw error;
    }
  }

  static getMetrics(): Record<string, number> {
    return Object.fromEntries(this.measures);
  }

  static clearMetrics(): void {
    this.marks.clear();
    this.measures.clear();
  }
}

// Optimized base component with performance features
@Component({
  template: '',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export abstract class OptimizedComponent implements OnInit, OnDestroy {
  protected destroy$ = new Subject<void>();

  // Loading states
  protected loading = signal(false);
  protected error = signal<string | null>(null);

  // Performance monitoring
  protected componentName = this.constructor.name;

  ngOnInit(): void {
    PerformanceMonitor.startMark(`${this.componentName}_init`);
    this.initializeComponent();
    PerformanceMonitor.endMark(`${this.componentName}_init`);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    PerformanceMonitor.clearMetrics();
  }

  protected abstract initializeComponent(): void;

  // Debounced search utility
  protected createDebouncedSearch<T>(
    searchFn: (query: string) => void,
    debounceMs: number = 300
  ) {
    return new Subject<string>().pipe(
      takeUntil(this.destroy$),
      debounceTime(debounceMs),
      distinctUntilChanged()
    ).subscribe(searchFn);
  }

  // Computed signal utilities
  protected createComputedSignal<T>(
    computation: () => T,
    debugName?: string
  ) {
    const computedSignal = computed(computation);

    if (debugName) {
      effect(() => {
        const value = computedSignal();
        console.log(`🔄 ${debugName}:`, value);
      });
    }

    return computedSignal;
  }

  // Error boundary pattern
  protected handleError(error: any, context: string): void {
    console.error(`❌ Error in ${this.componentName}.${context}:`, error);
    this.error.set(error.message || 'An unexpected error occurred');
  }

  // Loading state management
  protected setLoading(state: boolean): void {
    this.loading.set(state);
  }

  protected clearError(): void {
    this.error.set(null);
  }
}

// Virtual scrolling utilities for large lists
export class VirtualScrollUtils {
  static readonly DEFAULT_ITEM_HEIGHT = 50;
  static readonly BUFFER_SIZE = 10;

  static calculateVisibleRange(
    scrollTop: number,
    containerHeight: number,
    itemHeight: number = this.DEFAULT_ITEM_HEIGHT,
    totalItems: number
  ): { start: number; end: number } {
    const start = Math.floor(scrollTop / itemHeight);
    const visibleCount = Math.ceil(containerHeight / itemHeight);
    const end = Math.min(start + visibleCount + this.BUFFER_SIZE, totalItems);

    return {
      start: Math.max(0, start - this.BUFFER_SIZE),
      end
    };
  }

  static getItemStyle(index: number, itemHeight: number = this.DEFAULT_ITEM_HEIGHT): Record<string, string> {
    return {
      position: 'absolute',
      top: `${index * itemHeight}px`,
      height: `${itemHeight}px`,
      width: '100%'
    };
  }
}

// Image optimization utilities
export class ImageOptimizationUtils {
  static readonly SUPPORTED_FORMATS = ['webp', 'avif', 'jpg', 'png'];
  static readonly BREAKPOINTS = [480, 768, 1024, 1280, 1920];

  static generateSrcSet(imageUrl: string, widths: number[] = this.BREAKPOINTS): string {
    return widths
      .map(width => `${this.getOptimizedImageUrl(imageUrl, width)} ${width}w`)
      .join(', ');
  }

  static getOptimizedImageUrl(imageUrl: string, width: number, format: string = 'webp'): string {
    // This would integrate with your image CDN/service
    // Example: return `https://cdn.example.com/${width}x${format}/${imageUrl}`;
    return imageUrl; // Placeholder
  }

  static preloadCriticalImages(imageUrls: string[]): void {
    imageUrls.forEach(url => {
      const link = document.createElement('link');
      link.rel = 'preload';
      link.as = 'image';
      link.href = url;
      document.head.appendChild(link);
    });
  }

  static lazyLoadImage(
    imgElement: HTMLImageElement,
    src: string,
    placeholder?: string
  ): void {
    if (placeholder) {
      imgElement.src = placeholder;
    }

    const image = new Image();
    image.onload = () => {
      imgElement.src = src;
      imgElement.classList.add('loaded');
    };
    image.src = src;
  }
}

// Bundle size monitoring
export class BundleAnalyzer {
  static logBundleSize(): void {
    if (typeof performance !== 'undefined' && performance.getEntriesByType) {
      const resources = performance.getEntriesByType('resource') as PerformanceResourceTiming[];

      console.group('📦 Bundle Analysis');
      resources
        .filter(resource => resource.name.includes('.js'))
        .sort((a, b) => b.transferSize - a.transferSize)
        .slice(0, 10)
        .forEach(resource => {
          console.log(`${resource.name.split('/').pop()}: ${(resource.transferSize / 1024).toFixed(2)} KB`);
        });
      console.groupEnd();
    }
  }

  static monitorBundleGrowth(previousSize: number, currentSize: number): void {
    const growth = ((currentSize - previousSize) / previousSize) * 100;

    if (growth > 10) {
      console.warn(`⚠️ Bundle size increased by ${growth.toFixed(2)}%`);
    } else if (growth < -5) {
      console.log(`✅ Bundle size decreased by ${Math.abs(growth).toFixed(2)}%`);
    }
  }
}

// Web Vitals monitoring
export class WebVitalsMonitor {
  static init(): void {
    if (typeof window !== 'undefined') {
      this.monitorCLS();
      this.monitorFID();
      this.monitorLCP();
    }
  }

  private static monitorCLS(): void {
    let clsValue = 0;
    let clsEntries: any[] = [];

    const observer = new PerformanceObserver((list) => {
      for (const entry of list.getEntries()) {
        if (!(entry as any).hadRecentInput) {
          clsValue += (entry as any).value;
          clsEntries.push(entry);
        }
      }

      if (clsValue > 0.1) {
        console.warn(`⚠️ High CLS detected: ${clsValue}`);
      }
    });

    observer.observe({ entryTypes: ['layout-shift'] });
  }

  private static monitorFID(): void {
    const observer = new PerformanceObserver((list) => {
      for (const entry of list.getEntries()) {
        console.log(`👆 FID: ${(entry as any).processingStart - entry.startTime}ms`);
      }
    });

    observer.observe({ entryTypes: ['first-input'] });
  }

  private static monitorLCP(): void {
    const observer = new PerformanceObserver((list) => {
      const entries = list.getEntries();
      const lastEntry = entries[entries.length - 1] as any;

      console.log(`🏁 LCP: ${lastEntry.startTime}ms`);

      if (lastEntry.startTime > 2500) {
        console.warn('⚠️ Poor LCP detected');
      }
    });

    observer.observe({ entryTypes: ['largest-contentful-paint'] });
  }
}
