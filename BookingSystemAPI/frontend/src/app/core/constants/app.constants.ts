/**
 * Application constants
 */
export const APP_CONSTANTS = {
  /** Application name */
  APP_NAME: 'Booking System',

  /** Default pagination settings */
  PAGINATION: {
    DEFAULT_PAGE_SIZE: 10,
    PAGE_SIZE_OPTIONS: [5, 10, 25, 50],
  },

  /** Date/time formats */
  DATE_FORMATS: {
    DATE: 'dd/MM/yyyy',
    TIME: 'HH:mm',
    DATETIME: 'dd/MM/yyyy HH:mm',
    ISO: "yyyy-MM-dd'T'HH:mm:ss",
  },

  /** Local storage keys */
  STORAGE_KEYS: {
    THEME: 'app-theme',
    LANGUAGE: 'app-language',
    USER_PREFERENCES: 'user-preferences',
  },

  /** HTTP request timeouts (ms) */
  TIMEOUTS: {
    DEFAULT: 30000,
    UPLOAD: 120000,
  },
} as const;
