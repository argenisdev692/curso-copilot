using System.Linq.Expressions;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Specifications;

/// <summary>
/// Base specification for queries
/// </summary>
public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }
}

/// <summary>
/// Specification for users
/// </summary>
public class UserByRoleSpecification : Specification<User>
{
    private readonly string _role;

    public UserByRoleSpecification(string role)
    {
        _role = role;
    }

    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.Role == _role;
    }
}

/// <summary>
/// Specification for active users
/// </summary>
public class ActiveUserSpecification : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
    {
        return user => user.IsActive && !user.IsDeleted;
    }
}