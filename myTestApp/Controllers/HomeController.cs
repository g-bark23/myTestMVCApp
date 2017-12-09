using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using myTestApp.Models;
using myTestApp.DatabaseHelp;
using static myTestApp.Session.SessionHelper;
using Microsoft.AspNetCore.Http;

namespace myTestApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetString(USERKEY, "");
            HttpContext.Session.SetString(PROJECTKEY, "");
            HttpContext.Session.SetString(GROUPKEY, "");
            return RedirectToAction("myTestView", "Home");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult myTestView()
        {
            ViewData["Message"] = "This is my test page.";

            return View();
        }

        public IActionResult dashboard()
        {
            var sessionUserID = HttpContext.Session.GetString(USERKEY);
            var sessionProjectID = HttpContext.Session.GetString(PROJECTKEY);
            var sessionGroupID = HttpContext.Session.GetString(GROUPKEY);

            DBHelper dbhelp = new DBHelper();
            string Message = "LOGGED IN USER ID " + sessionUserID;
            ViewBag.userID = Message;
            Message = "CURRENT GROUP ID " + sessionGroupID;
            ViewBag.groupID = Message;
            Message = "CURRENT PROJECT ID " + sessionProjectID;
            ViewBag.projectID = Message;
            List<String> testList = new List<string>();
            testList.Add("test1");
            testList.Add("test2");
            testList.Add("test3");
            ViewBag.testList = testList;
            List<TimeCard> timeCardList = new List<TimeCard>();
            addTimeCard();
            timeCardList = dbhelp.getAllUserTimeCard("13");
            /*
            for (int i = 0; i < 10; i++)
            {
                TimeCard tc = new TimeCard();
                tc.comments = "testing + " + i;
                tc.stopTime = "11/17/17";
                tc.startTime = "11/16/17";
                tc.timeCardID = i;
                tc.userID = i + 10;
                timeCardList.Add(tc);
            }
            */
            //List<Project> allProjects;
            //List<Group> projectGroups;
            //List<User> groupUsers;
            ViewBag.timeCardList = timeCardList;
            return View();
        }

        public IActionResult Create()
        {

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void addTimeCard()
        {
            TimeCard tc = new TimeCard();
            tc.comments = "testing 1";
            tc.stopTime = "12/08/17";
            tc.startTime = "12/08/17";
            tc.timeCardID = 1234;
            tc.userID = 13;
            DBHelper dbhelp = new DBHelper();
            dbhelp.insertTimeCard(tc);
        }
    }
}
