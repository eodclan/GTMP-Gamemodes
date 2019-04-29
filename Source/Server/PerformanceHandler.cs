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

namespace FactionLife.Server.Performance
{
    public class IntrusiveExports
    {
        API _api;
        public IntrusiveExports(API api)
        {
            this._api = api;
        }
        public void AddExportsForResource(string resource)
        {
            var gtmpServerMainType = typeof(API).Assembly.GetTypes().FirstOrDefault(p => p.Name == "Program");
            var resourceType = typeof(API).Assembly.GetTypes().FirstOrDefault(p => p.Name == "Resource");
            var gameServerInstanceMember = gtmpServerMainType.GetProperties(BindingFlags.Static | BindingFlags.NonPublic).FirstOrDefault(p => p.Name == "ServerInstance");
            var runningResourcesMember = gameServerInstanceMember.PropertyType.GetFields(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(p => p.Name == "RunningResources");
            var resourceInfoMember = resourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(p => p.Name == "Info");
            var directoryNameMember = resourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(p => p.Name == "DirectoryName");
            var enginesMember = resourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(p => p.Name == "Engines");
            var getAssemblyMember = enginesMember.PropertyType.GetGenericArguments().First().GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(p => p.Name == "GetAssembly");
            var gameServerInstance = gameServerInstanceMember.GetValue(null);
            var runningResources = runningResourcesMember.GetValue(gameServerInstance) as IEnumerable;
            var methodExportNameMember = typeof(MethodExport).GetProperties().FirstOrDefault(p => p.Name == "Name");
            var exported = this._api.exported as IDictionary<String, System.Object>;
            foreach (var runningResource in runningResources)
            {
                var resourceInfo = resourceInfoMember.GetValue(runningResource) as ResourceInfo;
                var directoryName = directoryNameMember.GetValue(runningResource) as string;
                if (string.CompareOrdinal(directoryName, resource) != 0)
                {
                    continue;
                }
                var engines = enginesMember.GetValue(runningResource) as IEnumerable;
                foreach (var export in resourceInfo.ExportedFunctions)
                {
                    foreach (var engine in engines)
                    {
                        var script = getAssemblyMember.GetValue(engine);
                        var scriptType = script.GetType();
                        var typeName = scriptType.Name;
                        if (string.CompareOrdinal(export.Path, typeName) == 0)
                        {
                            var name = methodExportNameMember.GetValue(export) as string;
                            if (!string.IsNullOrEmpty(name))
                            {
                                var method = scriptType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(p => p.Name == name);
                                var instanceExpression = LambdaExpression.Constant(script);
                                var callExpression = LambdaExpression.Call(instanceExpression, method);
                                var lambda = Expression.Lambda(callExpression, null);
                                var compiledDelegate = lambda.Compile();
                                var exportedType = exported[directoryName] as IDictionary<String, System.Object>;
                                exportedType[name] = compiledDelegate;
                            }
                        }
                    }
                }
            }
        }
    }
}	