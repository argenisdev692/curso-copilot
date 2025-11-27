// Application constants

export const APP_CONSTANTS = {
  NAME: 'Ticket Management System',
  VERSION: '1.0.0',
  AUTHOR: 'Your Organization'
} as const;

export const PAGINATION_CONSTANTS = {
  DEFAULT_PAGE_SIZE: 10,
  MAX_PAGE_SIZE: 100,
  PAGE_SIZE_OPTIONS: [5, 10, 25, 50, 100]
} as const;

export const VALIDATION_CONSTANTS = {
  PASSWORD_MIN_LENGTH: 8,
  PASSWORD_MAX_LENGTH: 128,
  EMAIL_MAX_LENGTH: 254,
  NAME_MAX_LENGTH: 100,
  DESCRIPTION_MAX_LENGTH: 1000
} as const;

export const TIMEOUT_CONSTANTS = {
  DEBOUNCE_TIME: 300,
  API_TIMEOUT: 30000,
  NOTIFICATION_DURATION: 5000
} as const;

export const STORAGE_KEYS = {
  TOKEN: 'token',
  REFRESH_TOKEN: 'refreshToken',
  USER: 'user',
  TOKEN_EXPIRY: 'tokenExpiry',
  THEME: 'theme',
  LANGUAGE: 'language'
} as const;
