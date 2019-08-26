using log4net.Core;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdfToMsSQL
{
    public class CustomLogger : LayoutSkeleton
    {
        public override void ActivateOptions()
        {
            throw new NotImplementedException();
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            throw new NotImplementedException();
        }
    }
}
