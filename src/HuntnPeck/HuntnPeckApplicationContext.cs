using HuntnPeck.Engine.Renderer;
using HuntnPeck.Engine.Services;
using HuntnPeck.Forms;
using System;
using System.Linq;
using System.Windows.Forms;

namespace HuntnPeck
{
    public class HuntnPeckApplicationContext : ApplicationContext
    {
        private readonly HintRenderer _hintRenderer;

        public HuntnPeckApplicationContext()
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

            base.Dispose();
        }
    }
}
