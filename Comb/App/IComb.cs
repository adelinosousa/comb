using System.Threading.Tasks;

namespace Site.Comb
{
    public interface IComb
    {
        Task<CombResponse> Brush(CombRequest request);
    }
}
