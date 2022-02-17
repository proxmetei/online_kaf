using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace OnlineChat.Models
{
	public class LogFactory
	{
		private ILog log;
		public LogFactory()
		{
			XmlConfigurator.Configure(System.IO.File.OpenRead("log4net.config.xml"));
			log = LogManager.GetLogger("Logger");
		}
		public ILog GetLogger()
		{
			return log;
		}
	}
}
