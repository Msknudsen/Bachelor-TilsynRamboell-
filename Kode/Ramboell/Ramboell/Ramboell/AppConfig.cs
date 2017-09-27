using System;
using System.Collections.Generic;
using System.Text;
using Autofac;


namespace Ramboell
{
    public class AppConfig
    {
        public IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }
        protected virtual void RegisterDependencies(ContainerBuilder cb)
        {
        }
    }
}
