using System.Collections.Generic;

namespace Comb
{
    public class CombResponse
    {
        public List<string> Errors { get; }

        public bool Success => Errors.Count == 0;

        public CombResponse()
        {
            Errors = new List<string>();
        }
    }
}
