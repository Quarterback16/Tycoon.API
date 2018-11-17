using ContosoUniversity.Models;
using System;
using System.Collections.Generic;

namespace ContosoUniversity.DAL
{
   /// <summary>
   /// The Entity Framework can automatically create (or drop and re-create) a database for you when the 
   /// application runs. You can specify that this should be done every time your application runs or 
   /// only when the model is out of sync with the existing database. You can also write a Seed method 
   /// that the Entity Framework automatically calls after creating the database in order to populate it with test data.
   /// The default behavior is to create a database only if it doesn't exist (and throw an exception if 
   /// the model has changed and the database already exists). Herel specify that the database should be dropped and 
   /// re-created whenever the model changes. Dropping the database causes the loss of all your data. This is generally 
   /// OK during development, because the Seed method will run when the database is re-created and will re-create your 
   /// test data. But in production you generally don't want to lose all your data every time you need to change the 
   /// database schema. Later you'll see how to handle model changes by using Code First Migrations to change the 
   /// database schema instead of dropping and re-creating the database.
   /// </summary>
   public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SchoolContext>
   {
      /// <summary>
      ///  The Seed method takes the database context object as an input parameter, and the code in 
      ///  the method uses that object to add new entities to the database. For each entity type, 
      ///  the code creates a collection of new entities, adds them to the appropriate DbSet property, 
      ///  and then saves the changes to the database. It isn't necessary to call the SaveChanges method 
      ///  after each group of entities, as is done here, but doing that helps you locate the source of 
      ///  a problem if an exception occurs while the code is writing to the database.
      /// </summary>
      /// <param name="context"></param>
      protected override void Seed(SchoolContext context)
      {
         var students = new List<Student>
         {
         new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
         new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
         new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
         new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
         new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
         new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
         new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
         new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
         };
         students.ForEach(s => context.Students.Add(s));
         context.SaveChanges();
         var courses = new List<Course>
         {
         new Course{CourseID=1050,Title="Chemistry",Credits=3,},
         new Course{CourseID=4022,Title="Microeconomics",Credits=3,},
         new Course{CourseID=4041,Title="Macroeconomics",Credits=3,},
         new Course{CourseID=1045,Title="Calculus",Credits=4,},
         new Course{CourseID=3141,Title="Trigonometry",Credits=4,},
         new Course{CourseID=2021,Title="Composition",Credits=3,},
         new Course{CourseID=2042,Title="Literature",Credits=4,}
         };
         courses.ForEach(s => context.Courses.Add(s));
         context.SaveChanges();
         var enrollments = new List<Enrollment>
         {
         new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
         new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
         new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
         new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
         new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
         new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
         new Enrollment{StudentID=3,CourseID=1050},
         new Enrollment{StudentID=4,CourseID=1050,},
         new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
         new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
         new Enrollment{StudentID=6,CourseID=1045},
         new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
         };
         enrollments.ForEach(s => context.Enrollments.Add(s));
         context.SaveChanges();
      }
   }
}