using Autofac;
using hap.Engine.Services;
using hap.Engine.Services.Interfaces;

namespace hap.Engine
{
    public class EngineAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Hint labeller
            builder.RegisterType<HintLabelService>()
                   .As<IHintLabelService>()
                   .SingleInstance();

            // Hint provider, there's only one for now otherwise we could expose two modules
            builder.RegisterType<UiAutomationHintProviderService>()
                   .As<IHintProviderService>()
                   .SingleInstance();

            // Hint "Factory"
            builder.RegisterType<UiAutomationHintFactory>()
                   .As<IUiAutomationHintFactory>()
                   .SingleInstance();


        }
    }
}
