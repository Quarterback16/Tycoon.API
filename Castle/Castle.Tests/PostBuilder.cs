using System;

namespace Castle.Tests
{
	public class PostBuilder
	{
		public string Title { get; set; }
		public string Contents { get; set; }
		public string Category { get; set; }
		public bool Published { get; set; }
		public DateTime Created { get; set; }
		public Blog Blog { get; set; }

		public PostBuilder()
		{
			Title = "Default Title";
			Contents = "Default Contents";
			Category = "Default Category";
			Published = false;
			Created = new DateTime( 2015, 1, 1 );
			Blog = new Blog();
		}

		public PostBuilder WithTitle( string title )
		{
			Title = title;
			return this;
		}

		public PostBuilder WithContents( string contents )
		{
			Contents = contents;
			return this;
		}

		public PostBuilder WithCategory( string category )
		{
			Category = category;
			return this;
		}

		public PostBuilder WithPublished( bool published )
		{
			Published = published;
			return this;
		}

		public PostBuilder WithCreated( DateTime created )
		{
			Created = created;
			return this;
		}

		public PostBuilder WithBlog( Blog blog )
		{
			Blog = blog;
			return this;
		}

		public Post Build()
		{
			return new Post(
					Title     ,
					Contents  ,
					Category  ,
					Published ,
					Created   ,
					Blog      
				);
		}
	}
}
