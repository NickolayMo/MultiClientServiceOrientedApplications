using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Utils
{
    public static class PropertySupport
    {
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");

            }
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null) 
            {
                throw new ArgumentNullException("memberExperssion");
            }
            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            var getMethod = property.GetGetMethod(true);
            if (getMethod == null)
            {
                throw new ArgumentNullException("static method");
            }
            return memberExpression.Member.Name;
        }
    }
}
