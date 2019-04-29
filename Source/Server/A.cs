using System;

namespace FactionLife.Server.Performance
{
    internal class A
    {
        public A()
        {
        }

        public static int ExportedMethodInvocations { get; internal set; }

        internal void ExportedMethod()
        {
            throw new NotImplementedException();
        }
    }
}