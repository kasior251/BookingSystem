using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookingSystem.Controllers;
using BookingSystem.BLL;
using BookingSystem.DAL;
using BookingSystem.Model;
using System.Web.Mvc;
using System.Linq;
using System;
using Moq;
using System.Web;

namespace UnitTest
{
    [TestClass]
    public class AdminControllerTest
    {
        [TestMethod]
        public void Index()
        {   
            //setter session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (ViewResult)controller.Index();

            Assert.AreEqual(actionResult.ViewName, "");
        }

        //sjekke om en admin får logget seg inn
        [TestMethod]
        public void Test_Index_Post()
        {
            //Sett session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var admin = new Admin()
            {
                username = "newAdmin",
                password = "password"
            };


            //Act
            var result = (RedirectToRouteResult)controller.Index(admin);

            //Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "AdminMenu");
        }

        //sjekker om et tomt username-felt gir feil i innlogging
        [TestMethod]
        public void Test_Index_Post_Empty_Username()
        {
            //sett authorized i session til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var admin = new Admin()
            {
                username = "",
                password = ""
            };

            //Act
            var result = (ViewResult)controller.Index(admin);

            //Assert
            Assert.AreEqual(result.ViewName, "");
        }

        //tester om en tom admin gir feil i registrering 
        [TestMethod]
        public void Test_Register_Admin_WrongFormat_Post()
        {
            //Arrange
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()));
            var admin = new Admin()
            {
                username = "",
                password = ""
            };
            controller.ViewData.ModelState.AddModelError("Username", "Username can't be empty");

            //Act
            var result = (ViewResult)controller.RegisterAdmin(admin);

            //Assert
            Assert.IsTrue(result.ViewData.ModelState.Count == 1);
            Assert.AreEqual(result.ViewName, "");
        }
    }
}
