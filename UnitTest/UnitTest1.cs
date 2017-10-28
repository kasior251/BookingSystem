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
using System.Web.Script.Serialization;

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

            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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

            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (RedirectToRouteResult)controller.AdminMenu();

            Assert.AreEqual(actionResult.RouteValues.Values.First(), "Index");
        }

        //sjekke SeeRoutes metoden mens utlogget
        [TestMethod]
        public void Test_SeeRoutes_LoggedOut()
        {
            //Sett session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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
        public void Test_AddRoute_LoggedIn()
        {
            //setter session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(true);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (ViewResult)controller.AddRoute();

            Assert.AreEqual(actionResult.ViewName, "");
        }

        //sjekke add route mens pålogget
        [TestMethod]
        public void Test_AddRoute_LoggedOut()
        {
            //setter session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
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

        [TestMethod]
        public void Test_DeteteRoute_LoggedOut()
        {
            //Sett session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (RedirectToRouteResult)controller.DeleteRoute(1);

            Assert.AreEqual(actionResult.RouteValues.Values.First(), "Index");
        }

        [TestMethod]
        public void Test_DeteteRoute_LoggedInInvalidId()
        {
            //Sett session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(true);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (RedirectToRouteResult)controller.DeleteRoute(0);
            
            Assert.AreEqual(actionResult.RouteValues.Values.First(), "SeeRoutes");

        }

        [TestMethod]
        public void Test_SeeFlights_LoggedOut()
        {
            //Sett session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (RedirectToRouteResult)controller.SeeFlights(1);

            Assert.AreEqual(actionResult.RouteValues.Values.First(), "Index");
        }

        [TestMethod]
        public void Test_SeeFlights_LoggedIn()
        {
            //Sett session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.Setup(s => s["Authorized"]).Returns(true);
            mockSession.Setup(s => s["RouteId"]).Returns(1);
            mockSession.SetupAllProperties();

            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()), 
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            List<Schedule> flights = new List<Schedule>();
            Schedule f = new Schedule()
            {
                id = 1,
                departureDate = 100,
                arrivalDate = 10,
                seatsLeft = 0,
                price = 0,
                route = new Route()
                {
                    id = 1,
                    origin = "Oslo",
                    destination = "Bergen"
                }
            };
            flights.Add(f);
            flights.Add(f);
            flights.Add(f);

            var actionResult = (ViewResult)controller.SeeFlights(1);
            var result = (List<Schedule>)actionResult.Model;

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < flights.Count; i++) {
                Assert.AreEqual(flights[i].id, result[i].id);
                Assert.AreEqual(flights[i].departureDate, result[i].departureDate);
                Assert.AreEqual(flights[i].arrivalDate, result[i].arrivalDate);
                Assert.AreEqual(flights[i].seatsLeft, result[i].seatsLeft);
                Assert.AreEqual(flights[i].price, result[i].price);
                Assert.AreEqual(flights[i].route.id, result[i].route.id);
                Assert.AreEqual(flights[i].route.origin, result[i].route.origin);
                Assert.AreEqual(flights[i].route.destination, result[i].route.destination);
            }
        }

        [TestMethod]
        public void Test_CreateFlight_LoggedOut()
        {
            //Sett session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (RedirectToRouteResult)controller.CreateFlight();

            Assert.AreEqual(actionResult.RouteValues.Values.First(), "Index");
        }

        [TestMethod]
        public void Test_CreateFlight_Loggedin()
        {
            //Sett session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(true);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (ViewResult)controller.CreateFlight();

            Assert.AreEqual(actionResult.ViewName, "");
        }

        [TestMethod]
        public void Test_CreateNewFlight()
        {
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            //trenger å ha en id i RouteId session
            mockSession.SetupGet(s => s["RouteId"]).Returns(1);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            //bør få ERROR siden den første variablelen er satt til 0
            var actionResult = controller.CreateNewFlight(0, 0, 0, 0);
            var expectedResult = (new JavaScriptSerializer()).Serialize("ERROR");

            //bør få OK siden den første variablelen ikke er satt til 0
            var actionResult2 = controller.CreateNewFlight(1, 0, 0, 0);
            var expectedResult2 = (new JavaScriptSerializer()).Serialize("OK");

            Assert.AreEqual(actionResult, expectedResult);
            Assert.AreEqual(actionResult2, expectedResult2);
        }

        [TestMethod]
        public void Test_FindRoute()
        {
            List<string> cities = new List<string>();
            cities.Add("Oslo");
            cities.Add("Bergen");

            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));

            var actionResult = controller.FindRoute(1);
            var expectedResult = (new JavaScriptSerializer()).Serialize(cities);

            Assert.AreEqual(actionResult, expectedResult);
        }

        [TestMethod]
        public void Test_GetDates()
        {
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));

            List<Schedule> list = new List<Schedule>();

            var times = new List<long>();

            //som satt i stub-metoden
            long i = 100;
            long j = 10;

            for (int teller = 0; teller < 3; teller++)
            {
                times.Add(i);
                times.Add(j);
            }
            var actionResult = controller.GetDates(1);
            var expectedResult = (new JavaScriptSerializer().Serialize(times));

            Assert.AreEqual(actionResult, expectedResult);
        }

        [TestMethod]
        public void Test_DeleteFlight_loggedOut()
        {
            //Sett session authorized til false
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(s => s["Authorized"]).Returns(false);
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (RedirectToRouteResult)controller.AdminMenu();

            Assert.AreEqual(actionResult.RouteValues.Values.First(), "Index");
        }

        [TestMethod]
        public void Test_DeleteFlight_loggedIn()
        {
            //Sett session authorized til true
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.Setup(s => s["Authorized"]).Returns(true);
            mockSession.Setup(s => s["RouteId"]).Returns(1);
            mockSession.SetupAllProperties();
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);
            var controller = new AdminController(new AdminLogic(new DatabaseDALStub()),
                new RouteLogic(new DatabaseDALStub()), new ScheduleLogic(new DatabaseDALStub()));
            controller.ControllerContext = mockControllerContext.Object;

            var actionResult = (RedirectToRouteResult)controller.DeleteFlight(1);

            Assert.AreEqual(actionResult.RouteValues.Values.First(), 1);
        }
    }
}
