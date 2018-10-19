using System;
using System.Linq;
using Surgicalogic.Model.EntityModel.Base;
using System.Linq.Expressions;
using System.Collections.Generic;
using Surgicalogic.Common.CustomAttributes;
using System.Reflection;
using Surgicalogic.Services.Common;

namespace Surgicalogic.Services.Query
{
    public static class QueryFilterService<TModel> where TModel : EntityModel
    {
        public static Expression<Func<TModel, bool>> GetSearchQuery(List<string> propertyNames, string searchText)
        {
            Expression<Func<TModel, bool>> result = x => false;
            ParameterExpression parameter = Expression.Parameter(typeof(TModel), "model");

            foreach (var propertyName in propertyNames)
            {
                Expression property = null;

                if (propertyName.Contains("."))
                {
                    var childModel = typeof(TModel).GetProperty(propertyName.Split('.')[0]);
                    var childObject = childModel.PropertyType.GetProperty(propertyName.Split('.')[1]);
                    var inner = Expression.Property(parameter, childModel);
                    property = Expression.Property(inner, childObject);
                }
                else
                {
                    property = Expression.Property(parameter, propertyName);
                }

                Expression target = Expression.Constant(searchText.Replace("İ","i").Replace("I","ı"));
                var lambda = GetContainsLambdaExpression(property, target, parameter, propertyName);
                result = CombineExpressionsByOr(result, lambda);
            }

            return result;
        }

        private static Expression<Func<TModel, bool>> GetContainsLambdaExpression(Expression property, Expression target, ParameterExpression parameter, string propertyName)
        {
            Expression containsMethod = Expression.Call(property, "IndexOf", null, target, Expression.Constant(StringComparison.CurrentCultureIgnoreCase, typeof(StringComparison)));
            var like = Expression.GreaterThanOrEqual(containsMethod, Expression.Constant(0));
            var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(object)));
            like = Expression.AndAlso(nullCheck, like);
            return Expression.Lambda<Func<TModel, bool>>(like, parameter);
        }

        private static Expression<Func<TModel, bool>> CombineExpressionsByOr(Expression<Func<TModel, bool>> expr1, Expression<Func<TModel, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(TModel));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<TModel, bool>>(Expression.Or(left, right), parameter);
        }

        internal static List<string> GetSearchableProperties()
        {
            var result = new List<string>();
            var properties = typeof(TModel).GetProperties();
            foreach (var item in properties)
            {
                //if it is searchable
                if (item.GetCustomAttributes(typeof(SearchableAttribute), false).Any())
                {
                    //if it is a child model.
                    if (item.PropertyType.BaseType == typeof(EntityModel))
                    {
                        var subProperties = item.PropertyType.GetProperties();
                        foreach (var subItem in subProperties)
                        {
                            if (subItem.GetCustomAttributes(typeof(SearchableAttribute), false).Any())
                            {
                                var attr = subItem.GetCustomAttribute<SearchableAttribute>();
                                if (attr.SearchableNestedProperty)
                                {
                                    result.Add(item.Name + "." + subItem.Name);
                                }
                            }
                        }
                    }
                    else  //else it is a property.
                    {
                        result.Add(item.Name);
                    }
                }
            }

            return result;
        }
    }
}
