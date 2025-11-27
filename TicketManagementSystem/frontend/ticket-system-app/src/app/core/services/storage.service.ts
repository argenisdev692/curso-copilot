import { inject, InjectionToken } from '@angular/core';

/**
 * Storage abstraction service for type-safe localStorage/sessionStorage operations
 */
export class StorageService {
  private readonly storage: Storage;

  constructor(storage: Storage) {
    this.storage = storage;
  }

  /**
   * Get item from storage with type safety
   */
  getItem<T>(key: string): T | null {
    try {
      const item = this.storage.getItem(key);
      return item ? JSON.parse(item) : null;
    } catch (error) {
      console.error(`Error getting item ${key} from storage:`, error);
      return null;
    }
  }

  /**
   * Set item in storage with type safety
   */
  setItem<T>(key: string, value: T): void {
    try {
      this.storage.setItem(key, JSON.stringify(value));
    } catch (error) {
      console.error(`Error setting item ${key} in storage:`, error);
    }
  }

  /**
   * Remove item from storage
   */
  removeItem(key: string): void {
    try {
      this.storage.removeItem(key);
    } catch (error) {
      console.error(`Error removing item ${key} from storage:`, error);
    }
  }

  /**
   * Clear all items from storage
   */
  clear(): void {
    try {
      this.storage.clear();
    } catch (error) {
      console.error('Error clearing storage:', error);
    }
  }

  /**
   * Check if key exists in storage
   */
  hasItem(key: string): boolean {
    return this.storage.getItem(key) !== null;
  }
}

// Injection tokens for different storage types
export const LOCAL_STORAGE = new InjectionToken<Storage>('LocalStorage', {
  providedIn: 'root',
  factory: () => localStorage
});

export const SESSION_STORAGE = new InjectionToken<Storage>('SessionStorage', {
  providedIn: 'root',
  factory: () => sessionStorage
});

// Functional providers
export function provideLocalStorageService(): {
  provide: typeof StorageService;
  useFactory: () => StorageService;
} {
  return {
    provide: StorageService,
    useFactory: () => new StorageService(inject(LOCAL_STORAGE))
  };
}

export function provideSessionStorageService(): {
  provide: typeof StorageService;
  useFactory: () => StorageService;
} {
  return {
    provide: StorageService,
    useFactory: () => new StorageService(inject(SESSION_STORAGE))
  };
}
