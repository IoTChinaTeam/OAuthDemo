using System;
using System.Diagnostics;
using Microsoft.Owin.Logging;

namespace OAuthDemo
{
    public class OwinTraceLogger : ILogger
    {
        private string _name;

        public OwinTraceLogger(string name)
        {
            _name = name;
        }

        public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            Debug.WriteLine($"[{_name}]: {eventType} {formatter(state, exception)}");

            return true;
        }
    }
}