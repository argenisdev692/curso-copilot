using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;
using TicketManagementSystem.API.Services;
using Xunit;

namespace TicketManagementSystem.API.Tests.Unit;

/// <summary>
/// Unit tests for UserService
/// </summary>
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UserService>>();

        _userService = new UserService(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    #region GetUsersAsync

    [Fact]
    public async Task GetUsersAsync_ValidParameters_ReturnsPagedResponse()
    {
        // Arrange
        var parameters = new GetUsersQueryParameters
        {
            Page = 1,
            PageSize = 10
        };

        var pagedUsers = new PagedResponse<User>
        {
            Items = new List<User>
            {
                new User { Id = 1, Email = "user1@test.com", FullName = "User One" }
            },
            TotalItems = 1,
            Page = 1,
            PageSize = 10,
            TotalPages = 1
        };

        var userDtos = new List<UserDto>
        {
            new UserDto { Id = 1, Email = "user1@test.com", FullName = "User One" }
        };

        _userRepositoryMock
            .Setup(x => x.GetUsersAsync(parameters))
            .ReturnsAsync(pagedUsers);

        _mapperMock
            .Setup(x => x.Map<List<UserDto>>(It.IsAny<List<User>>()))
            .Returns(userDtos);

        // Act
        var result = await _userService.GetUsersAsync(parameters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.TotalItems.Should().Be(1);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);

        _userRepositoryMock.Verify(x => x.GetUsersAsync(parameters), Times.Once);
        _mapperMock.Verify(x => x.Map<List<UserDto>>(It.IsAny<List<User>>()), Times.Once);
    }

    #endregion

    #region GetUserByIdAsync

    [Fact]
    public async Task GetUserByIdAsync_ExistingUser_ReturnsUserDto()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            FullName = "Test User",
            Role = Role.User
        };

        var userDto = new UserDto
        {
            Id = userId,
            Email = "test@example.com",
            FullName = "Test User",
            Role = "User"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        _mapperMock
            .Setup(x => x.Map<UserDto>(user))
            .Returns(userDto);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(userDto);

        _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDto>(user), Times.Once);
    }

    [Fact]
    public async Task GetUserByIdAsync_NonExistentUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = 999;
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var act = () => _userService.GetUserByIdAsync(userId);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {userId} not found");

        _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDto>(It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region CreateUserAsync

    [Fact]
    public async Task CreateUserAsync_ValidData_ReturnsCreatedUserDto()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Email = "newuser@example.com",
            FullName = "New User",
            Password = "SecurePassword123!",
            Role = "User"
        };

        var user = new User
        {
            Id = 0, // Will be set by database
            Email = userDto.Email,
            FullName = userDto.FullName,
            Role = Role.User
        };

        var createdUser = new User
        {
            Id = 1,
            Email = userDto.Email,
            FullName = userDto.FullName,
            Role = Role.User,
            PasswordHash = "hashed_password",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var resultUserDto = new UserDto
        {
            Id = 1,
            Email = userDto.Email,
            FullName = userDto.FullName,
            Role = "User"
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(userDto.Email))
            .ReturnsAsync((User?)null);

        _mapperMock
            .Setup(x => x.Map<User>(userDto))
            .Returns(user);

        _passwordHasherMock
            .Setup(x => x.HashPassword(userDto.Password))
            .Returns("hashed_password");

        _userRepositoryMock
            .Setup(x => x.AddAsync(user))
            .Callback<User>(u =>
            {
                u.Id = 1;
                u.CreatedAt = DateTime.UtcNow;
                u.UpdatedAt = DateTime.UtcNow;
            })
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(x => x.Map<UserDto>(It.Is<User>(u => u.Id == 1)))
            .Returns(resultUserDto);

        // Act
        var result = await _userService.CreateUserAsync(userDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(resultUserDto);

        _userRepositoryMock.Verify(x => x.GetByEmailAsync(userDto.Email), Times.Once);
        _mapperMock.Verify(x => x.Map<User>(userDto), Times.Once);
        _passwordHasherMock.Verify(x => x.HashPassword(userDto.Password), Times.Once);
        _userRepositoryMock.Verify(x => x.AddAsync(It.Is<User>(u =>
            u.Email == userDto.Email &&
            u.PasswordHash == "hashed_password")), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_ExistingEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Email = "existing@example.com",
            FullName = "New User",
            Password = "Password123!",
            Role = "User"
        };

        var existingUser = new User
        {
            Id = 1,
            Email = userDto.Email
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(userDto.Email))
            .ReturnsAsync(existingUser);

        // Act
        var act = () => _userService.CreateUserAsync(userDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User with this email already exists");

        _userRepositoryMock.Verify(x => x.GetByEmailAsync(userDto.Email), Times.Once);
        _mapperMock.Verify(x => x.Map<User>(It.IsAny<CreateUserDto>()), Times.Never);
        _passwordHasherMock.Verify(x => x.HashPassword(It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region UpdateUserAsync

    [Fact]
    public async Task UpdateUserAsync_ValidData_ReturnsUpdatedUserDto()
    {
        // Arrange
        var userId = 1;
        var updateDto = new UpdateUserDto
        {
            Email = "updated@example.com",
            FullName = "Updated Name",
            Role = "Admin",
            IsActive = true
        };

        var existingUser = new User
        {
            Id = userId,
            Email = "original@example.com",
            FullName = "Original Name",
            Role = Role.User,
            IsActive = true
        };

        var updatedUserDto = new UserDto
        {
            Id = userId,
            Email = updateDto.Email,
            FullName = updateDto.FullName,
            Role = "Admin",
            IsActive = true
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(existingUser);

        _userRepositoryMock
            .Setup(x => x.UpdateAsync(It.Is<User>(u =>
                u.Id == userId &&
                u.Email == updateDto.Email &&
                u.FullName == updateDto.FullName &&
                u.Role == Role.Admin)))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(x => x.Map<UserDto>(It.Is<User>(u => u.Id == userId)))
            .Returns(updatedUserDto);

        // Act
        var result = await _userService.UpdateUserAsync(userId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(updatedUserDto);

        _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(x => x.Map(updateDto, existingUser), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.Is<User>(u =>
            u.Id == userId &&
            u.UpdatedAt != default)), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_NonExistentUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = 999;
        var updateDto = new UpdateUserDto
        {
            Email = "test@example.com",
            FullName = "Test User",
            Role = "User",
            IsActive = true
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var act = () => _userService.UpdateUserAsync(userId, updateDto);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {userId} not found");

        _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(x => x.Map(It.IsAny<UpdateUserDto>(), It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region DeleteUserAsync

    [Fact]
    public async Task DeleteUserAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            IsDeleted = false
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.DeleteUserAsync(userId);

        // Assert
        result.Should().BeTrue();

        _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.Is<User>(u =>
            u.Id == userId &&
            u.IsDeleted == true &&
            u.UpdatedAt != default)), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_NonExistentUser_ReturnsFalse()
    {
        // Arrange
        var userId = 999;
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.DeleteUserAsync(userId);

        // Assert
        result.Should().BeFalse();

        _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    #endregion
}