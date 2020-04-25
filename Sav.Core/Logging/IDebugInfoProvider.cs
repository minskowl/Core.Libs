using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savchin.Logging
{
    internal interface IDebugInfoProvider
    {
        /// <summary>
        /// Gets the debug information.
        /// </summary>
        /// <returns></returns>
        object GetDebugInfo();
    }
}
