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
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class AdminControllerTest
    {
        //sjekke index fro AdminController
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

        [TestMethod]
        public void Test_logOut()
        {
            //sett authorized i session til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (ViewResult)controller.Index();

            Assert.AreEqual(actionResult.ViewName, "");
        }

        [TestMethod]
        public void Test_RegisterAdmin()
        {
            //setter session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(true);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (ViewResult)controller.RegisterAdmin();

            Assert.AreEqual(actionResult.ViewName, "");
        }

        [TestMethod]
        public void Test_RegisterAdmin_Post()
        {
            //Sett session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(true);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var admin = new Admin()
            {
                username = "newAdmin",
                password = "password"
            };


            //Act
            var result = (RedirectToRouteResult)controller.RegisterAdmin(admin);

            //Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "AdminMenu");
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

        //sjekk AdminMenu method mens pålogget
        [TestMethod]
        public void Test_AdminMenu_LoggedIn()
        {
            //Sett session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(true);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (ViewResult)controller.AdminMenu();
            Assert.AreEqual(actionResult.ViewName, "");
        }

        //sjekk AdminMenu metode mens utlogget
        [TestMethod]
        public void Test_AdminMenu_LoggedOut()
        {
            //Sett session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (RedirectToRouteResult)controller.AdminMenu();

            Assert.AreEqual(actionResult.RouteValues.Values.First(), "Index");
        }

        //sjekke SeeRoutes metoden mens utlogget
        [TestMethod]
        public void Test_SeeRoutes_LoggetOut()
        {
            //Sett session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new RouteLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (RedirectToRouteResult)controller.SeeRoutes();

            Assert.AreEqual(actionResult.RouteValues.Values.First(), "Index");
        }

        //sjekke SeeRoutes metoden mens pålogget
        [TestMethod]
        public void Test_SeeRoutes_LoggedIn()
        {
            //Sett session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(true);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new RouteLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            List<Route> routes = new List<Route>();
            Route r = new Route()
            {
                id = 1,
                origin = "Oslo",
                destination = "Bergen"
            };
            routes.Add(r);
            routes.Add(r);
            routes.Add(r);

            var actionResult = (ViewResult)controller.SeeRoutes();
            var result = (List<Route>)actionResult.Model;

            //sjekke om action result stemmer
            Assert.AreEqual(actionResult.ViewName, "");

            //sjekke om alle verdier i lista stemmer med det vi bør få
            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(routes[i].id, result[i].id);
                Assert.AreEqual(routes[i].origin, result[i].origin);
                Assert.AreEqual(routes[i].destination, result[i].destination);
            }
        }

        //sjekke add route mens pålogget
        [TestMethod]
        public void Test_AddRoute_LoggetIn()
        {
            //setter session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(true);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var controller = new AdminController(new RouteLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (ViewResult)controller.AddRoute();

            Assert.AreEqual(actionResult.ViewName, "");
        }

        //sjekke add route mens pålogget
        [TestMethod]
        public void Test_AddRoute_LoggetOut()
        {
            //setter session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var controller = new AdminController(new RouteLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (RedirectToRouteResult)controller.AddRoute();

            Assert.AreEqual(actionResult.RouteValues.Values.First(), "Index");
        }

        [TestMethod]
        public void Test_AddRoute_Post()
        {
            //Sett session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(true);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new RouteLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var route = new Route()
            {
                id = 1,
                origin = "Oslo",
                destination = "Bergen"
            };


            //Act
            var result = (RedirectToRouteResult)controller.AddRoute(route);

            //Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "SeeRoutes");
        }
    }
}
