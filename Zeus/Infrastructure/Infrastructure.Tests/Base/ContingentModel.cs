using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.Tests.Base
{
    public abstract class ContingentModel<T> where T : ContingentAttribute
    {
        public T GetAttribute(string property)
        {
            return (T)GetType().GetProperty(property).GetCustomAttributes(typeof(T), false)[0];
        }

        public bool IsConditionMet(string property)
        {
            var attribute = GetAttribute(property);
            return attribute.IsConditionMet(property, GetType().GetProperty(property).GetValue(this, null), this);
        }
    }
}
