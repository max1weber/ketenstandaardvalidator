using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nl.ketenstandaard.api;

namespace KetenstandaardModule.Tests
{
    [TestClass]
    [TestCategory("KetenstandaardModule")]
    public class KetenModuleUnitTests
    {
        Validator validator;

        [TestInitialize]
        public void initClass()
        {
             validator = new Validator("mweber@volkerwessels.com", "MaWes09!", "VolkerWessels_01", "FTSIwlL1qvQhH7nrigZL");


        }

        [TestMethod]
        public void Authenticate()
        {

            validator.Authenticate();

            Assert.IsTrue(validator.IsAuthenticated);

        }


        [TestMethod]
        public void CheckInvalidXml()
        {

           
           // validator.Authenticate();

            //Assert.IsTrue(validator.IsAuthenticated);


            var xmlcontent = Resource.InvalidInvoiceXml.ToString();
            bool finalresult = false;
            var result = validator.ValidateXmlMessage("SALES", "004", "invoic", xmlcontent);
            finalresult = result.IsValid;

            Assert.AreEqual(6, result.totalErrorsFound);
            Assert.AreEqual(1, result.totalWarningsFound);

          

        }
    }
}
