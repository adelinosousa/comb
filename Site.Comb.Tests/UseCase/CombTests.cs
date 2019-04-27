using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.IO;
using System.Linq;
using System.Reflection;
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
        [DataRow(null)]
        [DataRow("test.com")]
        [DataRow("http://")]
        [DataRow("abc")]
        [DataRow("http:/test")]
        [DataRow("http.test.com")]
        public async Task CombWithInvalidUrl(string url)
        {
            var response = await this.comb.Brush(new CombRequest
            {
                Url = url
            });

            this.combHttpClient.Received(0);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Errors.Count == 1);
            Assert.AreEqual(response.Errors.First(), "Invalid Url");
        }

        [TestMethod]
        [DataRow("http://test.com", "", 0, 0, 0, 0)]
        [DataRow("http://test.com", "DescendentTest.html", 5, 0, 0, 0)]
        [DataRow("http://test.com", "ImageTest.html", 0, 7, 0, 0)]
        [DataRow("http://test.com", "VideoTest.html", 0, 0, 4, 0)]
        [DataRow("http://test.com", "OtherTest.html", 0, 0, 0, 3)]
        public async Task Comb(string url, string fileName, int numberOfDescendents, int numberOfImg, int numberOfVideo, int numberOfOther)
        {
            var content = ReadFileContent(fileName);

            this.combHttpClient.FetchHtmlAsync(Arg.Any<string>()).Returns(Task.FromResult(content));

            var response = await this.comb.Brush(new CombRequest
            {
                Url = url
            });

            this.combHttpClient.Received(1);

            Assert.IsTrue(response.Success);

            Assert.AreEqual(response.Result.Value, url);
            Assert.AreEqual(response.Result.Type, CombLinkType.URL);

            Assert.AreEqual(response.Result.Descendents.Count(), numberOfDescendents);
            foreach (var descendent in response.Result.Descendents)
            {
                Assert.AreEqual(descendent.Descendents.Count(), 0);
            }

            Assert.AreEqual(response.Result.All(CombLinkType.IMG).Count(), numberOfImg);
            Assert.AreEqual(response.Result.All(CombLinkType.VIDEO).Count(), numberOfVideo);
            Assert.AreEqual(response.Result.All(CombLinkType.OTHER).Count(), numberOfOther);
        }

        private string ReadFileContent(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return string.Empty;

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"UseCase/TestCases/{fileName}");
            return File.ReadAllText(path);
        }
    }
}
