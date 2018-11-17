using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ProgramAssuranceTool.Tests.Models
{
	[TestClass]
    [Ignore]
	public class BulletinTests
	{
		[TestMethod]
		public void ShouldNotAllowEndDateBeforeStartDate()
		{
			var bulletin = new Bulletin
			{
				BulletinTitle = "Unit Testing",
				Description = "Dummy description for Unit Testing",
				BulletinType = "BOGUS",
				StartDate = new DateTime( 2012, 10, 1 ),
				EndDate = new DateTime( 2011, 10, 1 )
			};

			var result = ValidateBulletin( bulletin );

			Assert.IsTrue( ErrorFound( result, "End Date must not be before Start Date" ) );
		}

		[TestMethod]
		public void ShouldNotAllowEndDateWithoutStartDate()
		{
			var bulletin = new Bulletin
			{
				BulletinTitle = "Unit Testing",
				Description = "Dummy description for Unit Testing",
				BulletinType = "BOGUS",
				EndDate = new DateTime( 2011, 10, 1 )
			};

			var result = ValidateBulletin( bulletin );

			Assert.IsTrue( ErrorFound( result, "There must be a Start Date" ) );
		}

		[TestMethod]
		public void ShouldAlwaysBeABulletinTitle()
		{
			var bulletin = new Bulletin
			{
				Description = "Dummy description for Unit Testing",
				BulletinType = "BOGUS",
				StartDate = new DateTime( 2012, 10, 1 ),
				EndDate = new DateTime( 2013, 10, 1 )
			};

			var result = ValidateBulletin( bulletin );

			Assert.IsTrue( ErrorFound( result, "The Title field is required." ) );
		}

		[TestMethod]
		public void ShouldAlwaysBeABulletinDescription()
		{
			var bulletin = new Bulletin
			{
				BulletinTitle = "Unit Testing",
				BulletinType = "BOGUS",
				StartDate = new DateTime( 2012, 10, 1 ),
				EndDate = new DateTime( 2013, 10, 1 )
			};

			var result = ValidateBulletin( bulletin );

			Assert.IsTrue( ErrorFound( result, "The Description field is required." ) );
		}

		private static List<ValidationResult> ValidateBulletin( Bulletin bulletin )
		{
			var context = new ValidationContext( bulletin, null, null );
			var result = new List<ValidationResult>();
			Validator.TryValidateObject( bulletin, context, result, true );
			return result;
		}

		private static bool ErrorFound( IEnumerable<ValidationResult> result, string errorMsg )
		{
			var errorFound = false;
			foreach ( var validationResult in result
				.Where( validationResult => validationResult != null )
				.Where( validationResult => validationResult.ErrorMessage.Equals( errorMsg ) ) )
				errorFound = true;
			return errorFound;
		}

	}
}
