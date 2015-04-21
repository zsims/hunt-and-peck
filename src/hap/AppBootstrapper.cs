using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using hap.Models;
using hap.Services;
using hap.Services.Interfaces;
using hap.ViewModels;

namespace hap
{
    public class AppBootstrapper : BootstrapperBase
    {
        private IContainer _container;
        private SingleLaunchMutex _singleLaunchMutex = new SingleLaunchMutex();

        public AppBootstrapper()
        {
            Initialize();
        }

        #region Startup

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Contains("/hint"))
            {
                // support headless mode
                var providerService = _container.Resolve<IHintProviderService>();
                var windowManager = _container.Resolve<IWindowManager>();
                var overlayFactory = _container.Resolve<Func<HintSession, OverlayViewModel>>();

                var session = providerService.EnumHints();
                var vm = overlayFactory(session);
                windowManager.ShowWindow(vm);
            }
            else
            {
                // Prevent multiple startup in non-headless mode
                if (_singleLaunchMutex.AlreadyRunning)
                {
                    Application.Current.Shutdown();
                    return;
                }

                DisplayRootViewFor<ShellViewModel>();
            }
        }

        #endregion

        #region Shutdown

        protected override void OnExit(object sender, EventArgs e)
        {
            _container.Dispose();
            _container = null;

            _singleLaunchMutex.Dispose();
            _singleLaunchMutex = null;

            base.OnExit(sender, e);
        }

        #endregion

        #region IoC Setup

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            RegisterViewModels(builder);
            RegisterServices(builder);
            RegisterCaliburnServices(builder);

            _container = builder.Build();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<SessionCache>()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterType<KeyListenerService>()
                   .AsImplementedInterfaces()
                   .SingleInstance()
                   .AutoActivate();

            builder.RegisterType<AheadOfTimeSessionService>()
                   .AsImplementedInterfaces()
                   .SingleInstance()
                   .AutoActivate();

            // Hint labeller
            builder.RegisterType<HintLabelService>()
                   .As<IHintLabelService>()
                   .SingleInstance();

            // Hint provider
            builder.RegisterType<UiAutomationHintProviderService>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }

        /// <summary>
        /// Registers the caliburn services
        /// </summary>
        /// <param name="builder"></param>
        private void RegisterCaliburnServices(ContainerBuilder builder)
        {
            // Window Manager
            builder.RegisterType<WindowManager>()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            // Event Aggregator
            builder.RegisterType<EventAggregator>()
                   .AsImplementedInterfaces()
                   .AutoActivate()
                   .SingleInstance();
        }

        /// <summary>
        /// Registers all the view models by convention
        /// </summary>
        private void RegisterViewModels(ContainerBuilder builder)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            // register all view models by namespace convention
            builder.RegisterAssemblyTypes(currentAssembly)
                   .InNamespaceOf<OptionsViewModel>()
                   .AsImplementedInterfaces()
                   .AsSelf()
                   .InstancePerDependency();
        }

        #endregion

        #region IoC Integration

        /// <summary>
        /// Requests a specific instance
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.Resolve(service);
            }

            return _container.ResolveKeyed(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }

        #endregion
    }
}
