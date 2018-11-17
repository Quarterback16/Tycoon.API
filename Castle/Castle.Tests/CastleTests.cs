using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Castle.Tests
{
	[TestClass]
	public class CastleTests
	{
		[TestMethod]
		public void TestPostBuilder()
		{
			var testPost = new PostBuilder()
				.WithTitle( "Steves Title" )
				.WithContents( "Steves Contents" )
				.WithCategory( "Steves Category" )
				.WithPublished( true )
				.WithCreated( new DateTime( 2015, 08, 24 ) )
				.WithBlog( new Blog() )
				.Build();

			Assert.IsTrue( testPost.Title.Equals( "Steves Title" ) );
		}
	}
}
