using System;
using System.Collections.Generic;
using Castle.ActiveRecord;

namespace Castle
{
	[ActiveRecord( "Blogs" )]
	public class Blog : ActiveRecordBase<Blog>
	{
		[PrimaryKey( PrimaryKeyType.Native, "blog_id" )]
		public int Id { get; set; }

		[Property( "blog_name" )]
		public String Name { get; set; }

		[Property( "blog_author" )]
		public String Author { get; set; }

      [HasMany( typeof( Post ), Table = "Posts", ColumnKey = "post_blogid", Cascade = ManyRelationCascadeEnum.SaveUpdate )]
		public IList<Post> Posts { get; set; }

		public Blog()
		{
			
		}
	}
}
