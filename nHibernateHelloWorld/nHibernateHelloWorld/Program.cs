using System;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;

namespace nHibernateHelloWorld
{
	class Program
	{
		static ISessionFactory factory;

		static void Main( string[] args )
		{
			CreateEmployeeAndSaveToDatabase();
			UpdateTobinAndAssignPierreHenriAsManager();
			LoadEmployeesFromDatabase();
			Console.WriteLine( "Press any key to exit..." );
			Console.ReadKey();
		}

		static void CreateEmployeeAndSaveToDatabase()
		{
			var tobin = new Employee {name = "Tobin Harris"};
			using ( var session = OpenSession() )
			{
				using ( var transaction = session.BeginTransaction() )
				{
					session.Save( tobin );
					transaction.Commit();
				}
				Console.WriteLine( "Saved Tobin to the database" );
			}
		}

		static ISession OpenSession()
		{
			if (factory != null) return factory.OpenSession();

			LoggerProvider.SetLoggersFactory( new NoLoggingLoggerFactory() );

			var c = new Configuration();
			//c.DataBaseIntegration( x =>
			//{
			//	x.ConnectionString = "Server=DA313679\\MSSQLSERVER2012;Database=HelloNHibernate;Integrated Security=SSPI";
			//	x.Driver<SqlClientDriver>();
			//	x.Dialect<MsSql2012Dialect>();
			//	x.LogSqlInConsole = true;
			//} );
			c.AddAssembly( Assembly.GetCallingAssembly() );
			factory = c.BuildSessionFactory();
			return factory.OpenSession();
		}

		static void LoadEmployeesFromDatabase()
		{
			using ( var session = OpenSession() )
			{
				var query = session.CreateQuery( "from Employee as emp order by emp.name asc" );  //  this is HQL
				var foundEmployees = query.List<Employee>();
				Console.WriteLine( "\n{0} employees found:", foundEmployees.Count );
				foreach ( var employee in foundEmployees )
					Console.WriteLine( employee.SayHello() );
			}
		}

		static void UpdateTobinAndAssignPierreHenriAsManager()
		{
			using ( var session = OpenSession() )
			{
				using ( var transaction = session.BeginTransaction() )
				{
					var q = session.CreateQuery( "from Employee where name = 'Tobin Harris'" );
					var tobin = q.List<Employee>()[ 0 ];
					tobin.name = "Tobin David Harris";
					var pierreHenri = new Employee {name = "Pierre Henri Kuate"};
					tobin.manager = pierreHenri;
					transaction.Commit();
					Console.WriteLine( "Updated Tobin and added Pierre Henri" );
				}
			}
		}
	}
}
