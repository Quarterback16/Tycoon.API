using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using NHibernate;
using NUnit.Framework;

namespace Tests.Unit.Mappings
{
	[TestFixture]
	public class EmployeeMappingsTests
	{
		private InMemoryDatabaseForXmlMappings database;
		private ISession session;

		[TestFixtureSetUp]
		public void Setup()
		{
			database = new InMemoryDatabaseForXmlMappings();
			session = database.Session;
		}

		[Test]
		public void MapsPrimitiveProperties()
		{
			object id;
			using ( var transaction = session.BeginTransaction() )
			{
				id = session.Save( new Employee
				{
					EmployeeNumber = "5987123",
					Firstname = "Hillary",
					Lastname = "Gamble",
					EmailAddress = "hillary.gamble@corporate.com",
					DateOfBirth = new DateTime( 1980, 4, 23 ),
					DateOfJoining = new DateTime( 2010, 7, 12 ),
					IsAdmin = true,
					Password = "Password"
				} );
				transaction.Commit();
			}
			session.Clear();
			using ( var transaction = session.BeginTransaction() )
			{
				var employee = session.Get<Employee>( id );
				Assert.That( employee.EmployeeNumber, Is.EqualTo( "5987123" ) );
				Assert.That( employee.Firstname, Is.EqualTo( "Hillary" ) );
				Assert.That( employee.Lastname, Is.EqualTo( "Gamble" ) );
				Assert.That( employee.EmailAddress, Is.EqualTo( "hillary.gamble@corporate.com" ) );
				Assert.That( employee.DateOfBirth.Year, Is.EqualTo( 1980 ) );
				Assert.That( employee.DateOfBirth.Month, Is.EqualTo( 4 ) );
				Assert.That( employee.DateOfBirth.Day, Is.EqualTo( 23 ) );
				Assert.That( employee.DateOfJoining.Year, Is.EqualTo( 2010 ) );
				Assert.That( employee.DateOfJoining.Month, Is.EqualTo( 7 ) );
				Assert.That( employee.DateOfJoining.Day, Is.EqualTo( 12 ) );
				Assert.That( employee.IsAdmin, Is.True );
				Assert.That( employee.Password, Is.EqualTo( "Password" ) );
				transaction.Commit();
			}
		}

		[Test]
		public void MapsBenefits()
		{
			object id;
			using ( var transaction = session.BeginTransaction() )
			{
				id = session.Save( new Employee
				{
					EmployeeNumber = "123456789",
					Benefits = new HashSet<Benefit>
					{
						new SkillsEnhancementAllowance
						{
							Entitlement = 1000,
							RemainingEntitlement = 250
						},
						new SeasonTicketLoan
						{
							Amount = 1416,
							MonthlyInstalment = 118,
							StartDate = new DateTime( 2014, 4, 25 ),
							EndDate = new DateTime( 2015, 3, 25 )
						},
						new Leave
						{
							AvailableEntitlement = 30,
							RemainingEntitlement = 15,
							Type = LeaveType.Paid
						}
					}
				} );
				transaction.Commit();
			}

			session.Clear();

			using ( var transaction = session.BeginTransaction() )
			{
				var employee = session.Get<Employee>( id );
				Assert.That( employee.Benefits.Count, Is.EqualTo( 3 ) );

				var seasonTicketLoan = employee.Benefits.OfType<SeasonTicketLoan>().
					FirstOrDefault();
				Assert.That( seasonTicketLoan, Is.Not.Null );
				if (seasonTicketLoan != null)
				{
					Assert.That( seasonTicketLoan.Employee.EmployeeNumber,
						Is.EqualTo( "123456789" ) );
				}
				var skillsEnhancementAllowance = employee.Benefits
					.OfType<SkillsEnhancementAllowance>().FirstOrDefault();
				Assert.That( skillsEnhancementAllowance, Is.Not.Null );
				if (skillsEnhancementAllowance != null)
				{
					Assert.That( skillsEnhancementAllowance.Employee.EmployeeNumber,
						Is.EqualTo( "123456789" ) );
				}
				var leave = employee.Benefits.OfType<Leave>().FirstOrDefault();
				Assert.That( leave, Is.Not.Null );
				if (leave != null)
				{
					Assert.That( leave.Employee.EmployeeNumber,
						Is.EqualTo( "123456789" ) );
				}

				transaction.Commit();
			}
		}
	}
}