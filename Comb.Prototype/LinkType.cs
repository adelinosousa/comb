using System;

namespace Comb.Prototype
{
    [Flags]
    public enum LinkType
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
        public static LinkType[] GetValues()
        {
            return new[] { LinkType.PNG, LinkType.JPG, LinkType.CSS, LinkType.HTML };
        }
    }
}
