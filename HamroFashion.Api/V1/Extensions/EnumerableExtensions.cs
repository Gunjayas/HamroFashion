using System.Linq.Expressions;
using System.Reflection;

namespace HamroFashion.Api.V1.Extensions
{
    /// <summary>
    /// Some helpers when working with IEnumerables
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Allows you to order by the name of a field
        /// </summary>
        /// <typeparam name="T">Derrived from source, the type of enumerable thing</typeparam>
        /// <param name="source">The enumerable of things</param>
        /// <param name="orderBy">The name of the field to order by</param>
        /// <returns>Source but ordered by field name if possible</returns>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string orderBy, bool isDescending = false)
        {
            var instance = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(instance, orderBy);

            var propInfo = (PropertyInfo)prop.Member;
            var propType = propInfo.PropertyType;

            switch (propType.Name.ToLower())
            {
                case "bool":
                    var byBool = BuildOrderBySelector<T, bool>(instance, prop);
                    return isDescending ? source.OrderByDescending(byBool) : source.OrderBy(byBool);

                case "byte":
                    var byByte = BuildOrderBySelector<T, byte>(instance, prop);
                    return isDescending ? source.OrderByDescending(byByte) : source.OrderBy(byByte);

                case "char":
                    var byChar = BuildOrderBySelector<T, char>(instance, prop);
                    return isDescending ? source.OrderByDescending(byChar) : source.OrderBy(byChar);

                case "double":
                    var byDouble = BuildOrderBySelector<T, double>(instance, prop);
                    return isDescending ? source.OrderByDescending(byDouble) : source.OrderBy(byDouble);

                case "float":
                    var byFloat = BuildOrderBySelector<T, float>(instance, prop);
                    return isDescending ? source.OrderByDescending(byFloat) : source.OrderBy(byFloat);

                case "int":
                case "int32":
                    var byInt = BuildOrderBySelector<T, int>(instance, prop);
                    return isDescending ? source.OrderByDescending(byInt) : source.OrderBy(byInt);

                case "string":
                    var byString = BuildOrderBySelector<T, string>(instance, prop);
                    return isDescending ? source.OrderByDescending(byString) : source.OrderBy(byString);

                default:
                    return source.OrderBy(x => x);
            }
        }

        /// <summary>
        /// Helper method to simplify authoring a Where function that has an additional if condition
        /// </summary>
        /// <typeparam name="T">Derrived from source, the type of enumerable thing</typeparam>
        /// <param name="source">The enumerable of things</param>
        /// <param name="predicate">The where predicate (the x => x.whatever part)</param>
        /// <param name="condition">If true we run the where, if false we do not</param>
        /// <returns>Source but where'd if condition was true</returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        /// <summary>
        /// Helper used in the order by extension, builds the selector predicate for
        /// our OrderBy
        /// </summary>
        /// <typeparam name="T">They type of enumerable things we are defining a selector for</typeparam>
        /// <typeparam name="TProp">The type of the field we are selecting</typeparam>
        /// <param name="instance">The parameter expression representing an instance of T</param>
        /// <param name="prop">A member expression representing the property to select</param>
        /// <returns>Selector predicate function</returns>
        private static Func<T, TProp> BuildOrderBySelector<T, TProp>(ParameterExpression instance, MemberExpression prop)
        {
            var selector = Expression.Lambda<Func<T, TProp>>(prop, instance);
            return selector.Compile();
        }
    }
}
