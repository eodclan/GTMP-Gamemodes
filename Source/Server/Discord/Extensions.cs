﻿using System.Drawing;
using System.Globalization;

namespace FactionLife.Server.Discord
{
    public static class Extensions
    {
        public static int ToRgb(this Color color)
        {
            return int.Parse(ColorTranslator.ToHtml(Color.FromArgb(color.ToArgb())).Replace("#", ""), NumberStyles.HexNumber);
        }
    }
}
