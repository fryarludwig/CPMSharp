using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class WindowLoggingAdapter
    {

        public static ConcurrentQueue<LogItem> LogMessageQueue { get; set; }
    }
}
