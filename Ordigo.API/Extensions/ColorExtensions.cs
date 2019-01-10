using System.Drawing;

namespace Ordigo.API.Extensions
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static Color ToColor(this string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }
    }
}
