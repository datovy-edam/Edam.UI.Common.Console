using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml.Media;

// -----------------------------------------------------------------------------

namespace Edam.UI.Common.Resources;


public class Colour
{
    private static Windows.UI.ViewManagement.UISettings m_UISettings;

    public static Color Default;
    public static Color Accent;

    /// <summary>
    /// Given a HEX color string...
    /// </summary>
    /// <param name="hex"></param>
    /// <returns>returns solid brush</returns>
    public static SolidColorBrush BrushFromHex(string hex)
    {
        hex = hex.Replace("#", string.Empty);

        byte a, r, g, b;
        if (hex.Length == 6)
        {
            a = 0;
            r = Convert.HexTextToByte(hex.Substring(0, 2));
            g = Convert.HexTextToByte(hex.Substring(2, 2));
            b = Convert.HexTextToByte(hex.Substring(4, 2));
        }
        else if (hex.Length == 8)
        {
            a = Convert.HexTextToByte(hex.Substring(0, 2));
            r = Convert.HexTextToByte(hex.Substring(2, 2));
            g = Convert.HexTextToByte(hex.Substring(4, 2));
            b = Convert.HexTextToByte(hex.Substring(6, 2));
        }
        else
            throw new Exception("Colour::BrushFromHex: Bad HEX Color");

        return new SolidColorBrush(Color.FromArgb(a, r, g, b));
    }

    public static Color ColorFromHex(string hex)
    {
        return BrushFromHex(hex).Color;
    }

    public static Brush ToBrush(string hex)
    {
        return BrushFromHex(hex);
    }

    public Colour()
    {
        m_UISettings = new UISettings();
        Accent = m_UISettings.UIElementColor(UIElementType.AccentColor);
        Default = m_UISettings.UIElementColor(UIElementType.Background);
    }
}
