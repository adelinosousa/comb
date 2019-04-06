namespace Comb
{
    public interface ICombLink
    {
        string Value { get; }

        CombLinkType Type { get; }

        ICombLink[] All { get; }

        ICombLink[] Descendents { get; }
    }
}
