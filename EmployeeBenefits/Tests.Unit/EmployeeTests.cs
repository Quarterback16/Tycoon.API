using System.Collections.Generic;
using System.Linq;
using Domain;
using NHibernate;
using NUnit.Framework;

namespace Tests.Unit
{
	[TestFixture]
	public class EmployeeTests
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
		public void EmployeeIsEntitledToPaidLeaves()
		{
			//Arrange
			var employee = new Employee
			{
				Leaves = new List<Leave>
				{
					new Leave
					{
						Type = LeaveType.Paid,
						AvailableEntitlement = 15
					}
				}
			};
			//Act
			//Assert
			var paidLeave = employee.Leaves.FirstOrDefault( l => l.Type
			                                                     == LeaveType.Paid );
			Assert.That( paidLeave, Is.Not.Null );
			if (paidLeave != null) Assert.That( paidLeave.AvailableEntitlement, Is.EqualTo( 15 ) );
		}

		[Test]
		public void MapsResidentialAddress()
		{
			object id;
			using ( var transaction = session.BeginTransaction() )
			{
				var residentialAddress = new Address
				{
					AddressLine1 = "Address line 1",
					AddressLine2 = "Address line 2",
					Postcode = "postcode",
					City = "city",
					Country = "country"
				};
				var employee = new Employee
				{
					EmployeeNumber = "123456789",
					ResidentialAddress = residentialAddress
				};
				residentialAddress.Employee = employee;
				id = session.Save( employee );
				transaction.Commit();
			}
			session.Clear();
			using ( var transaction = session.BeginTransaction() )
			{
				var employee = session.Get<Employee>( id );
				Assert.That( employee.ResidentialAddress.AddressLine1,
					Is.EqualTo( "Address line 1" ) );
				Assert.That( employee.ResidentialAddress.AddressLine2,
					Is.EqualTo( "Address line 2" ) );
				Assert.That( employee.ResidentialAddress.Postcode,
					Is.EqualTo( "postcode" ) );
				Assert.That( employee.ResidentialAddress.City,
					Is.EqualTo( "city" ) );
				Assert.That( employee.ResidentialAddress.Country,
					Is.EqualTo( "country" ) );
				Assert.That( employee.ResidentialAddress.Employee.EmployeeNumber, Is
					.EqualTo( "123456789" ) );
				transaction.Commit();
			}
		}

		[Test]
		public void MapsCommunities()
		{
			object id;
			using ( var transaction = session.BeginTransaction() )
			{
				id = session.Save( new Employee
				{
					EmployeeNumber = "123456789",
					Communities = new HashSet<Community>
					{
						new Community
						{
							Name = "Community 1"
						},
						new Community
						{
							Name = "Community 2"
						}
					}
				} );
				transaction.Commit();
			}
			session.Clear();
			using ( var transaction = session.BeginTransaction() )
			{
				var employee = session.Get<Employee>( id );
				Assert.That( employee.Communities.Count, Is.EqualTo( 2 ) );
				Assert.That( employee.Communities.First().Members.First().
					EmployeeNumber, Is.EqualTo( "123456789" ) );
				transaction.Commit();
			}
		}


		[Test]
		public void MapsSkillsEnhancementAllowance()
		{
			object id = 0;
			using ( var transaction = session.BeginTransaction() )
			{
				id = session.Save( new SkillsEnhancementAllowance
				{
					Name = "Skill Enhacement Allowance",
					Description = "Allowance for employees so that their skill enhancement trainings are paid for",
					Entitlement = 1000,
					RemainingEntitlement = 250
				} );
				transaction.Commit();
			}
			session.Clear();
			using ( var transaction = session.BeginTransaction() )
			{
				var benefit = session.Get<Benefit>( id );
				var skillsEnhancementAllowance = benefit as
					SkillsEnhancementAllowance;
				Assert.That( skillsEnhancementAllowance, Is.Not.Null );
				if (skillsEnhancementAllowance != null)
				{
					Assert.That( skillsEnhancementAllowance.Name, Is.EqualTo( "Skill Enhacement Allowance "));
					Assert.That( skillsEnhancementAllowance.Description,
						Is.EqualTo( "Allowance for employees so that their skill enhancement trainings are paid for"));
					Assert.That( skillsEnhancementAllowance.Entitlement,
						Is.EqualTo( 1000 ) );
					Assert.That( skillsEnhancementAllowance.RemainingEntitlement,
						Is.EqualTo( 250 ) );
				}
				transaction.Commit();
			}
		}
	}
}