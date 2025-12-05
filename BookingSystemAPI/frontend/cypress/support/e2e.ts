// ***********************************************************
// This file is processed and loaded automatically before your test files.
// ***********************************************************

// Import commands
import './commands';

// Cypress commands types
declare global {
  namespace Cypress {
    interface Chainable {
      /**
       * Custom command to login via API
       * @example cy.login('user@example.com', 'password123')
       */
      login(email: string, password: string): Chainable<void>;

      /**
       * Custom command to get element by data-testid attribute
       * @example cy.getByTestId('submit-button')
       */
      getByTestId(testId: string): Chainable<JQuery<HTMLElement>>;
    }
  }
}

// Prevent uncaught exception failures
Cypress.on('uncaught:exception', () => {
  return false;
});
