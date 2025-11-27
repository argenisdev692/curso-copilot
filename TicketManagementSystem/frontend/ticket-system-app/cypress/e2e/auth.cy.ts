describe('User Authentication', () => {
  beforeEach(() => {
    cy.visit('/')
  })

  it('should display login page by default', () => {
    cy.url().should('include', '/auth/login')
    cy.get('[data-cy="login-form"]').should('be.visible')
  })

  it('should login successfully with valid credentials', () => {
    cy.get('[data-cy="email-input"]').type('testuser@example.com')
    cy.get('[data-cy="password-input"]').type('password123')
    cy.get('[data-cy="login-button"]').click()
    cy.url().should('not.include', '/auth/login')
    cy.get('[data-cy="dashboard"]').should('be.visible')
  })

  it('should display error message with invalid credentials', () => {
    cy.get('[data-cy="email-input"]').type('invaliduser@example.com')
    cy.get('[data-cy="password-input"]').type('wrongpassword')
    cy.get('[data-cy="login-button"]').click()
    cy.get('[data-cy="error-message"]').should('be.visible')
    cy.get('[data-cy="error-message"]').should('contain', 'Invalid credentials')
  })

  it('should show validation errors for empty fields', () => {
    cy.get('[data-cy="login-button"]').click()
    cy.get('[data-cy="email-error"]').should('be.visible')
    cy.get('[data-cy="password-error"]').should('be.visible')
  })

  it('should navigate to register page', () => {
    cy.get('[data-cy="register-link"]').click()
    cy.url().should('include', '/auth/register')
  })

  it('should register new user successfully', () => {
    cy.visit('/auth/register')
    cy.get('[data-cy="name-input"]').type('newuser')
    cy.get('[data-cy="email-input"]').type('newuser@example.com')
    cy.get('[data-cy="password-input"]').type('password123')
    cy.get('[data-cy="confirm-password-input"]').type('password123')
    cy.get('[data-cy="register-button"]').click()
    cy.url().should('include', '/auth/login')
    cy.get('[data-cy="success-message"]').should('contain', 'Registration successful')
  })

  it('should handle password mismatch during registration', () => {
    cy.visit('/auth/register')
    cy.get('[data-cy="name-input"]').type('newuser')
    cy.get('[data-cy="email-input"]').type('newuser@example.com')
    cy.get('[data-cy="password-input"]').type('password123')
    cy.get('[data-cy="confirm-password-input"]').type('differentpassword')
    cy.get('[data-cy="register-button"]').click()
    cy.get('[data-cy="password-error"]').should('contain', 'Passwords do not match')
  })

  it('should logout successfully', () => {
    // @ts-ignore
    cy.login('testuser@example.com', 'password123')
    cy.get('[data-cy="logout-button"]').click()
    cy.url().should('include', '/auth/login')
  })
})
