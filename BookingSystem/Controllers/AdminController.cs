using BLL;
using BookingSystem.BLL;
using BookingSystem.Model;
using BookingSystem.Models;
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
            var AdminDB = new AdminLogic();

            if(AdminLogic.adminExists(admin))
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

            var adminLogic = new AdminLogic();
            if (adminLogic.addAdmin(admin))
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
                var routeLogic = new RouteLogic();
                return View(routeLogic.findAllRoutes());
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
                var routeLogic = new RouteLogic();
                routeLogic.addNew(route);
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
                var routeLogic = new RouteLogic();
                if (routeLogic.deleteRoute(routeId))
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
                var scheduleLogic = new ScheduleLogic();
                List<Schedule> flights = scheduleLogic.getFlights(routeId);
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
                return View();
            }
        }
        public string CreateNewFlight(long departure, long arrival, int seats, int price)
        {
            var scheduleLogic = new ScheduleLogic();
            var status = "";
            if (scheduleLogic.addNewFlight(departure, arrival, seats, price, (int)Session["RouteId"]))
            {
                status = "OK";
            } else
            {
                status = "ERROR";
            }
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(status);
            
            
            //return RedirectToAction("SeeFlights", new { routeId = (int)Session["RouteId"] });

        }

        public string FindRoute(int id)
        {
            var routeLogic = new RouteLogic();
            List<string> cities = new List<string>();
            Route route = routeLogic.getRoute(id);
            cities.Add(route.origin);
            cities.Add(route.destination);
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(cities);

        }

        public string GetDates(int id)
        {
            var scheduleLogic = new ScheduleLogic();
            List<long> dates = new List<long>();
            List<Schedule> flights = scheduleLogic.getFlights(id);
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
            checkStatus();
            if (!(bool)Session["Authorized"])
            {
                return RedirectToAction("Index");
            }
            else
            {
                var scheduleLogic = new ScheduleLogic();
                if (scheduleLogic.deleteFlight(id)) {
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

