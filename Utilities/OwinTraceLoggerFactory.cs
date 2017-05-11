using Microsoft.Owin.Logging;

namespace OAuthDemo
{
    public class OwinTraceLoggerFactory : ILoggerFactory
    {
        public ILogger Create(string name)
        {
            return new OwinTraceLogger(name);
        }
    }
}