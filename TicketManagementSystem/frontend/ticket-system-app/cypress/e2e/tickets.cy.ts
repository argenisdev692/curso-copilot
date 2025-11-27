describe('Ticket Management', () => {
  beforeEach(() => {
    // @ts-ignore
    cy.login('testuser@example.com', 'password123')
  })

  it('should display tickets list on dashboard', () => {
    cy.visit('/dashboard')
    cy.get('[data-cy="tickets-list"]').should('be.visible')
  })

  it('should create new ticket successfully', () => {
    // @ts-ignore
    cy.createTicket('Test Ticket', 'This is a test ticket description', 'High')
    cy.get('[data-cy="ticket-title"]').should('contain', 'Test Ticket')
    cy.get('[data-cy="ticket-description"]').should('contain', 'This is a test ticket description')
    cy.get('[data-cy="ticket-priority"]').should('contain', 'High')
  })

  it('should display validation errors for empty title', () => {
    cy.visit('/tickets/new')
    cy.get('[data-cy="description-input"]').type('Description without title')
    cy.get('[data-cy="submit-button"]').click()
    cy.get('[data-cy="title-error"]').should('be.visible')
    cy.get('[data-cy="title-error"]').should('contain', 'Title is required')
  })

  it('should update ticket status', () => {
    // @ts-ignore
    cy.createTicket('Status Update Test', 'Testing status change', 'Medium')
    cy.get('[data-cy="status-select"]').select('In Progress')
    cy.get('[data-cy="update-button"]').click()
    cy.get('[data-cy="ticket-status"]').should('contain', 'In Progress')
  })

  it('should filter tickets by status', () => {
    cy.visit('/tickets')
    cy.get('[data-cy="status-filter"]').select('Open')
    cy.get('[data-cy="tickets-list"]').children().each(($ticket) => {
      cy.wrap($ticket).find('[data-cy="ticket-status"]').should('contain', 'Open')
    })
  })

  it('should search tickets by title', () => {
    // @ts-ignore
    cy.createTicket('Unique Search Test', 'Searchable ticket', 'Low')
    cy.get('[data-cy="search-input"]').type('Unique Search Test')
    cy.get('[data-cy="tickets-list"]').should('have.length', 1)
    cy.get('[data-cy="ticket-title"]').should('contain', 'Unique Search Test')
  })

  it('should handle API error when creating ticket', () => {
    cy.intercept('POST', '/api/tickets', { statusCode: 500 }).as('createTicketError')
    cy.visit('/tickets/new')
    cy.get('[data-cy="title-input"]').type('Error Test')
    cy.get('[data-cy="description-input"]').type('Testing error handling')
    cy.get('[data-cy="submit-button"]').click()
    cy.wait('@createTicketError')
    cy.get('[data-cy="error-message"]').should('be.visible')
    cy.get('[data-cy="error-message"]').should('contain', 'Failed to create ticket')
  })

  it('should navigate back from ticket detail', () => {
    // @ts-ignore
    cy.createTicket('Navigation Test', 'Testing navigation', 'Medium')
    cy.get('[data-cy="ticket-link"]').first().click()
    cy.url().should('include', '/tickets/')
    cy.get('[data-cy="back-button"]').click()
    cy.url().should('include', '/tickets')
  })
})
