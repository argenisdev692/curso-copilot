/// <reference types="cypress" />

/**
 * Custom command to login via API
 */
Cypress.Commands.add('login', (email: string, password: string) => {
  cy.request({
    method: 'POST',
    url: `${Cypress.env('apiUrl')}/auth/login`,
    body: { email, password },
  }).then((response) => {
    window.localStorage.setItem('accessToken', response.body.data.accessToken);
    window.localStorage.setItem('refreshToken', response.body.data.refreshToken);
  });
});

/**
 * Custom command to get element by data-testid
 */
Cypress.Commands.add('getByTestId', (testId: string) => {
  return cy.get(`[data-testid="${testId}"]`);
});
