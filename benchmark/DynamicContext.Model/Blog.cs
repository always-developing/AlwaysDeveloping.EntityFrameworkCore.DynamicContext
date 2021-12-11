using System;
using System.Collections.Generic;

namespace DynamicContext.Model
{
    public class Blog
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Owner { get; set; }

        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }
}
