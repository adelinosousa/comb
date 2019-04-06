using System.Threading.Tasks;

namespace Comb
{
    public interface IComb
    {
        Task<CombResponse> Brush(CombRequest request);
    }
}
