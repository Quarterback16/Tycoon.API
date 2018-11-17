using System;
using System.Security.Claims;
using System.Web.Routing;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using System.IdentityModel.Claims;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Menu model for <see cref="MenuAttribute" />.
    /// </summary>
    [Serializable]
    public class MenuModel
    {
        /// <summary>
        /// Name of menu item.
        /// </summary>
        public string Name { get; set; }

        private int? order;

        /// <summary>
        /// Order in which the menu item should appear.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="int.MaxValue" /> if not set.
        /// </remarks>
        public int Order
        {
            get
            {
                return order.HasValue ? order.Value : int.MaxValue;
            }
            set
            {
                order = value;
            }
        }

        /// <summary>
        /// The parent Controller of the current action.
        /// </summary>
        public string ParentController { get; set; }

        /// <summary>
        /// The parent action of the current action.
        /// </summary>
        public string ParentAction { get; set; }

        /// <summary>
        /// The parent area of the current action.
        /// </summary>
        public string ParentArea { get; set; }

        /// <summary>
        /// Controller area
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Controller
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public string[] Parameters { get; set; }

        /// <summary>
        /// Parent menu
        /// </summary>
        public MenuModel Parent { get; set; }

        /// <summary>
        /// controller security
        /// </summary>
        public SecurityModel ControllerSecurity { get; set; }

        /// <summary>
        /// action security
        /// </summary>
        public SecurityModel ActionSecurity { get; set; }

        /// <summary>
        /// url
        /// </summary>
        public string Url
        {
            get
            {
                var route = RouteTable.Routes.GetVirtualPath(null, new RouteValueDictionary(new {area = Area, controller = Controller, action = Action}));

                return route != null ? route.VirtualPath : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has child items.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has child items; otherwise, <c>false</c>.
        /// </value>
        public bool HasChildItems { get; set; }

        /// <summary>
        /// Gets or sets the menu item identifier.
        /// </summary>
        public short MenuItemId { get; set; }

        /// <summary>
        /// Gets or sets the parent menu item identifier.
        /// </summary>
        public short ParentMenuItemId { get; set; }

        /// <summary>
        /// Gets or sets the static URL.
        /// </summary>
        public string StaticUrl { get; set; }

        /// <summary>
        /// Whether the user is authorized to see the menu item.
        /// </summary>
        public bool IsAuthorized(ClaimsIdentity identity)
        {
            bool controllerAuthorized = ControllerSecurity != null && ControllerSecurity.IsAuthorized(identity);
	        bool actionAuthorized = ActionSecurity != null && ActionSecurity.IsAuthorized(identity);

            // Authorized if:
            // - controller authorized and:
            //      - action authorized or no action security specified (use controller security)
            // - or action authorized.
            return (controllerAuthorized && (actionAuthorized || ActionSecurity == null)) || actionAuthorized;
        }
        /// <summary>
        /// Whether the user is authorized to see the menu item.
        /// </summary>
        internal bool IsAuthorized(ClaimsIdentity identity, bool isPreprodOrProd)
        {
            bool controllerAuthorized = ControllerSecurity != null && ControllerSecurity.IsAuthorized(identity, isPreprodOrProd);
            bool actionAuthorized = ActionSecurity != null && ActionSecurity.IsAuthorized(identity, isPreprodOrProd);

            // Authorized if:
            // - controller authorized and:
            //      - action authorized or no action security specified (use controller security)
            // - or action authorized.
            return (controllerAuthorized && (actionAuthorized || ActionSecurity == null)) || actionAuthorized;
        }
        /// <summary>
        /// Whether the user is authorized to see the menu item.
        /// </summary>
        internal bool IsAuthorized(ClaimSubset identity, bool isPreprodOrProd)
        {
            bool controllerAuthorized = ControllerSecurity != null && ControllerSecurity.IsAuthorized(identity, isPreprodOrProd);
            bool actionAuthorized = ActionSecurity != null && ActionSecurity.IsAuthorized(identity, isPreprodOrProd);

            // Authorized if:
            // - controller authorized and:
            //      - action authorized or no action security specified (use controller security)
            // - or action authorized.
            return (controllerAuthorized && (actionAuthorized || ActionSecurity == null)) || actionAuthorized;
        }

        /// <summary>
        /// Override equals for custom comparison.
        /// </summary>
        /// <param name="obj">Other object.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var model = obj as MenuModel;

            if (model == null)
            {
                return false;
            }

            var parentMatch = (model.Parent == null && Parent == null) || (model.Parent != null && Parent != null && model.Parent.Equals(Parent));
	        if (!parentMatch){return false;}
	        var actionSecurityMatch = (model.ActionSecurity == null && ActionSecurity == null) || (model.ActionSecurity != null && ActionSecurity != null && model.ActionSecurity.Equals(ActionSecurity));
	        if (!actionSecurityMatch){return false;}
            var controllerSecurityMatch = (model.ControllerSecurity == null && ControllerSecurity == null) || (model.ControllerSecurity != null && ControllerSecurity != null && model.ControllerSecurity.Equals(ControllerSecurity));
	        if (!controllerSecurityMatch){return false;}

            return (parentMatch && actionSecurityMatch && controllerSecurityMatch &&
                    string.Equals(model.Action, Action,StringComparison.Ordinal) &&
                    string.Equals(model.Controller, Controller,StringComparison.Ordinal) &&
                    string. Equals(model.Area, Area,StringComparison.Ordinal) &&
                    string.Equals(model.Name, Name,StringComparison.Ordinal) &&
					model.Order == Order &&
                    string.Equals(model.ParentAction, ParentAction,StringComparison.Ordinal) &&
                    string.Equals(model.ParentController, ParentController,StringComparison.Ordinal) &&
                    string.Equals(model.ParentArea, ParentArea,StringComparison.Ordinal));
        }

        /// <summary>
        /// Override GetHashCode for object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 1; 
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ParentController != null ? ParentController.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ParentAction != null ? ParentAction.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ParentArea != null ? ParentArea.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Area != null ? Area.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Controller != null ? Controller.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Action != null ? Action.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Parent != null ? Parent.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ControllerSecurity != null ? ControllerSecurity.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ActionSecurity != null ? ActionSecurity.GetHashCode() : 0);
                return hashCode;
            }
        }

    }
}
