using System.Globalization;
using System.Linq.Expressions;
using DDDTemplate.Domain.Helpers;
using DDDTemplate.Domain.Interfaces.Entities;
using Microsoft.Extensions.Primitives;

namespace DDDTemplate.API.Helpers;

public class Filter<TEntity, TId> where TEntity : IEntity<TId>
{
  public int? Skip { get; set; }
  public int? Take { get; set; }
  public Expression<Func<TEntity, bool>>? Params { get; set; }
  public Expression<Func<TEntity, object>>? OrderByField { get; set; }
  public string OrderByDirection = "DESC";
}

public static class FilterBuilder
{

  private static Dictionary<string, StringValues> ExtractParams(IQueryCollection query)
  {
    Dictionary<string, StringValues> queryParams = query
      .Where(x => x.Key != "notify")
      .ToDictionary(
      q => q.Key,
      q => q.Value
    );

    return queryParams;
  }

  private static Expression ResetDates(Expression x)
  {
    return Expression.New(
      typeof(DateTime).GetConstructor([typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int)])!,
      Expression.Property(Expression.Convert(x, typeof(DateTime)), "Year"),
      Expression.Property(Expression.Convert(x, typeof(DateTime)), "Month"),
      Expression.Property(Expression.Convert(x, typeof(DateTime)), "Day"),
      Expression.Constant(0),
      Expression.Constant(0),
      Expression.Constant(0)
    );
  }

  private static Expression GetComparisonExpression(
    string matchMode,
    MemberExpression property,
    MemberExpression valueExpression,
    Expression expression
  )
  {
    MethodCallExpression? propertyToLower = null!;
    MethodCallExpression? expressionToLower = null!;

    if (property.Type == typeof(string))
    {
      var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
      propertyToLower = Expression.Call(property, toLowerMethod!);
      expressionToLower = Expression.Call(expression, toLowerMethod!);
    }

    return matchMode switch
    {
      "startsWith" => Expression.Call(propertyToLower is not null ? propertyToLower : property, typeof(string).GetMethod("StartsWith", [typeof(string)])!, expressionToLower is not null ? expressionToLower : expression),
      "contains" => Expression.Call(propertyToLower is not null ? propertyToLower : property, typeof(string).GetMethod("Contains", [typeof(string)])!, expressionToLower is not null ? expressionToLower : expression),
      "notContains" => Expression.Not(Expression.Call(propertyToLower is not null ? propertyToLower : property, typeof(string).GetMethod("Contains", [typeof(string)])!, expressionToLower is not null ? expressionToLower : expression)),
      "endsWith" => Expression.Call(propertyToLower is not null ? propertyToLower : property, typeof(string).GetMethod("EndsWith", [typeof(string)])!, expressionToLower is not null ? expressionToLower : expression),
      "equals" => Expression.Equal(property, expression),
      "notEquals" => Expression.NotEqual(property, expression),
      "lt" => Expression.LessThan(valueExpression, expression),
      "lte" => Expression.LessThanOrEqual(valueExpression, expression),
      "gt" => Expression.GreaterThan(valueExpression, expression),
      "gte" => Expression.GreaterThanOrEqual(valueExpression, expression),
      "is" => Expression.Equal(propertyToLower is not null ? propertyToLower : property, expressionToLower is not null ? expressionToLower : expression),
      "isNot" => Expression.NotEqual(propertyToLower is not null ? propertyToLower : property, expressionToLower is not null ? expressionToLower : expression),
      "before" => Expression.LessThan(property, expression),
      "after" => Expression.GreaterThan(property, expression),
      "dateIs" => Expression.Equal(ResetDates(property), ResetDates(expression)),
      "dateIsNot" => Expression.NotEqual(ResetDates(property), ResetDates(expression)),
      "dateBefore" => Expression.LessThan(ResetDates(property), ResetDates(expression)),
      "dateAfter" => Expression.GreaterThan(ResetDates(property), ResetDates(expression)),
      _ => throw new CustomExceptions.InvalidFilterOperatorException(matchMode)
    };
  }

  public static Filter<TEntity, TId>? Build<TEntity, TId>(IQueryCollection query) where TEntity : IEntity<TId>
  {
    Dictionary<string, StringValues> queryParams = ExtractParams(query);

    if (queryParams.Count == 0)
      return null!;

    var filter = new Filter<TEntity, TId> { };

    ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
    Expression? combinedExpression = null;

    foreach (KeyValuePair<string, StringValues> param in queryParams)
    {
      if (param.Key.ToLower() == "skip")
      {
        filter.Skip = Convert.ToInt32(param.Value);
        continue;
      }

      if (param.Key.ToLower() == "take")
      {
        filter.Take = Convert.ToInt32(param.Value);
        continue;
      }

      if (param.Key.ToLower() == "notify")
      {
        continue;
      }

      if (param.Key.ToLower() == "order_by_field")
      {
        string fieldName = param.Value.ToString() ?? "Id";
        var _parameter = Expression.Parameter(typeof(TEntity), "x");
        var _property = Expression.Property(parameter, fieldName);
        var conversion = Expression.Convert(_property, typeof(object));

        filter.OrderByField = Expression.Lambda<Func<TEntity, object>>(conversion, parameter);

        continue;
      }

      if (param.Key.ToLower() == "order_by_direction")
      {
        filter.OrderByDirection = param.Value.ToString().ToUpper();
        continue;
      }

      string[]? parts = param.Key.Split('_');
      string? propertyName = parts[0];
      string? matchMode = parts.Length > 1 ? parts[1] : "equals";

      var property = Expression.Property(parameter, propertyName);
      var targetType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;

      Expression? comparison = null;
      object? value = Convert.ChangeType(param.Value.ToString(), targetType, CultureInfo.InvariantCulture);

      Expression? expression = Expression.Constant(value);

      if (Nullable.GetUnderlyingType(property.Type) != null)
      {
        expression = Expression.Convert(expression, property.Type);
      }

      if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        var hasValueCheck = Expression.Property(property, "HasValue");
        var valueExpression = Expression.Property(property, "Value");

        comparison = Expression.AndAlso(
            hasValueCheck,
            GetComparisonExpression(matchMode, property, valueExpression, expression)
        );
      }
      else
        comparison = GetComparisonExpression(matchMode, property, property, expression);

      if (comparison != null)
        combinedExpression = combinedExpression == null ? comparison : Expression.AndAlso(combinedExpression, comparison);
    }

    filter.Params = combinedExpression == null ? x => true : Expression.Lambda<Func<TEntity, bool>>(combinedExpression, parameter);

    return filter;
  }
}
