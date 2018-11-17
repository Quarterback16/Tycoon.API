using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.FilterProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Employment.Web.Mvc.Infrastructure.Tests.FilterProviders
{
    /// <summary>
    /// Unit tests for <see cref="ConditionalFilterProvider" />.
    /// </summary>
    [TestClass]
    public class ConditionalFilterProviderTest
    {
        public class TestController : Controller
        {
            [HttpPost]
            public ActionResult PostAction(FormCollection model)
            {
                return View();
            }
        }

        [TestMethod]
        public void ConditionalFilterProvider_ApplyValidateAntiForgeryToken_FiltersContainValidateAntiForgeryToken()
        {
            var controller = new TestController();

            var mockHttpContext = new Mock<HttpContextBase>();
            var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            ControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(typeof(TestController));

            var mockActionMethodInfo = controller.GetType().GetMethods().Where(x => x.Name.Equals("PostAction")).FirstOrDefault();

            ActionDescriptor actionDescriptor = new ReflectedActionDescriptor(mockActionMethodInfo, "PostAction", controllerDescriptor);

            var conditions = new Func<ControllerContext, ActionDescriptor, object>[]
            {
                // Apply [ValidateAntiForgeryTokenAttribute] to all actions with [HttpPost] that don't already have [ValidateAntiForgeryTokenAttribute]
                (c, a) => a.GetCustomAttributes(true).OfType<HttpPostAttribute>().FirstOrDefault() != null && a.GetCustomAttributes(true).OfType<ValidateAntiForgeryTokenAttribute>().FirstOrDefault() == null ? new ValidateAntiForgeryTokenAttribute() : null,
            };

            var conditionalProvider = new ConditionalFilterProvider(conditions);

            var filters = conditionalProvider.GetFilters(controllerContext, actionDescriptor);

            Assert.IsTrue(filters.Any(f => f.Instance.GetType() == typeof(ValidateAntiForgeryTokenAttribute)));
        }

        [TestMethod]
        public void ConditionalFilterProvider_ApplySecurity_FiltersContainSecurity()
        {
            var controller = new TestController();

            var mockHttpContext = new Mock<HttpContextBase>();
            var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);

            ControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(typeof(TestController));

            var mockActionMethodInfo = controller.GetType().GetMethods().Where(x => x.Name.Equals("PostAction")).FirstOrDefault();

            ActionDescriptor actionDescriptor = new ReflectedActionDescriptor(mockActionMethodInfo, "PostAction", controllerDescriptor);

            var conditions = new Func<ControllerContext, ActionDescriptor, object>[]
            {
                // Apply [SecurityAttribute] to all actions that don't already have the attribute but only if their controller also doesn't have the attribute (this will ensure the OnAuthorization will be run for all actions)
                (c, a) =>
                    {
                        var controllerSecurity = a.ControllerDescriptor.GetCustomAttributes(true).OfType<SecurityAttribute>().FirstOrDefault();
                        var actionSecurity = a.GetCustomAttributes(true).OfType<SecurityAttribute>().FirstOrDefault();

                        return controllerSecurity == null && actionSecurity == null ? new SecurityAttribute() : null;
                    }
            };

            var conditionalProvider = new ConditionalFilterProvider(conditions);

            var filters = conditionalProvider.GetFilters(controllerContext, actionDescriptor);

            Assert.IsTrue(filters.Any(f => f.Instance.GetType() == typeof(SecurityAttribute)));
        }

    }
}
