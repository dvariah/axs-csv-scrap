using Axs.CsvScrap;
using NUnit.Framework;

namespace Axs.CvsScrap.Tests
{
    [TestFixture]
    internal class PathHelperTests
    {
        PathHelper _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new PathHelper();
        }

        [Test]
        public void CreateExtractedSalesFilePath_ReturnsCorrectPath()
        {
            var result = PathHelper.CreateExtractedSalesFilePath("C:\\testFolder", "de", "pines", "42");
            Assert.AreEqual($"C:\\testFolder\\input\\sales\\de\\pines\\axs_sales_de_pines_42.csv.gz", result);
        }

        [Test]
        public void CreateExtractedPaymentsFilePath_ReturnsCorrectPath()
        {
            var result = PathHelper.CreateExtractedPaymentsFilePath("C:\\testFolder", "de", "pines", "42");
            Assert.AreEqual($"C:\\testFolder\\input\\payment\\de\\pines\\axs_payment_de_pines_42.csv.gz", result);
        }

        [Test]
        public void CreateExtracteddistributionsFilePath_ReturnsCorrectPath()
        {
            var result = PathHelper.CreateExtractedDistributionsFilePath("C:\\testFolder", "de", "pines", "42");
            Assert.AreEqual($"C:\\testFolder\\input\\payment_distribution\\de\\pines\\axs_payment_distribution_de_pines_42.csv.gz", result);
        }

        [Test]
        public void CreateControlCardSalesFilePath_ReturnsCorrectPath()
        {
            var result = PathHelper.CreateControlCardSalesFilePath("C:\\testFolder", "de", "pines", "42");
            Assert.AreEqual($"C:\\testFolder\\input\\sales\\de\\pines\\axs_sales_de_pines_42_control_card.csv.gz", result);
        }

        [Test]
        public void CreateControlCaardPaymentsFilePath_ReturnsCorrectPath()
        {
            var result = PathHelper.CreateControlCardPaymentsFilePath("C:\\testFolder", "de", "pines", "42");
            Assert.AreEqual($"C:\\testFolder\\input\\payment\\de\\pines\\axs_payment_de_pines_42_control_card.csv.gz", result);
        }

        [Test]
        public void CreateControlCardDistributionsFilePath_ReturnsCorrectPath()
        {
            var result = PathHelper.CreateControlCardDistributionsFilePath("C:\\testFolder", "de", "pines", "42");
            Assert.AreEqual($"C:\\testFolder\\input\\payment_distribution\\de\\pines\\axs_payment_distribution_de_pines_42_control_card.csv.gz", result);
        }
    }
}
