using HuntAndPeck.UserInterface.Renderer;
using HuntAndPeck.Engine.Services;
using System;
using System.Linq;
using System.Windows.Forms;
using HuntAndPeck.UserInterface.Forms;

namespace HuntAndPeck.UserInterface
{
    public class HuntAndPeckApplicationContext : ApplicationContext
    {
        private readonly HintRenderer _hintRenderer;

        public HuntAndPeckApplicationContext()
        {
            var tray = new TrayListener();
            tray.HotKey = new Tuple<NativeMethods.KeyModifier, Keys>(NativeMethods.KeyModifier.Alt, Keys.OemSemicolon);

            var hintLabelService = new HintLabelService();
            var hintProviderService = new UiAutomationHintProviderService();
            _hintRenderer = new HintRenderer();

            tray.OnHotKeyActivated += () =>
            {
                var hintSession = hintProviderService.EnumHints();

                using (var overlay = new HintOverlay(hintLabelService, _hintRenderer, hintSession))
                {
                    if (overlay.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var selectedHint = overlay.SelectedHint;

                        // TODO: Make this "capability" focused, rather than hint
                        var frist = selectedHint.Capabilities.FirstOrDefault();
                        if (frist != null)
                        {
                            frist.Activate();
                        }
                    }
                }
            };

            tray.Show();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hintRenderer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
