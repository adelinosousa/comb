using System.Text.RegularExpressions;

namespace Comb.Integration
{
    public static class RegexMatchExtentions
    {
        public static CombLink ToLink(this Match match, string domain, CombLink parent)
        {
            return new CombLink(match.Value, domain, parent);
        }
    }
}
