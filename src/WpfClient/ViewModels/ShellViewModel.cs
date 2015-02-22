using Caliburn.Micro;
using hap.Engine.Hints;
using hap.Engine.Services.Interfaces;
using hap.NativeMethods;
using hap.WpfClient.Services;
using System;
using System.Windows;

namespace hap.WpfClient.ViewModels
{
    internal class ShellViewModel : Screen
    {
        private readonly IKeyListenerService _keyListener;
        private readonly Func<HintSession, OverlayViewModel> _overlayFactory;
        private readonly IHintProviderService _hintProviderService;
        private readonly IWindowManager _windowManager;
        private readonly Func<OptionsViewModel> _optionsVmFactory;

        public ShellViewModel(
            Func<HintSession, OverlayViewModel> overlayFactory,
            IHintProviderService hintProviderService,
            IWindowManager windowManager,
            Func<OptionsViewModel> optionsVmFactory,
            IKeyListenerService keyListener)
        {
            _overlayFactory = overlayFactory;
            _keyListener = keyListener;
            _windowManager = windowManager;
            _hintProviderService = hintProviderService;
            _optionsVmFactory = optionsVmFactory;

            _keyListener.HotKey = new HotKey
            {
                Keys = System.Windows.Forms.Keys.OemSemicolon,
                Modifier = KeyModifier.Alt
            };

            _keyListener.OnHotKeyActivated += _keyListener_OnHotKeyActivated;
        }

        private void _keyListener_OnHotKeyActivated(object sender, System.EventArgs e)
        {
            var session = _hintProviderService.EnumHints();
            var vm = _overlayFactory(session);
            _windowManager.ShowWindow(vm);
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        public bool CanExit()
        {
            return true;
        }

        public void Options()
        {
            var vm = _optionsVmFactory();
            _windowManager.ShowWindow(vm);
        }

        public bool CanOptions()
        {
            return true;
        }
    }
}
