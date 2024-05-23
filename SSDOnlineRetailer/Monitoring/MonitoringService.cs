using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Core;
using System.Diagnostics;
using System.Reflection;
namespace Monitoring
   
{
    public static  class MonitoringService
    {

        public static ILogger Log => Serilog.Log.Logger;

        static MonitoringService()
        {

            //Serilog stuff
            Serilog.Log.Logger = new LoggerConfiguration()

                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            Log.Debug("Started Logger in MonitorService");
            //Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine($"Serilog: {msg}"));
        }
        


    }
}