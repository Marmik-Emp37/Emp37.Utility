using System;

using UnityEngine;

namespace Emp37.Utility
{
        using static Shade;

        public static class ShadePalette
        {
                private static readonly (Shade shade, uint hex)[] source =
                {
                        (Amaranth, 0xE52B50),
                        (Amethyst, 0x9966CC),
                        (Apricot, 0xFBCEB1),
                        (Aquamarine, 0x7FFFD4),
                        (Azure, 0x007FFF),
                        (Beige, 0xF5F5DC),
                        (Black, 0x000000),
                        (Blond, 0xFAF0BE),
                        (Blue, 0x0000FF),
                        (Brown, 0x964B00),
                        (Cinnamon, 0xD2691E),
                        (Cherry, 0xDE3163),
                        (Chocolate, 0x7B3F00),
                        (Cobalt, 0x0047AB),
                        (Coffee, 0x6F4E37),
                        (Coral, 0xFF7F50),
                        (CottonCandy, 0xFFBCD9),
                        (Crimson, 0xDC143C),
                        (Cyan, 0x00FFFF),
                        (Dandelion, 0xF0E130),
                        (DarkGrey, 0x5A5A5A),
                        (EditorText, 0xB9B9B9),
                        (Eggplant, 0x614051),
                        (Emerald, 0x50C878),
                        (Forest, 0x228B22),
                        (Gold, 0xFFD700),
                        (Green, 0x00FF00),
                        (Grey, 0x7F7F7F),
                        (Heliotrope, 0xDF73FF),
                        (Honeydew, 0xF0FFF0),
                        (Icterine, 0xFCF75E),
                        (Khaki, 0xC3B091),
                        (Lavender, 0xE6E6FA),
                        (Lemon, 0xFFF700),
                        (Lime, 0xBFFF00),
                        (Linen, 0xFAF0E6),
                        (Magenta, 0xFF00FF),
                        (Maroon, 0x7F0000),
                        (Mint, 0x3EB489),
                        (MistyRose, 0xFFE4E1),
                        (Mustard, 0xFFDB58),
                        (Olive, 0x808000),
                        (Onyx, 0x0F0F0F),
                        (Orange, 0xFFA500),
                        (Pear, 0xD1E231),
                        (Pink, 0xFFC0CB),
                        (Pistachio, 0x93C572),
                        (Plum, 0xDDA0DD),
                        (Raspberry, 0xE30B5D),
                        (Red, 0xFF0000),
                        (RichBlack, 0x004040),
                        (Rose, 0xFF007F),
                        (Ruby, 0xE0115F),
                        (Salmon, 0xFA8072),
                        (SeaGreen, 0x005733),
                        (Sienna, 0x882D17),
                        (Silver, 0xC0C0C0),
                        (Skyblue, 0x87CEEB),
                        (Tangerine, 0xF28500),
                        (Teal, 0x007F7F),
                        (Tomato, 0xFF6347),
                        (Turquoise, 0x30D5C8),
                        (Vanilla, 0xF3E5AB),
                        (Violet, 0xEE82EE),
                        (White, 0xFFFFFF),
                        (WhiteSmoke, 0xF5F5EE),
                        (Wisteria, 0xC9A0DC),
                        (Yellow, 0xFFFF00)
                };
                private static readonly Color32[] palette;

                static ShadePalette()
                {
                        int length = Enum.GetValues(typeof(Shade)).Length;
                        palette = new Color32[length];

                        foreach ((Shade shade, uint hex) in source)
                        {
                                int shadeIndex = (int) shade;
                                byte r = (byte) (hex >> 16), g = (byte) (hex >> 8), b = (byte) hex;

                                palette[shadeIndex] = new Color32(r, g, b, byte.MaxValue);
                        }
                }

                /// <summary>
                /// Gets a random <see cref="Color32"/> from the palette.
                /// </summary>
                public static Color32 Random => palette[UnityEngine.Random.Range(0, palette.Length)];

                #region Extensions
                /// <summary>
                /// Converts the specified <see cref="Shade"/> to a <see cref="Color32"/>.
                /// </summary>
                public static Color32 ToColor32(this Shade shade, byte alpha = byte.MaxValue)
                {
                        Color32 color = palette[(int) shade];
                        color.a = alpha;
                        return color;
                }

                /// <summary>
                /// Converts the specified <see cref="Shade"/> to a <see cref="Color"/>.
                /// </summary>
                public static Color ToColor(this Shade shade, float alpha = 1F)
                {
                        Color color = (Color) ToColor32(shade);
                        color.a = Mathf.Clamp01(alpha);
                        return color;
                }

                /// <summary>
                /// Returns the six-digit hexadecimal RGB string for the specified <see cref="Shade"/>.
                /// </summary>
                /// <returns>An uppercase six-character hex string with no leading '#' (e.g. <c>FF0000</c>).</returns>
                public static string ToHexRGB(this Shade shade) => $"{source[(int) shade].hex:X6}";

                /// <summary>
                /// Wraps <paramref name="text"/> in a Unity rich-text color tag using the specified <see cref="Shade"/>.
                /// </summary>
                /// <param name="shade">The shade used for the text colour.</param>
                /// <param name="text">The text to colourize.</param>
                /// <returns>A rich-text string such as <c>&lt;color=#FF0000&gt;text&lt;/color&gt;</c>.</returns>
                public static string ToRichText(this Shade shade, string text) => $"<color=#{ToHexRGB(shade)}>{text}</color>";
                #endregion
        }
}