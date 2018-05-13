using Savchin.Logging;

namespace Savchin.Services.Logging
{
    public interface INamedLogger: ILogger
    {
    }

    public class NamedLogger : LoggerBase, INamedLogger
    {
        public NamedLogger(string name) : base(name)
        {

        }
    }
}
