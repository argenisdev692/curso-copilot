import { NgModule } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Meta } from '@angular/platform-browser';

/**
 * Security configuration module
 * Implements Content Security Policy and other security measures
 */
@NgModule({
  providers: [
    {
      provide: 'CSP_CONFIG',
      useValue: {
        // Content Security Policy directives
        defaultSrc: ["'self'"],
        scriptSrc: ["'self'", "'unsafe-inline'", "'unsafe-eval'"], // Relaxed for development
        styleSrc: ["'self'", "'unsafe-inline'", "https://fonts.googleapis.com"],
        fontSrc: ["'self'", "https://fonts.gstatic.com"],
        imgSrc: ["'self'", "data:", "https:", "blob:"],
        connectSrc: ["'self'", "https://api.example.com"], // Add your API domains
        frameSrc: ["'none'"],
        objectSrc: ["'none'"],
        baseUri: ["'self'"],
        formAction: ["'self'"],
        frameAncestors: ["'none'"],
        upgradeInsecureRequests: true
      }
    }
  ]
})
export class SecurityConfigModule {
  constructor(
    private meta: Meta,
    private sanitizer: DomSanitizer
  ) {
    this.configureSecurityHeaders();
  }

  private configureSecurityHeaders(): void {
    // Add security meta tags
    this.meta.addTags([
      {
        name: 'referrer',
        content: 'strict-origin-when-cross-origin'
      },
      {
        'http-equiv': 'X-Content-Type-Options',
        content: 'nosniff'
      },
      {
        'http-equiv': 'X-Frame-Options',
        content: 'DENY'
      },
      {
        'http-equiv': 'X-XSS-Protection',
        content: '1; mode=block'
      },
      {
        'http-equiv': 'Strict-Transport-Security',
        content: 'max-age=31536000; includeSubDomains'
      },
      {
        name: 'viewport',
        content: 'width=device-width, initial-scale=1, shrink-to-fit=no'
      }
    ]);

    // Configure CSP in production
    if (this.isProduction()) {
      this.configureCSP();
    }
  }

  private configureCSP(): void {
    const csp = [
      "default-src 'self'",
      "script-src 'self' 'unsafe-inline' 'unsafe-eval'",
      "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com",
      "font-src 'self' https://fonts.gstatic.com",
      "img-src 'self' data: https: blob:",
      "connect-src 'self' https://api.example.com",
      "frame-src 'none'",
      "object-src 'none'",
      "base-uri 'self'",
      "form-action 'self'",
      "frame-ancestors 'none'",
      "upgrade-insecure-requests"
    ].join('; ');

    this.meta.addTag({
      'http-equiv': 'Content-Security-Policy',
      content: csp
    });
  }

  private isProduction(): boolean {
    return !window.location.hostname.includes('localhost') &&
           !window.location.hostname.includes('127.0.0.1');
  }
}

// Security utilities
export class SecurityUtils {
  /**
   * Sanitize user input for display
   */
  static sanitizeInput(input: string): string {
    if (!input) return '';

    return input
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#x27;')
      .replace(/\//g, '&#x2F;');
  }

  /**
   * Validate email format
   */
  static isValidEmail(email: string): boolean {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(email);
  }

  /**
   * Validate password strength
   */
  static validatePasswordStrength(password: string): {
    isValid: boolean;
    score: number;
    feedback: string[];
  } {
    const feedback: string[] = [];
    let score = 0;

    if (password.length >= 8) score++;
    else feedback.push('Use at least 8 characters');

    if (/[a-z]/.test(password)) score++;
    else feedback.push('Include lowercase letters');

    if (/[A-Z]/.test(password)) score++;
    else feedback.push('Include uppercase letters');

    if (/[0-9]/.test(password)) score++;
    else feedback.push('Include numbers');

    if (/[^A-Za-z0-9]/.test(password)) score++;
    else feedback.push('Include special characters');

    return {
      isValid: score >= 4,
      score,
      feedback
    };
  }

  /**
   * Generate secure random string
   */
  static generateSecureToken(length: number = 32): string {
    const array = new Uint8Array(length);
    crypto.getRandomValues(array);
    return Array.from(array, byte => byte.toString(16).padStart(2, '0')).join('');
  }

  /**
   * Hash sensitive data for logging (never log raw sensitive data)
   */
  static hashForLogging(data: string): string {
    // Simple hash for logging purposes only
    let hash = 0;
    for (let i = 0; i < data.length; i++) {
      const char = data.charCodeAt(i);
      hash = ((hash << 5) - hash) + char;
      hash = hash & hash; // Convert to 32-bit integer
    }
    return hash.toString(16);
  }

  /**
   * Check if running in secure context
   */
  static isSecureContext(): boolean {
    return window.isSecureContext;
  }

  /**
   * Detect potential XSS attempts
   */
  static containsXSSAttempt(input: string): boolean {
    const xssPatterns = [
      /<script/i,
      /javascript:/i,
      /on\w+\s*=/i,
      /<iframe/i,
      /<object/i,
      /<embed/i,
      /expression\s*\(/i,
      /vbscript:/i,
      /data:text\/html/i
    ];

    return xssPatterns.some(pattern => pattern.test(input));
  }
}

// CSRF Protection utilities
export class CSRFProtection {
  private static token: string | null = null;

  static generateToken(): string {
    if (!this.token) {
      this.token = SecurityUtils.generateSecureToken(32);
      // Store in sessionStorage for same-session validity
      sessionStorage.setItem('csrf-token', this.token);
    }
    return this.token;
  }

  static getToken(): string | null {
    if (!this.token) {
      this.token = sessionStorage.getItem('csrf-token');
    }
    return this.token;
  }

  static validateToken(token: string): boolean {
    return this.getToken() === token;
  }

  static clearToken(): void {
    this.token = null;
    sessionStorage.removeItem('csrf-token');
  }
}
