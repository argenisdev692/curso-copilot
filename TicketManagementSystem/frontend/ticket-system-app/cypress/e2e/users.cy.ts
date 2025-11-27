describe('User Management', () => {
  beforeEach(() => {
    // @ts-ignore
    cy.login('admin@example.com', 'admin123') // Assuming admin user
  })

  it('should display users list', () => {
    cy.visit('/users')
    cy.get('[data-cy="users-list"]').should('be.visible')
    cy.get('[data-cy="users-list"]').children().should('have.length.greaterThan', 0)
  })

  it('should create new user', () => {
    cy.visit('/users/new')
    cy.get('[data-cy="name-input"]').type('newtestuser')
    cy.get('[data-cy="email-input"]').type('newtestuser@example.com')
    cy.get('[data-cy="role-select"]').select('User')
    cy.get('[data-cy="create-user-button"]').click()
    cy.url().should('include', '/users')
    cy.get('[data-cy="success-message"]').should('contain', 'User created successfully')
  })

  it('should edit existing user', () => {
    cy.visit('/users')
    cy.get('[data-cy="edit-user-button"]').first().click()
    cy.get('[data-cy="email-input"]').clear().type('updated@example.com')
    cy.get('[data-cy="update-user-button"]').click()
    cy.get('[data-cy="success-message"]').should('contain', 'User updated successfully')
  })

  it('should delete user with confirmation', () => {
    cy.visit('/users')
    cy.get('[data-cy="delete-user-button"]').first().click()
    cy.get('[data-cy="confirm-delete"]').should('be.visible')
    cy.get('[data-cy="confirm-delete-button"]').click()
    cy.get('[data-cy="success-message"]').should('contain', 'User deleted successfully')
  })

  it('should handle validation for duplicate username', () => {
    cy.visit('/users/new')
    cy.get('[data-cy="name-input"]').type('existinguser')
    cy.get('[data-cy="email-input"]').type('test@example.com')
    cy.get('[data-cy="create-user-button"]').click()
    cy.get('[data-cy="error-message"]').should('contain', 'Username already exists')
  })

  it('should filter users by role', () => {
    cy.visit('/users')
    cy.get('[data-cy="role-filter"]').select('Admin')
    cy.get('[data-cy="users-list"]').children().each(($user) => {
      cy.wrap($user).find('[data-cy="user-role"]').should('contain', 'Admin')
    })
  })

  it('should search users by name', () => {
    cy.visit('/users')
    cy.get('[data-cy="search-input"]').type('testuser')
    cy.get('[data-cy="users-list"]').should('have.length', 1)
    cy.get('[data-cy="user-name"]').should('contain', 'testuser')
  })
})
