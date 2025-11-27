describe('Complete Ticket Lifecycle', () => {
  const testUser = {
    username: 'testuser',
    password: 'password123',
    email: 'testuser@example.com'
  };

  const testAgent = {
    username: 'testagent',
    password: 'agent123',
    email: 'agent@example.com'
  };

  beforeEach(() => {
    // Intercept API calls for consistent testing
    cy.intercept('GET', '/api/tickets*', { fixture: 'tickets.json' }).as('getTickets');
    cy.intercept('POST', '/api/tickets', { statusCode: 201, body: { id: 1 } }).as('createTicket');
    cy.intercept('PUT', '/api/tickets/*', { statusCode: 200 }).as('updateTicket');
    cy.intercept('POST', '/api/tickets/*/comments', { statusCode: 201 }).as('addComment');
  });

  it('should complete full ticket lifecycle from creation to resolution', () => {
    // Step 1: Login as regular user
    cy.session([testUser.username, testUser.password], () => {
      cy.visit('/auth/login');
      cy.get('[data-cy="email-input"]').type(testUser.email);
      cy.get('[data-cy="password-input"]').type(testUser.password);
      cy.get('[data-cy="login-button"]').click();
      cy.url().should('not.include', '/auth/login');
    });

    // Step 2: Navigate to create ticket page
    cy.visit('/dashboard');
    cy.get('[data-cy="create-ticket-button"]').click();
    cy.url().should('include', '/tickets/new');

    // Step 3: Fill and submit ticket form
    cy.get('[data-cy="title-input"]').type('Software Bug Report');
    cy.get('[data-cy="description-input"]').type('Application crashes when clicking save button in the user profile section');
    cy.get('[data-cy="priority-select"]').select('High');
    cy.get('[data-cy="submit-button"]').click();

    // Step 4: Verify ticket creation success
    cy.url().should('include', '/tickets/');
    cy.get('[data-cy="ticket-title"]').should('contain', 'Software Bug Report');
    cy.get('[data-cy="ticket-description"]').should('contain', 'Application crashes');
    cy.get('[data-cy="ticket-status"]').should('contain', 'Open');
    cy.get('[data-cy="ticket-priority"]').should('contain', 'High');

    // Step 5: Add comment to ticket
    cy.get('[data-cy="comment-input"]').type('I also noticed this happens on Chrome browser version 120+');
    cy.get('[data-cy="add-comment-button"]').click();
    cy.get('[data-cy="comments-list"]').should('contain', 'Chrome browser');

    // Step 6: Logout and login as agent
    cy.get('[data-cy="logout-button"]').click();
    cy.session([testAgent.username, testAgent.password], () => {
      cy.visit('/auth/login');
      cy.get('[data-cy="email-input"]').type(testAgent.email);
      cy.get('[data-cy="password-input"]').type(testAgent.password);
      cy.get('[data-cy="login-button"]').click();
    });

    // Step 7: Agent views and assigns ticket
    cy.visit('/tickets');
    cy.get('[data-cy="tickets-list"]').contains('Software Bug Report').click();
    cy.get('[data-cy="assign-to-me-button"]').click();
    cy.get('[data-cy="ticket-assigned-to"]').should('contain', testAgent.username);

    // Step 8: Agent updates status to In Progress
    cy.get('[data-cy="status-select"]').select('In Progress');
    cy.get('[data-cy="update-status-button"]').click();
    cy.get('[data-cy="ticket-status"]').should('contain', 'In Progress');

    // Step 9: Agent adds investigation comment
    cy.get('[data-cy="comment-input"]').type('Investigating the issue. Found that it occurs due to null reference in UserProfileComponent.save() method.');
    cy.get('[data-cy="add-comment-button"]').click();

    // Step 10: Agent marks as Resolved
    cy.get('[data-cy="status-select"]').select('Resolved');
    cy.get('[data-cy="update-status-button"]').click();
    cy.get('[data-cy="ticket-status"]').should('contain', 'Resolved');

    // Step 11: User logs back in and verifies resolution
    cy.get('[data-cy="logout-button"]').click();
    cy.session([testUser.username, testUser.password], () => {
      cy.visit('/auth/login');
      cy.get('[data-cy="email-input"]').type(testUser.email);
      cy.get('[data-cy="password-input"]').type(testUser.password);
      cy.get('[data-cy="login-button"]').click();
    });

    cy.visit('/tickets');
    cy.get('[data-cy="tickets-list"]').contains('Software Bug Report').click();
    cy.get('[data-cy="ticket-status"]').should('contain', 'Resolved');

    // Step 12: User closes the ticket
    cy.get('[data-cy="status-select"]').select('Closed');
    cy.get('[data-cy="update-status-button"]').click();
    cy.get('[data-cy="ticket-status"]').should('contain', 'Closed');
  });

  it('should handle ticket creation validation errors', () => {
    cy.session([testUser.username, testUser.password], () => {
      cy.visit('/auth/login');
      cy.get('[data-cy="email-input"]').type(testUser.email);
      cy.get('[data-cy="password-input"]').type(testUser.password);
      cy.get('[data-cy="login-button"]').click();
    });

    cy.visit('/tickets/new');

    // Try to submit empty form
    cy.get('[data-cy="submit-button"]').click();
    cy.get('[data-cy="title-error"]').should('be.visible').and('contain', 'Title is required');
    cy.get('[data-cy="description-error"]').should('be.visible').and('contain', 'Description is required');

    // Try with title too long
    cy.get('[data-cy="title-input"]').type('a'.repeat(101));
    cy.get('[data-cy="description-input"]').type('Valid description');
    cy.get('[data-cy="submit-button"]').click();
    cy.get('[data-cy="title-error"]').should('be.visible').and('contain', 'Title must be less than 100 characters');

    // Test successful submission after fixing errors
    cy.get('[data-cy="title-input"]').clear().type('Valid Title');
    cy.get('[data-cy="submit-button"]').click();
    cy.url().should('include', '/tickets/');
  });

  it('should handle network errors during ticket operations', () => {
    cy.session([testUser.username, testUser.password], () => {
      cy.visit('/auth/login');
      cy.get('[data-cy="email-input"]').type(testUser.email);
      cy.get('[data-cy="password-input"]').type(testUser.password);
      cy.get('[data-cy="login-button"]').click();
    });

    // Mock network failure during ticket creation
    cy.intercept('POST', '/api/tickets', { statusCode: 500, body: { message: 'Internal server error' } }).as('createTicketError');

    cy.visit('/tickets/new');
    cy.get('[data-cy="title-input"]').type('Network Error Test');
    cy.get('[data-cy="description-input"]').type('Testing network error handling');
    cy.get('[data-cy="priority-select"]').select('Medium');
    cy.get('[data-cy="submit-button"]').click();

    cy.get('[data-cy="error-message"]').should('be.visible').and('contain', 'Failed to create ticket');
    cy.url().should('include', '/tickets/new'); // Should stay on create page
  });

  it('should handle concurrent ticket updates', () => {
    // This test simulates what happens when multiple users try to update the same ticket
    cy.session([testUser.username, testUser.password], () => {
      cy.visit('/auth/login');
      cy.get('[data-cy="email-input"]').type(testUser.email);
      cy.get('[data-cy="password-input"]').type(testUser.password);
      cy.get('[data-cy="login-button"]').click();
    });

    cy.visit('/tickets');
    cy.get('[data-cy="tickets-list"]').first().click();

    // Mock concurrent modification error
    cy.intercept('PUT', '/api/tickets/*', { statusCode: 409, body: { message: 'Ticket was modified by another user' } }).as('concurrentUpdate');

    cy.get('[data-cy="status-select"]').select('In Progress');
    cy.get('[data-cy="update-status-button"]').click();

    cy.get('[data-cy="error-message"]').should('be.visible').and('contain', 'modified by another user');
    cy.get('[data-cy="refresh-button"]').should('be.visible');
  });

  it('should maintain ticket history through lifecycle', () => {
    cy.session([testAgent.username, testAgent.password], () => {
      cy.visit('/auth/login');
      cy.get('[data-cy="email-input"]').type(testAgent.email);
      cy.get('[data-cy="password-input"]').type(testAgent.password);
      cy.get('[data-cy="login-button"]').click();
    });

    cy.visit('/tickets');
    cy.get('[data-cy="tickets-list"]').first().click();

    // Check that history section exists and shows initial creation
    cy.get('[data-cy="ticket-history"]').should('be.visible');
    cy.get('[data-cy="history-entries"]').should('have.length.greaterThan', 0);
    cy.get('[data-cy="history-entries"]').first().should('contain', 'created');

    // Make a change and verify history updates
    cy.get('[data-cy="status-select"]').select('In Progress');
    cy.get('[data-cy="update-status-button"]').click();

    cy.get('[data-cy="history-entries"]').should('contain', 'Status changed from Open to In Progress');
  });
});
