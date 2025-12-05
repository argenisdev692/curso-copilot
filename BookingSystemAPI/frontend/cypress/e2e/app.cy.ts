describe('Application', () => {
  beforeEach(() => {
    cy.visit('/');
  });

  it('should load the application', () => {
    cy.get('app-root').should('exist');
  });

  it('should display the main layout', () => {
    cy.getByTestId('main-content').should('be.visible');
  });
});
