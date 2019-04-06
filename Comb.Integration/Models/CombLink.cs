using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Comb.Integration
{
    public class CombLink : ICombLink
    {
        private readonly Dictionary<string, CombLink> children;
        private readonly CombLink parent;
        private CombLinkType? type;

        public ConcurrentDictionary<string, CombLink> Links { get; }

        public string Value { get; }

        public ICombLink[] All
        {
            get
            {
                return Links.Values.OfType<ICombLink>().ToArray();
            }
        }

        public ICombLink[] Descendents
        {
            get
            {
                return children.Values.Where(x => x.IsDescendent).OfType<ICombLink>().ToArray();
            }
        }

        public bool Combed { get; set; }

        public bool IsDescendent
        {
            get
            {
                if (parent == null) return true;

                if (Value.StartsWith(parent.Value, StringComparison.InvariantCultureIgnoreCase)
                    && !Value.Equals(parent.Value, StringComparison.InvariantCultureIgnoreCase)) return true;

                return false;
            }
        }

        public CombLinkType Type
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

        public CombLink(string value, string domain)
        {
            Value = PrefixDomain(Clean(value), domain);

            Links = new ConcurrentDictionary<string, CombLink>();
            children = new Dictionary<string, CombLink>();
        }

        private CombLink(string value, string domain, ConcurrentDictionary<string, CombLink> links)
        {
            Value = PrefixDomain(Clean(value), domain);

            Links = links;
            children = new Dictionary<string, CombLink>();
        }

        public CombLink(string value, string domain, CombLink parent) : this(value, domain, parent.Links)
        {
            this.parent = parent;
        }

        public bool AddDescendent(string key, CombLink value)
        {
            if (!Links.TryAdd(key, value)) return false;

            if (children.ContainsKey(key)) return false;
            
            // Core 2.2 Descendents.TryAdd(key, value);
            children.Add(key, value);
            return true;
        }

        private CombLinkType GetLinkType()
        {
            foreach (var linkType in LinkTypes.GetValues())
            {
                if (Value.EndsWith(linkType.ToString(), StringComparison.InvariantCultureIgnoreCase)) return linkType;
            }

            return CombLinkType.URL;
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
