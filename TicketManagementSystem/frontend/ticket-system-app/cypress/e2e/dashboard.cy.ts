describe('Dashboard', () => {
  beforeEach(() => {
    // @ts-ignore
    cy.login('testuser@example.com', 'password123')
    cy.visit('/dashboard')
  })

  it('should display dashboard with key metrics', () => {
    cy.get('[data-cy="dashboard"]').should('be.visible')
    cy.get('[data-cy="total-tickets"]').should('be.visible')
    cy.get('[data-cy="open-tickets"]').should('be.visible')
    cy.get('[data-cy="closed-tickets"]').should('be.visible')
  })

  it('should show recent tickets', () => {
    cy.get('[data-cy="recent-tickets"]').should('be.visible')
    cy.get('[data-cy="recent-tickets"]').children().should('have.length.greaterThan', 0)
  })

  it('should navigate to tickets page from dashboard', () => {
    cy.get('[data-cy="view-all-tickets"]').click()
    cy.url().should('include', '/tickets')
  })

  it('should display user profile information', () => {
    cy.get('[data-cy="user-profile"]').should('be.visible')
    cy.get('[data-cy="user-name"]').should('contain', 'testuser')
  })

  it('should handle loading state', () => {
    cy.intercept('GET', '/api/dashboard', { delay: 2000 }).as('slowDashboard')
    cy.visit('/dashboard')
    cy.get('[data-cy="loading-spinner"]').should('be.visible')
    cy.wait('@slowDashboard')
    cy.get('[data-cy="loading-spinner"]').should('not.exist')
  })

  it('should refresh dashboard data', () => {
    cy.get('[data-cy="refresh-button"]').click()
    cy.get('[data-cy="loading-spinner"]').should('be.visible')
    cy.get('[data-cy="loading-spinner"]').should('not.exist')
  })
})
