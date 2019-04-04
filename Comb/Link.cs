using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Comb
{
    public class Link
    {
        private LinkType? type;

        public string Value { get; }

        public Link Parent { get; }

        public Dictionary<string, Link> Descendents { get; }

        public ConcurrentDictionary<string, Link> Links { get; }

        public bool Combed { get; set; }

        public bool IsDescendent
        {
            get
            {
                if (Parent == null) return true;

                if (Value.StartsWith(Parent.Value, StringComparison.InvariantCultureIgnoreCase)
                    && !Value.Equals(Parent.Value, StringComparison.InvariantCultureIgnoreCase)) return true;

                return false;
            }
        }

        public LinkType Type
        {
            get
            {
                if (type == null)
                {
                    type = GetLinkType();
                }
                return type.Value;
            }
        }

        public Link(string value, string domain)
        {
            Value = PrefixDomain(Clean(value), domain);

            Links = new ConcurrentDictionary<string, Link>();
            Descendents = new Dictionary<string, Link>();
        }

        private Link(string value, string domain, ConcurrentDictionary<string, Link> links)
        {
            Value = PrefixDomain(Clean(value), domain);

            Links = links;
            Descendents = new Dictionary<string, Link>();
        }

        public Link(string value, string domain, Link parent) : this(value, domain, parent.Links)
        {
            Parent = parent;
        }

        public bool AddDescendent(string key, Link value)
        {
            if (!Links.TryAdd(key, value)) return false;

            return Descendents.TryAdd(key, value);
        }

        private LinkType GetLinkType()
        {
            foreach (var linkType in LinkTypes.GetValues())
            {
                if (Value.EndsWith(linkType.ToString(), StringComparison.InvariantCultureIgnoreCase)) return linkType;
            }

            return LinkType.URL;
        }

        private string Clean(string value)
        {
            const string regex = @"(href=|src=|"")";

            value = Regex.Replace(value, regex, string.Empty);

            if (value.EndsWith('/'))
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
