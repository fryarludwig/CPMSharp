using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Utilities;

namespace Common.Forms
{
    public static class WindowLoggingAdapter
    {
        public static ConcurrentQueue<LogItem> LogMessageQueue { get; set; }
    }
}
