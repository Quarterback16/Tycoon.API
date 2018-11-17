using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContosoUniversity.Models
{
   public class Course
   {
      /// <summary>
      /// Basically, this attribute lets you enter the primary key for the course rather than 
      /// having the database generate it.
      /// </summary>
      [DatabaseGenerated(DatabaseGeneratedOption.None)]

      public int CourseID { get; set; }
      public string Title { get; set; }
      public int Credits { get; set; }

      /// <summary>
      /// The Enrollments property is a navigation property. A Course entity can be related to
      /// any number of Enrollment entities
      /// </summary>
      public virtual ICollection<Enrollment> Enrollments { get; set; }
   }
}