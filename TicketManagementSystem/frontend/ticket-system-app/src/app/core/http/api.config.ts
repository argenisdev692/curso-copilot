export const API_CONFIG = {
  baseUrl: 'http://localhost:5201/api',
  timeout: 30000,
  retries: 3,
  version: 'v1'
} as const;

export const API_ENDPOINTS = {
  auth: {
    login: '/auth/login',
    register: '/auth/register',
    refresh: '/auth/refresh',
    logout: '/auth/logout',
    profile: '/auth/profile'
  },
  tickets: {
    list: '/tickets',
    detail: (id: number) => `/tickets/${id}`,
    create: '/tickets',
    update: (id: number) => `/tickets/${id}`,
    delete: (id: number) => `/tickets/${id}`,
    assign: (id: number) => `/tickets/${id}/assign`,
    status: (id: number) => `/tickets/${id}/status`
  },
  dashboard: {
    stats: '/dashboard/stats',
    activity: '/dashboard/activity',
    charts: '/dashboard/charts'
  },
  users: {
    list: '/users',
    detail: (id: number) => `/users/${id}`,
    create: '/users',
    update: (id: number) => `/users/${id}`,
    delete: (id: number) => `/users/${id}`,
    roles: '/users/roles'
  }
} as const;

export const HTTP_STATUS = {
  OK: 200,
  CREATED: 201,
  NO_CONTENT: 204,
  BAD_REQUEST: 400,
  UNAUTHORIZED: 401,
  FORBIDDEN: 403,
  NOT_FOUND: 404,
  CONFLICT: 409,
  INTERNAL_SERVER_ERROR: 500
} as const;
