using BookingSystem.BLL;
using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BookingSystem.Controllers
{
    public class AdminController : Controller
    {
        private IAdminLogic _adminBLL;
        private IRouteLogic _routeBLL;
        private IScheduleLogic _scheduleBLL;

        public AdminController()
        {
            _adminBLL = new AdminLogic();
            _routeBLL = new RouteLogic();
            _scheduleBLL = new ScheduleLogic();
        }

        public AdminController(IAdminLogic stub, IRouteLogic stubR, IScheduleLogic stubS)
        {
            _adminBLL = stub;
            _routeBLL = stubR;
            _scheduleBLL = stubS;
        }

        public AdminController(IRouteLogic stub)
        {
            _routeBLL = stub;
        }

        public AdminController(IScheduleLogic stub)
        {
            _scheduleBLL = stub;
        }

        public ActionResult Index()
        {
            ViewBag.LoginFail = false;
            checkStatus();
            if ((bool)Session["Authorized"])
            {
                return RedirectToAction("AdminMenu");
            }
            else
            {
                return View();
            }
        }

        private void checkStatus()
        {
            if (Session["Authorized"] == null)
            {
                Session["Authorized"] = true;
            }
        }

        [HttpPost]
        public ActionResult Index(Admin admin)
        {
            if(_adminBLL.adminExists(admin))
            {
                Session["Authorized"] = true;
                ViewBag.LoginFail = false;
                return RedirectToAction("AdminMenu");
            }
            else
            {
                Session["Authorized"] = false;
                ViewBag.LoginFail = true;
                return View();
            }

        }

        public ActionResult LogOut()
        {
            Session["Authorized"] = false;
            return RedirectToAction("Index");
        }

        public ActionResult RegisterAdmin()
        {
            checkStatus();
            if (!(bool)Session["Authorized"])
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public ActionResult RegisterAdmin(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_adminBLL.addAdmin(admin))
            {
                return RedirectToAction("AdminMenu");
            }
            return View();
        }

        public ActionResult AdminMenu()
        {
            checkStatus();
            if ((bool)Session["Authorized"])
            {
                Session["ErrorDeletingRoute"] = false;
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        public ActionResult SeeRoutes()
        {
            checkStatus();
            if ((bool)Session["Authorized"])
            {
                Session["ErrorDeleting"] = false;
                ViewBag.ErrorDeletingRoute = Session["ErrorDeletingRoute"];
                return View(_routeBLL.findAllRoutes());
            } else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddRoute()
        {
            checkStatus();
            if (!(bool)Session["Authorized"])
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddRoute(Route route)
        {
            checkStatus();
            if (!(bool)Session["Authorized"])
            {
                return RedirectToAction("Index");
            }
            else
            {
                _routeBLL.addNew(route);
                return RedirectToAction("SeeRoutes");
            }
        }

        public ActionResult DeleteRoute(int routeId)
        {
            checkStatus();
            if (!(bool)Session["Authorized"])
            {
                return RedirectToAction("Index");
            } else
            {
                if (_routeBLL.deleteRoute(routeId))
                {
                    Session["ErrorDeletingRoute"] = false;
                    return RedirectToAction("SeeRoutes");
                }
                else
                {
                    Session["ErrorDeletingRoute"] = true;
                    return RedirectToAction("SeeRoutes");
                }
                
            }
        }

        public ActionResult SeeFlights(int routeId)
        {
            checkStatus();
            if (!(bool)Session["Authorized"])
            {
                return RedirectToAction("Index");
            }
            else
            {
                Session["RouteId"] = routeId;
                if (Session["ErrorDeleting"] == null)
                {
                    ViewBag.ErrorDeleting = false;
                }
                else
                {
                    ViewBag.ErrorDeleting = Session["ErrorDeleting"];
                }
                List<Schedule> flights = _scheduleBLL.getFlights(routeId);
                return View(flights);
            }
        }

        public ActionResult CreateFlight()
        {
            checkStatus();
            if (!(bool)Session["Authorized"])
            {
                return RedirectToAction("Index");
            }
            else
            {
                //vis bilde for å skape en ny flyvning
                return View();
            }
        }

        public string CreateNewFlight(long departure, long arrival, int seats, int price)
        {
            var status = "";
            if (_scheduleBLL.addNewFlight(departure, arrival, seats, price, (int)Session["RouteId"]))
            {
                //returnere OK dersom flyvningen ble registrert i db
                status = "OK";
            } else
            {
                //returnere "ERROR" ellers
                status = "ERROR";
            }
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(status);

        }

        public string FindRoute(int id)
        {
            List<string> cities = new List<string>();
            Route route = _routeBLL.getRoute(id);
            cities.Add(route.origin);
            cities.Add(route.destination);
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(cities);

        }

        //få en liste av avgangs- og ankomst datoer for gjeldende avganger
        public string GetDates(int id)
        {
            List<long> dates = new List<long>();
            List<Schedule> flights = _scheduleBLL.getFlights(id);
            foreach (Schedule s in flights)
            {
                dates.Add(s.departureDate);
                dates.Add(s.arrivalDate);
            }
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(dates);
        }

        public ActionResult DeleteFlight(int id)
        {
            //sjekk om innlogget, hvis ikke send til innlogingsside
            checkStatus();
            if (!(bool)Session["Authorized"])
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (_scheduleBLL.deleteFlight(id)) {
                    Session["ErrorDeleting"] = false;
                    return RedirectToAction("SeeFlights", new { routeId = (int)Session["RouteId"] });
                } 
                else
                {
                    Session["ErrorDeleting"] = true;
                    return RedirectToAction("SeeFlights", new { routeId = (int)Session["RouteId"] });
                }
            }
        }
    }

}

