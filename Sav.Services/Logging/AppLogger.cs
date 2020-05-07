using System;
using log4net.Config;
using Savchin.Logging;
using Savchin.Services.Storage;

namespace Savchin.Services.Logging
{
    public class AppLogger : LoggerBase
    {
     
        /// <summary>
        /// Gets or sets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static ILogger Default { get; set; }
      
        
        /// <summary>
        /// Initializes the <see cref="AppLogger"/> class.
        /// </summary>
        static AppLogger()
        {
            Environment.SetEnvironmentVariable("APPLOGS", PathProvider.LogsFolderPath);
            //XmlConfigurator.Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppLogger"/> class.
        /// </summary>
        public AppLogger()
            : base(typeof(AppLogger))
        {
        }

      

    }


}
