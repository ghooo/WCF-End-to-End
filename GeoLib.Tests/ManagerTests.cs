using System;
using System.ServiceModel;
using GeoLib.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GeoLib.Data;
using GeoLib.Services;
using System.ServiceModel.Channels;

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

        [TestMethod]
        public void test_zip_code_retrieval_integration_test()
        {
            string address = "net.pipe://localhost/GeoService";
            Binding binding = new NetNamedPipeBinding();

            ServiceHost host = new ServiceHost(typeof(GeoManager));

            host.AddServiceEndpoint(typeof(IGeoService), binding, address);

            host.Open();

            ChannelFactory<IGeoService> factory = new ChannelFactory<IGeoService>(
                binding, new EndpointAddress(address));
            IGeoService proxy = factory.CreateChannel();

            ZipCodeData data = proxy.GetZipInfo("07035");

            Assert.AreEqual("LINCOLN PARK", data.City.ToUpper());
            Assert.AreEqual("NJ", data.State);
        }
    }
}
