using System.Text.RegularExpressions;

namespace Comb.Prototype
{
    public static class RegexMatchExtentions
    {
        public static Link ToLink(this Match match, string domain, Link parent)
        {
            return new Link(match.Value, domain, parent);
        }
    }
}
