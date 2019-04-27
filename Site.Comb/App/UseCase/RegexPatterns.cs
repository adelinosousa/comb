namespace Site.Comb
{
    public class RegexPatterns
    {
        public const string Filter = @"(href=\s?""|src=\s?""|file:\s?""|"")";
        public const string Search = @"(href=\s?""|src=\s?""|file:\s?"")([^""#\+]*)""";
    }
}
