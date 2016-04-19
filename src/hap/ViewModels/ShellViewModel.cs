using System;
using System.Windows.Forms;
using Caliburn.Micro;
using hap.Models;
using hap.NativeMethods;
using hap.Services.Interfaces;
using Application = System.Windows.Application;
using Screen = Caliburn.Micro.Screen;

namespace hap.ViewModels
{
    internal class ShellViewModel : Screen
    {
        private readonly IKeyListenerService _keyListener;
        private readonly Func<HintSession, OverlayViewModel> _overlayFactory;
        private readonly Func<HintSession, DebugOverlayViewModel> _debugOverlayFactory;
        private readonly IHintProviderService _hintProviderService;
        private readonly IDebugHintProviderService _debugHintProviderService;
        private readonly IWindowManager _windowManager;
        private readonly Func<OptionsViewModel> _optionsVmFactory;

        public ShellViewModel(
            Func<HintSession, OverlayViewModel> overlayFactory,
            Func<HintSession, DebugOverlayViewModel> debugOverlayFactory,
            IHintProviderService hintProviderService,
            IDebugHintProviderService debugHintProviderService,
            IWindowManager windowManager,
            Func<OptionsViewModel> optionsVmFactory,
            IKeyListenerService keyListener)
        {
            _overlayFactory = overlayFactory;
            _debugOverlayFactory = debugOverlayFactory;
            _keyListener = keyListener;
            _windowManager = windowManager;
            _hintProviderService = hintProviderService;
            _debugHintProviderService = debugHintProviderService;
            _optionsVmFactory = optionsVmFactory;

            _keyListener.HotKey = new HotKey
            {
                Keys = Keys.OemSemicolon,
                Modifier = KeyModifier.Alt
            };

#if DEBUG
            _keyListener.DebugHotKey = new HotKey
            {
                Keys = Keys.OemSemicolon,
                Modifier = KeyModifier.Alt | KeyModifier.Shift
            };
#endif

            _keyListener.OnHotKeyActivated += _keyListener_OnHotKeyActivated;
            _keyListener.OnDebugHotKeyActivated += _keyListener_OnDebugHotKeyActivated;
        }

        private void _keyListener_OnHotKeyActivated(object sender, EventArgs e)
        {
            var session = _hintProviderService.EnumHints();
            if (session != null)
            {
                var vm = _overlayFactory(session);
                _windowManager.ShowWindow(vm);
            }
        }

        private void _keyListener_OnDebugHotKeyActivated(object sender, EventArgs e)
        {
            var session = _debugHintProviderService.EnumDebugHints();
            if (session != null)
            {
                var vm = _debugOverlayFactory(session);
                _windowManager.ShowWindow(vm);
            }
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
