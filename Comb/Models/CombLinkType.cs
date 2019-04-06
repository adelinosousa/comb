using System;

namespace Comb
{
    [Flags]
    public enum CombLinkType
    {
        URL = 0,
        PNG = 1,
        JPG = 2,
        IMG = PNG | JPG,
        CSS = 4,
        HTML = 8
    }

    public static class LinkTypes
    {
        public static CombLinkType[] GetValues()
        {
            return new[] { CombLinkType.PNG, CombLinkType.JPG, CombLinkType.CSS, CombLinkType.HTML };
        }
    }
}
