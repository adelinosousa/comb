﻿using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Site.Comb
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
                type = CombLinkType.URL;

                var multiple = false;
                foreach (var linkType in LinkTypes.GetValues())
                {
                    if (Value.EndsWith(linkType.ToString(), StringComparison.InvariantCultureIgnoreCase)
                        || Value.ToLowerInvariant().Contains($".{linkType.ToString().ToLowerInvariant()}"))
                    {
                        if (multiple)
                        {
                            type |= linkType;
                        }
                        else
                        {
                            type = linkType;
                        }
                    }
                }
            }

            return type.Value;
        }

        private string Clean(string value)
        {
            value = Regex.Replace(value, RegexPatterns.Filter, string.Empty, RegexOptions.IgnoreCase);

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
                return $"{new Uri(domain).Scheme}:{value}";
            }

            if (!value.StartsWith(Uri.UriSchemeHttp) && !value.StartsWith(domain, StringComparison.InvariantCultureIgnoreCase))
            {
                if (!value.StartsWith("/")) value = $"/{value}";

                return $"{domain}{value}";
            }

            return value;
        }
    }
}
