using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Calendar
{

    /// <summary>
    /// Represents View-model for each category.
    /// </summary>
    public class CategoryViewModel //: IComparer
    {



        /// <summary>
        /// Creates a new instance of <see cref="CategoryViewModel"/>.
        /// </summary>
        /// <param name="action">Action for handling specific events belonging to this category.</param>
        /// <param name="categoryName">The name/title of the category.</param>
        public CategoryViewModel(string categoryName, string action)
        {
            if (string.IsNullOrEmpty(action))
            {
                throw new ArgumentNullException("action");
            }
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentNullException("categoryname");
            }

            Action = action;
            CategoryName = categoryName;
        }


        /// <summary>
        /// Name of the Category.
        /// </summary>
        /// <remarks>
        /// Do not use Special Characters like %,&,!,@,#,$,^,* etc.
        /// </remarks>
        [Bindable]
        public string CategoryName { get; set; }

        /// <summary>
        /// Color in rgb() format.
        /// </summary>
        [Bindable]
        public ColourType Color { get; set; }


        /// <summary>
        /// Icon type. The value is from <see cref="IconType"/>.
        /// </summary>
        [Bindable]
        public string IconType { get; set; }


        /// <summary>
        /// Description of category.
        /// </summary>
        public ContentViewModel Description { get; set; }



        // Metadata (action, controller, area, routename)

        #region METADATA

        /// <summary>
        /// Action.
        /// </summary>
        [Bindable]
        public string Action { get; set; }


        /// <summary>
        /// Action for handling the Drag and Resize events of calendar events. If no value is specified, then drag and resize is disabled.
        /// </summary>
        /// <remarks>
        /// There will only be a one action for both Drag and Resize events. This action must be decorated with [AjaxOnly] and [HttpPost] filters.
        /// </remarks>
        /// <example>
        /// public ActionResult UpdateEventTimes(string id, DateTime start, DateTime end)
        /// </example>
        [Bindable]
        public string DragResizeAction { get; set; }


        /// <summary>
        /// Controller.
        /// </summary>
        [Bindable]
        public string Controller { get; set; }


        /// <summary>
        /// Area.
        /// </summary>
        [Bindable]
        public string Area { get; set; }


        /// <summary>
        /// Route Values.
        /// </summary>
        [Bindable]
        public string RouteName { get; set; }

        #endregion


        /// <summary>
        /// All items for current category.
        /// </summary>
        [Bindable]
        public IList<CategoryItemViewModel> Items { get; set; }



        /*
        ///<summary> 
        ///Compares two objects and returns a value indicating whether one is less than,
        ///equal to, or greater than the other. 
        ///</summary>
        ///<param name="item1">The first object to compare.</param>
        ///<param name="item2">The second object to compare.</param>
        ///<returns>
        ///A signed integer that indicates the relative values of item1 and item2, as shown
        ///in the following table. Value Meaning Less than zero item1 is less than item2. Zero
        ///item1 equals item2. Greater than zero item1 is greater than item2.
        ///</returns>
        ///<exception cref="System.ArgumentException">
        ///Neither item1 nor item2 implements the System.IComparable interface.-or- item1 and item2
        ///are of different types and neither one can handle comparisons with the other.
        ///</exception>
        public int Compare(object category1, object category2)
        {

            // TODO: Enhance this to do more sophisticated comparison.

            CategoryViewModel item1 = category1 as CategoryViewModel;
            CategoryViewModel item2 = category2 as CategoryViewModel;

            if (item1 == null)
            {
                throw new ArgumentException("Unable to parse item1 to CategoryViewModel.", "category1");
            }
            if (item2 == null)
            {
                throw new ArgumentException("Unable to parse item2 to CategoryViewModel.", "category2");
            }

            var compareResult = 0;

            compareResult = item1.CategoryName.CompareTo(item2.CategoryName);

            return compareResult;
        }
        */
    }
}
