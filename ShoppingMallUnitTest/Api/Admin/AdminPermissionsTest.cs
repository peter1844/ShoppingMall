using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingMall.Api.Login;
using System;
using System.Web;

namespace ShoppingMallUnitTest.Api.Admin
{
    [TestClass]
    public class AdminPermissionsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            LoginByAcc login = new LoginByAcc();
            PrivateObject privateObject = new PrivateObject(login);

            privateObject.SetFieldOrProperty("context", new Mock<HttpContextBase>().Object);
        }
    }
}
