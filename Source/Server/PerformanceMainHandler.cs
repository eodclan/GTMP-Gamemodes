using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using GrandTheftMultiplayer.Server.Models;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FactionLife.Server.Performance
{
public class Main : Script
    {
        public Main()
        {
            this.API.onResourceStart += this.OnResourceStart;
        }
        void OnResourceStart()
        {
            Console.WriteLine("x");
            Task.Delay(1000)
                .ContinueWith(t =>
                {
                    const int iterations = 1000;
                    var a = new A();
                    var directCall = Measure(() => a.ExportedMethod(), iterations);
                    var exportCall = Measure(() => this.API.exported._perf.ExportedMethod(), iterations);
                    dynamic export = this.API.exported._perf.ExportedMethod;
                    var memorizedExportCall = Measure(() => export(), iterations);
                    var intrusiveExports = new IntrusiveExports(this.API);
                    intrusiveExports.AddExportsForResource(this.API.getThisResource());
                    var intrusiveExportCall = Measure(() => this.API.exported._perf.ExportedMethod(), iterations);
                    API.consoleOutput("total calls {0}", A.ExportedMethodInvocations);
                    API.consoleOutput("{0,20} {1,10}ms", "direct", directCall);
                    API.consoleOutput("{0,20} {1,10}ms", "export", exportCall);
                    API.consoleOutput("{0,20} {1,10}ms", "mem export", memorizedExportCall);
                    API.consoleOutput("{0,20} {1,10}ms", "intrusive export", intrusiveExportCall);
                });
        }
        static long Measure(Action what, int iterations)
        {
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < iterations; i++)
            {
                what();
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}	