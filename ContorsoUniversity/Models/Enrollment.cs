namespace ContosoUniversity.Models
{
   public enum Grade
   {
      A, B, C, D, F
   }

   public class Enrollment
   {
      /// <summary>
      /// The EnrollmentID property will be the primary key; this entity uses the classnameID 
      /// pattern instead of ID by itself as you saw in the Student entity. Ordinarily you 
      /// would choose one pattern and use it throughout your data model. Here, the variation 
      /// illustrates that you can use either pattern. Later you'll you'll see 
      /// how using ID without classname makes it easier to implement inheritance in the data model.
      /// </summary>
      public int EnrollmentID { get; set; }

      /// <summary>
      /// The CourseID property is a foreign key, and the corresponding navigation property is Course. 
      /// An Enrollment entity is associated with one Course entity.
      /// </summary>
      public int CourseID { get; set; }

      /// <summary>
      /// The StudentID property is a foreign key, and the corresponding navigation property is 
      /// Student. An Enrollment entity is associated with one Student entity, so the property 
      /// can only hold a single Student entity (unlike the Student.Enrollments navigation property 
      /// you saw earlier, which can hold multiple Enrollment entities).
      /// </summary>
      public int StudentID { get; set; }

      /// <summary>
      /// The Grade property is an enum. The question mark after the Grade type declaration 
      /// indicates that the Grade property is nullable. A grade that's null is different 
      /// from a zero grade — null means a grade isn't known or hasn't been assigned yet.
      /// </summary>
      public Grade? Grade { get; set; }

      public virtual Course Course { get; set; }

      public virtual Student Student { get; set; }
   }
}