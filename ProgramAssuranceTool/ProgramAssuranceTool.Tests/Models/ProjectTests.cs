using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramAssuranceTool.Models;

namespace ProgramAssuranceTool.Tests.Models
{
	[TestClass]
	public class ProjectTests
	{
		[TestMethod]
		public void ShouldNotBeAllowedToEditIfNotCreator()
		{
			var project = new Project
				{
					ProjectName = "Steves Unit Testing Project",
					Coordinator = "SC0779",
					ProjectType = "BOGUS"
				};
			Assert.IsFalse( project.CanEdit( "MB1212", false )  );
		}
	}
}
