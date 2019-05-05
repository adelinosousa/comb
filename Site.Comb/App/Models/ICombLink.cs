namespace Site.Comb
{
    public interface ICombLink
    {
        string Value { get; }

        CombLinkType Type { get; }

        ICombLink[] Descendants { get; }

        ICombLink[] All();

        ICombLink[] All(CombLinkType linkType);
    }
}
