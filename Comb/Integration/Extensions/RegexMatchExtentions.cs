using System.Text.RegularExpressions;

namespace Site.Comb
{
    public static class RegexMatchExtentions
    {
        public static CombLink ToLink(this Match match, string domain, CombLink parent)
        {
            return new CombLink(match.Value, domain, parent);
        }
    }
}
