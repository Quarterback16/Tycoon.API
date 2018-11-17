using System;
using Castle.ActiveRecord;

namespace Castle
{
	[ActiveRecord("Posts")]
	public class Post : ActiveRecordBase<Post>
	{
		public Post( string post )
		{
			Title = post;
		}

		[PrimaryKey( PrimaryKeyType.Native, "post_id" )]
		public int Id { get; set; }

		[Property( "post_title" )]
		public String Title { get; set; }


		[Property( "post_contents", ColumnType = "StringClob" )]
		public String Contents { get; set; }

		[Property( "post_category" )]
		public String Category { get; set; }

		[Property( "post_published" )]
		public bool Published { get; set; }

		[Property( "post_created" )]
		public DateTime Created { get; set; }
		
		[BelongsTo( "post_blogid" )]
		public Blog Blog { get; set; }

		public Post()
		{
			
		}

		public Post( string title, string contents, string category, bool published, DateTime created, Blog blog )
		{
			Title = title;
			Contents = contents;
			Category = category;
			Published = published;
			Created = created;
			Blog = blog;
		}
	}
}
