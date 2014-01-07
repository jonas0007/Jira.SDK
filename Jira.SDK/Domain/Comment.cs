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
    }

    public class CommentSearchResult
    {
        public List<Comment> Worklogs { get; set; }
    }
}
