using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.Loggers;

namespace StaticFileTransform.dotless
{
    internal class LoggerAdapter : ILogger
    {
        private readonly StringBuilder log = new StringBuilder();
        private Int32 errorCounter = 0;

        public Int32 ErrorCount => errorCounter;
        public String CompilationLog => log.ToString();
        
        public void Log(LogLevel level, string message)
        {
            log.AppendFormat("{0}:{1}\n", level.ToString(), message);
            if (level >= LogLevel.Error)
            {
                errorCounter++;
            }
        }

        private String Stringify(object[] args) => String.Join(" ", args.Select(obj => obj.ToString()));

        public void Info(string message) => Log(LogLevel.Info, message);

        public void Info(string message, params object[] args) => Info(Stringify(args));

        public void Debug(string message) => Log(LogLevel.Debug, message);

        public void Debug(string message, params object[] args) => Debug(Stringify(args));

        public void Warn(string message) => Log(LogLevel.Warn, message);

        public void Warn(string message, params object[] args) => Warn(Stringify(args));

        public void Error(string message) => Log(LogLevel.Error, message);
        
        public void Error(string message, params object[] args) => Error(Stringify(args));
    }
}
