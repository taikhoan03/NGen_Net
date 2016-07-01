using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using System.IO;
using System.Reflection;
using log4net;
namespace NexGen
{
    public class ApplicationContextFactory
{
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private static IApplicationContext _context;
    public static IApplicationContext GetContext()
    {
        if (_context == null)
        {
            try
            {
                _context = new XmlApplicationContext("spring-config.xml");
            }
            catch (Exception e)
            {
                string error = e.Message;
                    log.Error(e.Message);
                    log.Error(e.StackTrace);
            }
        }
        return _context;
    }
}
}
