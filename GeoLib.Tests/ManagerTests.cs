using System;
using GeoLib.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GeoLib.Data;
using GeoLib.Services;

namespace GeoLib.Tests
{
    [TestClass]
    public class ManagerTests
    {
        [TestMethod]
        public void test_zip_code_retrieval()
        {
            Mock<IZipCodeRepository> mockZipCodeRepository = new Mock<IZipCodeRepository>();

            ZipCode zipCode = new ZipCode()
            {
                City = "LINCOLN PARK",
                State = new State() {Abbreviation = "NJ"},
                Zip = "07035"
            };

            mockZipCodeRepository.Setup(obj => obj.GetByZip("07035")).Returns(zipCode);

            IGeoService geoService = new GeoManager(mockZipCodeRepository.Object);

            ZipCodeData data = geoService.GetZipInfo("07035");

            Assert.AreEqual("LINCOLN PARK", data.City.ToUpper());
            Assert.AreEqual("NJ", data.State);
        }
    }
}
