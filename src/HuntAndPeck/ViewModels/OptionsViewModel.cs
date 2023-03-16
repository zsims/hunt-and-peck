using HuntAndPeck.Properties;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace HuntAndPeck.ViewModels
{
    internal class OptionsViewModel : INotifyPropertyChanged
    {
        public OptionsViewModel()
        {
            DisplayName = "Options";
            FontSize = Settings.Default.FontSize;
            FontBackroundColor = Settings.Default.FontBackroundColor;
            FontColor= Settings.Default.FontColor;
            //Settings.Default.PropertyChanged += OnSettingsPropertyChanged;
        }

        public string DisplayName { get; set; }

        private string _fontSize;
        public string FontSize
        // Assign the font size value to a variable and update it every time user 
        // changes the option in tray menu
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    OnPropertyChanged("FontSize");
                    Settings.Default.FontSize = value;
                    Settings.Default.Save();
                }
            }
        }

        private Color _fontBackroundColor;
        public Color FontBackroundColor
        {
            get { return _fontBackroundColor; }
            set
            {
                if (_fontBackroundColor != value)
                {
                    _fontBackroundColor = value;
                    OnPropertyChanged("FontBackroundColor");
                    Settings.Default.FontBackroundColor = value;
                    Settings.Default.Save();
                }
            }
        }

        private Color _fontColor;
        public Color FontColor
        {
            get { return _fontColor; }
            set
            {
                if (_fontColor != value)
                {
                    _fontColor = value;
                    OnPropertyChanged("FontColor");
                    Settings.Default.FontColor = value;
                    Settings.Default.Save();
                }
            }
        }
            }
        }

        //private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "FontSize")
        //    {
        //        FontSize = Settings.Default.FontSize;
        //    }
        //    if (e.PropertyName == "FontBackroundColor")
        //    { 
        //       FontBackroundColor = Settings.Default.FontBackroundColor;
        //    }
        //    if (e.PropertyName == "FontColor")
        //    {
        //        FontColor = Settings.Default.FontColor;
        //    }
        //    if (e.PropertyName == "KbdShortWin")
        //    {
        //        KbdShortWin = Settings.Default.KbdShortWin;
        //    }
        //    if (e.PropertyName == "KbdShorTray")
        //    {
        //        KbdShortTray = Settings.Default.KbdShortTray;
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}