using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces.Calendar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Calendar
{
    /// <summary>
    /// Category Item View Model, implements the interface <see cref="ICategoryItemViewModel"/>.
    /// </summary>
    [Serializable]
    public class CategoryItemViewModel : ICategoryItemViewModel, IComparer
    {

        public CategoryItemViewModel()
        {
            this.IsEditable = true;
            this.HoverDescription = this.EventDescription;
        }


        // TODO: Must have a unique ID, so it can be identified from jQuery.
        /// <summary>
        /// Unique Id.
        /// </summary>
        /// <remarks>
        /// Must be unique across the entire calendar or list of items displayed on calendar. And not just unique for this category.
        /// </remarks>
        [Bindable]
        public string Id { get; set; }


        /// <summary>
        /// Start time.
        /// </summary>
        [Bindable]
        public DateTime? Start { get; set; }


        /// <summary>
        /// End time.
        /// </summary>
        [Bindable]
        public DateTime? End { get; set; }


        /// <summary>
        /// Title of the item.
        /// </summary>
        [Bindable]
        public string Title { get; set; }


        /// <summary>
        /// Description to be displayed for calendar event item.
        /// </summary>
        [Bindable]
        public ContentViewModel EventDescription { get; set; }


        /// <summary>
        /// This field contains the html for <see cref="CategoryItemViewModel.EventDescription"/>. This is ONLY for Framework Use.
        /// </summary>
        public string EventDescriptionHtml { get; set; }


        /// <summary>
        /// Description to be entered for category.
        /// </summary>
        [Bindable]
        public string Description { get; set; }


        /// <summary>
        /// Indicates whether the event is 'All Day event'.
        /// </summary>
        [Hidden]
        [Bindable]
        public bool AllDay { get; set; }


        [Hidden]
        public bool ModelStateIsValid { get; set; }


        /// <summary>
        /// Indicates whether this event is editable (i.e. Draggable AND Resizable). If, for this event's category <see cref="CategoryViewModel.DragResizeAction"/> is not defined then event will not be editable. Default is <c>True</c>.
        /// </summary>
        [Bindable]
        [Hidden]
        public bool IsEditable { get; set; }


        /// <summary>
        /// Contains the information that will displayed when user hovers over the event on calendar.
        /// </summary>
        [Bindable]        
        public ContentViewModel HoverDescription { get; set; }


        /// <summary>
        /// This field contains the html for <see cref="CategoryItemViewModel.HoverDescription"/>. This is ONLY for Framework Use.
        /// </summary>
        public string HoverDescriptionHtml { get; set; }


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
        public int Compare(object categoryItem1, object categoryItem2)
        {

            // TODO: Enhance this to do more sophisticated comparison.

            CategoryItemViewModel item1 = categoryItem1 as CategoryItemViewModel;
            CategoryItemViewModel item2 = categoryItem2 as CategoryItemViewModel;

            if (item1 == null)
            {
                throw new ArgumentException("Unable to parse categoryItem1 to CategoryItemViewModel.", "item1");
            }
            if (item2 == null)
            {
                throw new ArgumentException("Unable to parse categoryItem2 to CategoryItemViewModel.", "item2");
            }

            var compareResult = 0;



            long item1Id, item2Id;

            if (long.TryParse(item1.Id, out item1Id) && long.TryParse(item2.Id, out item2Id))
            {
                if (item1Id < item2Id)
                {
                    compareResult = -1;
                }
                else if (item1Id > item2Id)
                {
                    compareResult = 1;
                }
            }
            if (compareResult == 0)
            {
                if (item1.Start.HasValue && item2.Start.HasValue)
                {
                    compareResult = DateTime.Compare(item1.Start.Value, item2.Start.Value);
                }
                else if (item1.Start.HasValue)
                {
                    compareResult = DateTime.Compare(item1.Start.Value, DateTime.MinValue);
                }
                else if (item2.Start.HasValue)
                {
                    compareResult = DateTime.Compare(DateTime.MinValue, item2.Start.Value);
                }
            }
            if (compareResult == 0)
            {
                if (item1.End.HasValue && item2.End.HasValue)
                {
                    compareResult = DateTime.Compare(item1.End.Value, item2.End.Value);
                }
                else if (item1.End.HasValue)
                {
                    compareResult = DateTime.Compare(item1.End.Value, DateTime.MinValue);
                }
                else if (item2.End.HasValue)
                {
                    compareResult = DateTime.Compare(DateTime.MinValue, item2.End.Value);
                }
            }


            if (compareResult == 0)
            {
                compareResult = item1.Id.CompareTo(item2.Id);
            }
            return compareResult;
        }
    }
}
