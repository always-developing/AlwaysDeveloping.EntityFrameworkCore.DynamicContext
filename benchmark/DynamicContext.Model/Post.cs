using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicContext.Model
{
    public class Post
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int WordCount { get; set; }

        public Guid BlogId { get; set; }

        public Blog Blog { get; set; }
    }
}
