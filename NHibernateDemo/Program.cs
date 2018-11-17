using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using System;
using System.Linq;
using HibernatingRhinos.Profiler.Appender.NHibernate;

namespace NHibernateDemo
{
   internal class Program
   {
      private static void Main( string[] args )
      {//App_Start.NHibernateProfilerBootstrapper.PreStart();
         NHibernateProfiler.Initialize();
         var cfg = new Configuration();
         cfg.DataBaseIntegration( x =>
         {
            x.ConnectionString = "Server=DELOOCH\\SQLEXPRESS;Database=NHibernateDemo;Integrated Security=SSPI";
            x.Driver<SqlClientDriver>();
            x.Dialect<MsSql2012Dialect>();
            x.LogSqlInConsole = true;
         } );

         cfg.AddAssembly( Assembly.GetExecutingAssembly() );  // for mappings which have been baked in
         var sessionFactory = cfg.BuildSessionFactory();


         //using ( var session = sessionFactory.OpenSession() )
         //using ( var tx = session.BeginTransaction() )
         //{
            // Fetch!

            //var customers = session.CreateCriteria<Customer>()
            //   .List<Customer>();
            
            //  Linq query approach
            //var customers = from customer in session.Query<Customer>()
            //                orderby customer.LastName
            //                where customer.LastName.Length > 5
            //                select customer;
            //foreach (var customer in customers)
            //{
            //   Console.WriteLine("{0} {1}", customer.FirstName, customer.LastName);
            //}

         //   // adding data!
         //   var customer = new Customer
         //   {
         //      FirstName = "Shaun",
         //      LastName = "Hill"
         //   };
         //   session.Save(customer);
         //   Console.WriteLine("{0} created", customer.Id);
         //   tx.Commit();
         //}


         //// read it back out
         //using ( var session = sessionFactory.OpenSession() )
         //using (var tx = session.BeginTransaction())
         //{
         //   var query = from customer in session.Query<Customer>()
         //               where customer.LastName == "Hill"
         //               select customer;
         //   var retieved = query.Single();
         //   Console.WriteLine("{0} {1} {2}", retieved.FirstName, retieved.LastName, retieved.Id );
         //}

         //// update
         //using (var session = sessionFactory.OpenSession())
         //using (var tx = session.BeginTransaction())
         //{
         //   var query = from customer in session.Query<Customer>()
         //               where customer.LastName == "Hill"
         //               select customer;
         //   var c = query.First();
         //   c.FirstName = "Raplh";
         //   session.Update(c);
         //   tx.Commit();
         //}

         //// delete
         //using (var session = sessionFactory.OpenSession())
         //using (var tx = session.BeginTransaction())
         //{
         //   var query = from customer in session.Query<Customer>()
         //               where customer.LastName == "Hill"
         //               select customer;
         //   var c = query.First();
         //   session.Delete(c);
         //   tx.Commit();
         //}

         Console.WriteLine("press ENTER to exit");
         Console.ReadLine();


      }
   }
}

