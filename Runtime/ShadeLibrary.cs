using System.Collections.Generic;

using UnityEngine;

namespace Emp37.Utility
{
      using static Shade;


      public readonly struct ShadeLibrary
      {
            private static readonly Dictionary<Shade, Color32> library = new()
            {
                  {
                        Amaranth,
                        new(229, 043, 080, 255)
                  },
                  {
                        Amethyst,
                        new(153, 102, 204, 255)
                  },
                  {
                        Apricot,
                        new(251, 206, 177, 255)
                  },
                  {
                        Aquamarine,
                        new(127, 255, 212, 255)
                  },
                  {
                        Azure,
                        new(000, 127, 255, 255)
                  },
                  {
                        Beige,
                        new(245, 245, 220, 255)
                  },
                  {
                        Black,
                        new(000, 000, 000, 255)
                  },
                  {
                        Blond,
                        new(250, 240, 190, 255)
                  },
                  {
                        Blue,
                        new(000, 000, 255, 255)
                  },
                  {
                        Brown,
                        new(150, 075, 000, 255)
                  },
                  {
                        Cinnamon,
                        new(210, 105, 030, 255)
                  },
                  {
                        Cherry,
                        new(222, 049, 099, 255)
                  },
                  {
                        Chocolate,
                        new(123, 063, 000, 255)
                  },
                  {
                        Cobalt,
                        new(000, 071, 171, 255)
                  },
                  {
                        Coffee,
                        new(111, 078, 055, 255)
                  },
                  {
                        Coral,
                        new(255, 127, 080, 255)
                  },
                  {
                        CottonCandy,
                        new(255, 188, 217, 255)
                  },
                  {
                        Crimson,
                        new(220, 020, 060, 255)
                  },
                  {
                        Cyan,
                        new(000, 255, 255, 255)
                  },
                  {
                        Dandelion,
                        new(240, 225, 048, 255)
                  },
                  {
                        DarkGrey,
                        new(090, 090, 090, 255)
                  },
                  {
                        EditorText,
                        new(185, 185, 185, 255)
                  },
                  {
                        Eggplant,
                        new(097, 064, 081, 255)
                  },
                  {
                        Emerald,
                        new(080, 200, 120, 255)
                  },
                  {
                        Forest,
                        new(034, 139, 034, 255)
                  },
                  {
                        Gold,
                        new(255, 215, 000, 255)
                  },
                  {
                        Green,
                        new(000, 255, 000, 255)
                  },
                  {
                        Grey,
                        new(127, 127, 127, 255)
                  },
                  {
                        Heliotrope,
                        new(223, 115, 255, 255)
                  },
                  {
                        Honeydew,
                        new(240, 255, 240, 255)
                  },
                  {
                        Icterine,
                        new(252, 247, 094, 255)
                  },
                  {
                        Khaki,
                        new(195, 176, 145, 255)
                  },
                  {
                        Lavender,
                        new(230, 230, 250, 255)
                  },
                  {
                        Lemon,
                        new(255, 247, 000, 255)
                  },
                  {
                        Lime,
                        new(191, 255, 000, 255)
                  },
                  {
                        Linen,
                        new(250, 240, 230, 255)
                  },
                  {
                        Magenta,
                        new(255, 000, 255, 255)
                  },
                  {
                        Maroon,
                        new(127, 000, 000, 255)
                  },
                  {
                        Mint,
                        new(062, 180, 137, 255)
                  },
                  {
                        MistyRose,
                        new(255, 228, 225, 255)
                  },
                  {
                        Mustard,
                        new(255, 219, 088, 255)
                  },
                  {
                        Olive,
                        new(128, 128, 000, 255)
                  },
                  {
                        Onyx,
                        new(015, 015, 015, 255)
                  },
                  {
                        Orange,
                        new(255, 165, 000, 255)
                  },
                  {
                        Pear,
                        new(209, 226, 049, 255)
                  },
                  {
                        Pink,
                        new(255, 192, 203, 255)
                  },
                  {
                        Pistachio,
                        new(147, 197, 114, 255)
                  },
                  {
                        Plum,
                        new(221, 160, 221, 255)
                  },
                  {
                        Raspberry,
                        new(227, 011, 093, 255)
                  },
                  {
                        Red,
                        new(255, 000, 000, 255)
                  },
                  {
                        RichBlack,
                        new(000, 064, 064, 255)
                  },
                  {
                        Rose,
                        new(255, 000, 127, 255)
                  },
                  {
                        Ruby,
                        new(224, 017, 095, 255)
                  },
                  {
                        Salmon,
                        new(250, 128, 114, 255)
                  },
                  {
                        SeaGreen,
                        new(000, 087, 051, 255)
                  },
                  {
                        Sienna,
                        new(136, 045, 023, 255)
                  },
                  {
                        Silver,
                        new(192, 192, 192, 255)
                  },
                  {
                        Skyblue,
                        new(135, 206, 235, 255)
                  },
                  {
                        Tangerine,
                        new(242, 133, 000, 255)
                  },
                  {
                        Teal,
                        new(000, 127, 127, 255)
                  },
                  {
                        Tomato,
                        new(255, 099, 071, 255)
                  },
                  {
                        Turquoise,
                        new(048, 213, 200, 255)
                  },
                  {
                        Vanilla,
                        new(243, 229, 171, 255)
                  },
                  {
                        Violet,
                        new(238, 130, 238, 255)
                  },
                  {
                        White,
                        new(255, 255, 255, 255)
                  },
                  {
                        WhiteSmoke,
                        new(245, 245, 238, 255)
                  },
                  {
                        Wisteria,
                        new(201, 160, 220, 255)
                  },
                  {
                        Yellow,
                        new(255, 255, 000, 255)
                  }
            };
            private static readonly int shadesLength = System.Enum.GetValues(typeof(Shade)).Length;

            public static Color32 PickRandom => Pick((Shade) Random.Range(0, shadesLength));
            public static Color32 Pick(Shade shade) => library[shade];
      }
}