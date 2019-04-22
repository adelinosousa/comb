using System.Collections.Generic;

namespace Site.Comb
{
    public class CombResponse
    {
        public List<string> Errors { get; }

        public bool Success => Errors.Count == 0;

        public ICombLink Result { get; set; }

        public CombResponse()
        {
            Errors = new List<string>();
        }
    }
}
