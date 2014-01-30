using HuntnPeck.Engine.Services;
using HuntnPeck.Forms;
using System;
using System.Windows.Forms;

namespace HuntnPeck
{
    public class HuntnPeckApplicationContext : ApplicationContext
    {
        public HuntnPeckApplicationContext()
        {
            var tray = new TrayListener();
            tray.HotKey = new Tuple<NativeMethods.KeyModifier, Keys>(NativeMethods.KeyModifier.Alt, Keys.OemSemicolon);

            var hintLabelService = new HintLabelService();
            var hintProviderService = new UiAutomationHintProviderService();

            tray.OnHotKeyActivated += () =>
            {
                var hintSession = hintProviderService.EnumHints();

                using (var overlay = new HintOverlay(hintLabelService, hintSession))
                {
                    if (overlay.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        var selectedHint = overlay.SelectedHint;
                        selectedHint.Activate();
                    }
                }
            };

            tray.Show();
        }
    }
}
