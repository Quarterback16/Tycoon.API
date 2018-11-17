using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.Interfaces.Calendar
{
    /// <summary>
    /// Defines an interface for each category Item to be displayed on calendar.
    /// </summary>
    public interface ICategoryItemViewModel //: IComparer
    {

        /// <summary>
        /// Title of the event.
        /// </summary>
        string Title { get; set; }


        /// <summary>
        /// Description for the event. This property represents the Description field on view-model.
        /// </summary>
        string Description { get; set; }

       
        /// <summary>
        /// Start DateTime for the event.
        /// </summary>
        DateTime? Start { get; set; }


        /// <summary>
        /// End DateTime for the event.
        /// </summary>
        DateTime? End { get; set; }


        /// <summary>
        /// Indicates whether the ModelState or view model in general is valid or invalid.
        /// </summary>
        bool ModelStateIsValid { get; set; }

        /// <summary>
        /// Unique Identifier for the event.
        /// </summary>
        string Id { get; set; }


        /// <summary>
        /// Indicates whether the event is allDay event.
        /// </summary>
        bool AllDay { get; set; }



        /////<summary> 
        /////Compares two objects and returns a value indicating whether one is less than,
        /////equal to, or greater than the other. 
        /////</summary>
        /////<param name="item1">The first object to compare.</param>
        /////<param name="item2">The second object to compare.</param>
        /////<returns>
        /////A signed integer that indicates the relative values of item1 and item2, as shown
        /////in the following table. Value Meaning Less than zero item1 is less than item2. Zero
        /////item1 equals item2. Greater than zero item1 is greater than item2.
        /////</returns>
        /////<exception cref="System.ArgumentException">
        /////Neither item1 nor item2 implements the System.IComparable interface.-or- item1 and item2
        /////are of different types and neither one can handle comparisons with the other.
        /////</exception>
        //int Compare(ICategoryItemViewModel item1, ICategoryItemViewModel item2);
       
    }
}
