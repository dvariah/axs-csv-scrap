using Axs.CsvScrap;
using NUnit.Framework;

namespace Axs.CvsScrap.Tests
{
    [TestFixture]
    internal class ReaderTests
    {
        Reader _sut;
        private const string TESTSTRING = "aaa,bbb,ccc,ddd";

        [SetUp]
        public void Setup()
        {
            _sut = new Reader();
        }

        [Test]
        [TestCase("aaa", 0)]
        [TestCase("bbb", 1)]
        [TestCase("ccc", 2)]
        [TestCase("ddd", 3)]
        public void GetField_ReturnElementAtIndex(string expected, int position)
        {
            var result = _sut.GetField(TESTSTRING, position);
            Assert.AreEqual(expected, result);
        }
    }
}
