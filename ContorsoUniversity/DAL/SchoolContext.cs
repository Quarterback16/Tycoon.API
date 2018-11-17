using ContosoUniversity.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ContosoUniversity.DAL
{
   /// <summary>
   /// This code creates a DbSet property for each entity set. In Entity Framework terminology, an entity set typically 
   /// corresponds to a database table, and an entity corresponds to a row in the table.
   /// </summary>
   public class SchoolContext : DbContext
   {
      /// <summary>
      ///   The name of the connection string is passed in to the constructor.
      ///   If you don't specify a connection string or the name of one explicitly, 
      ///   Entity Framework assumes that the connection string name is the same as the class name. 
      ///   The default connection string name in this example would then be SchoolContext, 
      ///   the same as what you're specifying explicitly.
      /// </summary>
      public SchoolContext()
         : base("SchoolContext")
      {
      }

      public DbSet<Student> Students { get; set; }

      public DbSet<Enrollment> Enrollments { get; set; }

      public DbSet<Course> Courses { get; set; }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         //
         // The modelBuilder.Conventions.Remove statement in the OnModelCreating method prevents 
         // table names from being pluralized. If you didn't do this, the generated tables in the 
         // database would be named Students, Courses, and Enrollments. Instead, the table names 
         // will be Student, Course, and Enrollment. Developers disagree about whether table names 
         // should be pluralized or not.
         //
         modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
      }
   }
}