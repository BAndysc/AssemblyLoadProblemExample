using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Common;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using Unity.Lifetime;
using Unity.RegistrationByConvention;

namespace TestMultiProjectApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App() : base()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        protected override Window CreateShell()
        {
            var splash = Container.Resolve<MainWindow>();
            
            return splash;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
        
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            List<Assembly> allAssemblies = new List<Assembly>();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (string dll in Directory.GetFiles(path + "/Modules/", "*.dll"))
                allAssemblies.Add(Assembly.LoadFile(dll));
            
            AutoRegisterClasses(allAssemblies);

            var modules = AllClasses.FromAssemblies(allAssemblies).Where(t => t.GetInterfaces().Contains(typeof(IModule)));

            foreach (var module in modules)
            {
                moduleCatalog.AddModule(
                    new ModuleInfo()
                    {
                        ModuleName = module.Name,
                        ModuleType = module.AssemblyQualifiedName,
                        Ref = "file://" + module.Assembly.Location
                    });
            }
        }

        private void AutoRegisterClasses(List<Assembly> allAssemblies)
        {
            var defaultRegisters = AllClasses.FromAssemblies(allAssemblies).Where(t => t.IsDefined(typeof(AutoRegisterAttribute), true));
            
            foreach (var register in defaultRegisters)
            {
                if (register.IsAbstract)
                    continue;
                
                foreach (var interface_ in register.GetInterfaces().Union(new[] { register }))
                {                    
                    Container.GetContainer().RegisterType(interface_, register, null, new TransientLifetimeManager());
                    Console.WriteLine($"{interface_} as {register}");
                }
            }
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override void OnInitialized()
        {
        }
    }
}
