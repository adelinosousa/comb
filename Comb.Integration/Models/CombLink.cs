using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Comb.Integration
{
    public class CombLink : Link, ICombLink
    {
        private readonly CombLink parent;

        private Dictionary<string, CombLink> Children { get; }

        public ICombLink[] Descendents
        {
            get
            {
                return Children.Values.Where(x => x.IsDescendent).OfType<ICombLink>().ToArray();
            }
        }

        public bool IsDescendent
        {
            get
            {
                if (parent == null) return true;

                if (Value.StartsWith(parent.Value, StringComparison.InvariantCultureIgnoreCase)
                    && !Value.Equals(parent.Value, StringComparison.InvariantCultureIgnoreCase)
                    && Type == CombLinkType.URL)
                {
                    return true;
                }

                return false;
            }
        }

        public CombLinkType Type
        {
            get
            {
                return GetLinkType();
            }
        }

        public CombLink(string value, string domain) : base(value, domain, new ConcurrentDictionary<string, ICombLink>())
        {
            Children = new Dictionary<string, CombLink>();
        }

        public CombLink(string value, string domain, CombLink parent) : base(value, domain, parent.Links)
        {
            this.parent = parent;
            Children = new Dictionary<string, CombLink>();
        }

        public bool AddDescendent(string key, CombLink value)
        {
            if (!Links.TryAdd(key, value)) return false;

            if (Children.ContainsKey(key)) return false;

            // Core 2.2 Descendents.TryAdd(key, value);
            Children.Add(key, value);
            return true;
        }

        public ICombLink[] All()
        {
            return Links.Values.OfType<ICombLink>().ToArray();
        }

        public ICombLink[] All(CombLinkType linkType)
        {
            return Links.Values.Where(x => linkType.HasFlag(x.Type)).OfType<ICombLink>().ToArray();
        }
    }
}
