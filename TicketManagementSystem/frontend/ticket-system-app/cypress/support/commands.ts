// ***********************************************
// Custom Cypress Commands for Ticket Management System
// https://on.cypress.io/custom-commands
// ***********************************************

/// <reference types="cypress" />

// Extend Cypress Chainable interface for custom commands
declare global {
  namespace Cypress {
    interface Chainable {
      login(username: string, password: string): Chainable<void>
      createTicket(title: string, description: string, priority: string): Chainable<void>
      addComment(comment: string): Chainable<void>
      updateTicketStatus(status: string): Chainable<void>
      assignTicketToMe(): Chainable<void>
      navigateToTicket(title: string): Chainable<void>
    }
  }
}

// Custom command to login
Cypress.Commands.add('login', (username: string, password: string) => {
  cy.session([username, password], () => {
    cy.visit('/auth/login')
    cy.get('[data-cy="email-input"]').type(username)
    cy.get('[data-cy="password-input"]').type(password)
    cy.get('[data-cy="login-button"]').click()
    cy.url().should('not.include', '/auth/login')
  })
})

// Custom command to create a ticket
Cypress.Commands.add('createTicket', (title: string, description: string, priority: string) => {
  cy.visit('/tickets/new')
  cy.get('[data-cy="title-input"]').type(title)
  cy.get('[data-cy="description-input"]').type(description)
  cy.get('[data-cy="priority-select"]').select(priority)
  cy.get('[data-cy="submit-button"]').click()
  cy.url().should('include', '/tickets/')
})

// Custom command to add comment to current ticket
Cypress.Commands.add('addComment', (comment: string) => {
  cy.get('[data-cy="comment-input"]').type(comment)
  cy.get('[data-cy="add-comment-button"]').click()
  cy.get('[data-cy="comments-list"]').should('contain', comment)
})

// Custom command to update ticket status
Cypress.Commands.add('updateTicketStatus', (status: string) => {
  cy.get('[data-cy="status-select"]').select(status)
  cy.get('[data-cy="update-status-button"]').click()
  cy.get('[data-cy="ticket-status"]').should('contain', status)
})

// Custom command to assign ticket to current user
Cypress.Commands.add('assignTicketToMe', () => {
  cy.get('[data-cy="assign-to-me-button"]').click()
  cy.get('[data-cy="ticket-assigned-to"]').should('be.visible')
})

// Custom command to navigate to ticket details by title
Cypress.Commands.add('navigateToTicket', (title: string) => {
  cy.visit('/tickets')
  cy.get('[data-cy="tickets-list"]').contains(title).click()
  cy.url().should('include', '/tickets/')
})

// Add more custom commands as needed
