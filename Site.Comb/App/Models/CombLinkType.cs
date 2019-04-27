using System;
using System.Linq;

namespace Site.Comb
{
    [Flags]
    public enum CombLinkType
    {
        URL = 1,
        // Image
        PNG = 2,
        JPG = 4,
        JPEG = 8,
        TIF = 16,
        TIFF = 32,
        BMP = 64,
        GIF = 128,
        // Video
        MP4 = 256,
        FLV = 512,
        MPG = 1024,
        MPEG = 2048,
        // Other
        CSS = 4096,
        JS = 8192,
        HTML = 16384,
        // Group
        IMG = PNG | JPG | JPEG | TIF | TIFF | BMP | GIF,
        VIDEO = MP4 | FLV | MPG | MPEG,
        OTHER = CSS | JS | HTML
    }

    public static class LinkTypes
    {
        public static CombLinkType[] GetValues()
        {
            return Enum.GetValues(typeof(CombLinkType))
                .Cast<CombLinkType>()
                .Except(new CombLinkType[] { CombLinkType.IMG })
                .ToArray();
        }
    }
}
