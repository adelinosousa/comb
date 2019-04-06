using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Comb.Integration
{
    public class Link
    {
        private bool combed;
        private CombLinkType? type;

        protected ConcurrentDictionary<string, ICombLink> Links { get; }

        public string Value { get; private set; }

        public Link(string value, string domain, ConcurrentDictionary<string, ICombLink> links)
        {
            Links = links;
            Value = PrefixDomain(Clean(value), domain);
        }

        public bool Combed
        {
            get
            {
                Links.TryGetValue(Value, out ICombLink existingLink);
                return existingLink != null && combed;
            }
        }

        public void SetCombed()
        {
            combed = true;
        }

        protected CombLinkType GetLinkType()
        {
            if (type == null)
            {
                foreach (var linkType in LinkTypes.GetValues())
                {
                    if (Value.EndsWith(linkType.ToString(), StringComparison.InvariantCultureIgnoreCase)) type = linkType;
                }

                type = CombLinkType.URL;
            }

            return type.Value;
        }

        private string Clean(string value)
        {
            const string regex = @"(href=|src=|"")";

            value = Regex.Replace(value, regex, string.Empty);

            if (value.EndsWith("/"))
            {
                value = value.Substring(0, value.Length - 1);
            }

            return value;
        }

        private string PrefixDomain(string value, string domain)
        {
            if (value.StartsWith("//"))
            {
                return $"{Uri.UriSchemeHttp}:{value}";
            }

            if (!value.StartsWith(Uri.UriSchemeHttp) && !value.StartsWith(domain, StringComparison.InvariantCultureIgnoreCase))
            {
                return $"{domain}{value}";
            }

            return value;
        }
    }
}
