using System;

namespace Comb
{
    [Flags]
    public enum CombLinkType
    {
        URL = 1,
        PNG = 2,
        JPG = 4,
        CSS = 8,
        JS = 16,
        MP4 = 32,
        HTML = 64,
        IMG = PNG | JPG
    }

    public static class LinkTypes
    {
        public static CombLinkType[] GetValues()
        {
            return new[] { CombLinkType.PNG, CombLinkType.JPG, CombLinkType.CSS, CombLinkType.HTML };
        }
    }
}
