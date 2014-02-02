using Autofac;
using HuntAndPeck.Engine.Services;
using HuntAndPeck.Engine.Services.Interfaces;

namespace HuntAndPeck.Engine
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
