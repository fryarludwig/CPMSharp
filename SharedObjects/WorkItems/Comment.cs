using System;
using System.Collections.Generic;
using System.Text;

using SharedObjects.Users;

namespace SharedObjects.WorkItems
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
