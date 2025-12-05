using System.Linq.Expressions;
using System.Reflection;
using Shadowchats.Conversations.Domain.Entities;
using Shadowchats.Conversations.Domain.Exceptions;

namespace Shadowchats.Conversations.Application.Exceptions;

public sealed class EntityAlreadyExistsException<TEntity, TProperty> : Exception where TEntity : BaseEntity
{
    public EntityAlreadyExistsException(Expression<Func<TEntity, TProperty>> propertyExpression) =>
        _propertyMember = GetMemberInfo(propertyExpression);

    public bool IsConflictOn(Expression<Func<TEntity, TProperty>> propertyExpression) =>
        GetMemberInfo(propertyExpression) == _propertyMember;

    private static MemberInfo GetMemberInfo(Expression<Func<TEntity, TProperty>> expression)
    {
        return expression.Body switch
        {
            MemberExpression memberExpr => memberExpr.Member,
            UnaryExpression { Operand: MemberExpression unaryMember } => unaryMember.Member,
            _ => throw new BugException()
        };
    }

    private readonly MemberInfo _propertyMember;
}