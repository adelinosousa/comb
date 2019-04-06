namespace Comb
{
    public interface ICombLink
    {
        string Value { get; }

        CombLinkType Type { get; }

        ICombLink[] Descendents { get; }

        ICombLink[] All();

        ICombLink[] All(CombLinkType linkType);
    }
}
