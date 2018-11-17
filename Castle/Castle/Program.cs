using System;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

namespace Castle
{
	class Program
	{
		static void Main( string[] args )
		{
			var source = System.Configuration.ConfigurationManager.GetSection( "activerecord" ) as IConfigurationSource;
			ActiveRecordStarter.Initialize( source, typeof( Blog ),  typeof( Post ) );

			var blog = new Blog {Author = "Steve", Name = "Swiki"};
			blog.Save();

			var gotBlog = Blog.Find( 1 );
			gotBlog.Posts.Add( new Post("This is the first post")  );

			Console.WriteLine( blog.Name );
			Console.ReadLine();
		}
	}
}
