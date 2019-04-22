using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using System.Threading.Tasks;

namespace Site.Comb.Tests
{
    [TestClass]
    public class CombTests
    {
        private ICombHttpClient combHttpClient;
        private Comb comb;

        [TestInitialize]
        public void Setup()
        {
            this.combHttpClient = Substitute.For<ICombHttpClient>();
            this.comb = new Comb(combHttpClient);
        }

        [TestMethod]
        public async Task CombWithNullOrEmptyUrl()
        {
            var response = await this.comb.Brush(new CombRequest
            {
                Url = null
            });

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Count == 1);
            Assert.AreEqual(response.Errors.First(), "Invalid Url");
        }
    }
}
