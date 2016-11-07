using System;
using System.Collections.Generic;
using System.Text;

using Common.Users;

namespace Common.WorkItems
{
    public class Comment
    {
        public Comment()
        {

        }

        public string Body { get; set; }
        public Identity Author { get; set; }
    }
}
