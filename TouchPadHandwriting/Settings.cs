using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Color = System.Drawing.Color;
using Keys = System.Windows.Forms.Keys;
using Recognizer = Microsoft.Ink.Recognizer;

namespace TouchPadHandwriting
{
    internal class Settings
    {
        #region Static members

        internal static Settings LoadSettings()
        {
            Settings settings = new Settings();
            settings.InkRecognizer = RecognizersHelper.GetRecognizer(Properties.Settings.Default.InkRecognizerGuid);
            if (settings.InkRecognizer == null)
            {
                settings.InkRecognizer = RecognizersHelper.GetFirstRecognizer();
            }
            settings.RecognitionTime = Properties.Settings.Default.RecognitionTime;
            settings.AutoInsertionEnabled = Properties.Settings.Default.AutoInsertionEnabled;
            Keys toggleKey;
            int toggleKeyScancode;
            if (Enum.TryParse<Keys>(Properties.Settings.Default.ToggleKey, out toggleKey))
            {
                settings.ToggleKeyUseScancode = false;
                settings.ToggleKey = toggleKey;
            }
            else if (Properties.Settings.Default.ToggleKey.Substring(0, 2) == "sc" && int.TryParse(Properties.Settings.Default.ToggleKey.Substring(2), out toggleKeyScancode))
            {
                settings.ToggleKeyUseScancode = true;
                settings.ToggleKeyScancode = toggleKeyScancode;
            }
            else
            {
                settings.ToggleKeyUseScancode = false;
                settings.ToggleKey = Keys.LControlKey;
            }

            settings.StrokeColor = Properties.Settings.Default.StrokesColor;
            settings.StrokeWidth = Properties.Settings.Default.StrokeWidth;

            return settings;
        }

        internal static void SaveSettings(Settings settings)
        {
            if (settings.InkRecognizer != null)
            {
                Properties.Settings.Default.InkRecognizerGuid = settings.InkRecognizer.Id;
            }
            Properties.Settings.Default.RecognitionTime = settings.RecognitionTime;
            Properties.Settings.Default.AutoInsertionEnabled = settings.AutoInsertionEnabled;

            if (settings.ToggleKeyUseScancode)
            {
                Properties.Settings.Default.ToggleKey = string.Format("sc{0}", settings.ToggleKeyScancode);
            }
            else
            {
                Properties.Settings.Default.ToggleKey = Enum.GetName(typeof(Keys), settings.ToggleKey);
            }

            Properties.Settings.Default.StrokesColor = settings.StrokeColor;
            Properties.Settings.Default.StrokeWidth = settings.StrokeWidth;

            Properties.Settings.Default.Save();
        }

        #endregion

        #region Instance members

        //internal Guid InkRecognizerGuid { get; set; }
        internal Recognizer InkRecognizer { get; set; }

        internal ushort RecognitionTime { get; set; }

        internal bool AutoInsertionEnabled { get; set; }

        internal bool ToggleKeyUseScancode { get; set; }

        internal Keys ToggleKey { get; set; }

        internal int ToggleKeyScancode { get; set; }

        internal Color StrokeColor { get; set; }

        internal int StrokeWidth { get; set; }
        #endregion
    }
}
