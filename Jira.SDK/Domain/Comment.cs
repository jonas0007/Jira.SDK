using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
    public class Comment
    {
        public User Author { get; set; }
        public String Body { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public User UpdateAuthor { get; set; }

		public Comment()
		{ }

		public Comment(String body)
		{
			this.Body = body;
		}
    }

    public class CommentSearchResult
    {
        public List<Comment> Comments { get; set; }
    }
}
