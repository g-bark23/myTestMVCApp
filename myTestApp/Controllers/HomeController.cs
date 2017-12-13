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
            HttpContext.Session.SetString(SELECTEDUSER, "");
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
            //Initialize session variables
            var sessionUserID = HttpContext.Session.GetString(USERKEY);
            var sessionProjectID = HttpContext.Session.GetString(PROJECTKEY);
            var sessionGroupID = HttpContext.Session.GetString(GROUPKEY);
            var sessionSelectedUserID = HttpContext.Session.GetString(SELECTEDUSER);

            ViewBag.sessionUserID = sessionUserID;
            ViewBag.sessionProjectID = sessionProjectID;
            ViewBag.sessionGroupID = sessionGroupID;
            ViewBag.sessionSelectedUserID = sessionSelectedUserID;
            //Initialize Database Helper
            DBHelper dbhelp = new DBHelper();

            //Data Structures
                //<User Full Name, Users Total Worked Hours>
            Dictionary<String, String> groupUserHours = new Dictionary<string, string>();

            String projectHoursHigh;
            String projectHoursAverage;
            String projectHoursLow;
            String projectHoursSelectedUser;

                //<Group Name, Group Hours>
            Dictionary<String, String> projectGroupHours = new Dictionary<string, string>();
            List<TimeCard> timeCardList = new List<TimeCard>();
            List<Project> allProjects = new List<Project>();
            List<Group> projectGroups = new List<Group>();
            List<User> groupUsers = new List<User>();

            bool isLoggedInUserSelected;
            bool isLoggedInUserAdmin;
            if (sessionUserID.Equals(sessionSelectedUserID))
            {
                isLoggedInUserSelected = true;
            }
            else
            {
                isLoggedInUserSelected = false;
            }


            isLoggedInUserAdmin = getIsLoggedInUserAdmin(sessionUserID);
            allProjects = dbhelp.getAllProject();
            projectGroups = getProjcetGroups(sessionProjectID);
            groupUsers = getGroupUsers(sessionGroupID);
            timeCardList = dbhelp.getAllUserTimeCard(sessionSelectedUserID);
            projectHoursHigh = getProjectHoursHigh();
            projectHoursAverage = getProjectAverageHours();
            projectHoursLow = getProjectHoursLow();
            groupUserHours = getGroupUserHours(sessionGroupID);
            projectHoursSelectedUser = getUserTotalHours(sessionSelectedUserID);

            ViewBag.allProjects = allProjects;
            ViewBag.projectGroups = projectGroups;
            ViewBag.groupUsers = groupUsers;
            ViewBag.timeCardList = timeCardList;
            ViewBag.projectHoursHigh = projectHoursHigh;
            ViewBag.projectHoursAverage = projectHoursAverage;
            ViewBag.projectHoursLow = projectHoursLow;
            ViewBag.groupUserHours = groupUserHours;
            ViewBag.projectHoursSelectedUser = projectHoursSelectedUser;
            ViewBag.projectGroups = projectGroups;
            ViewBag.isLoggedInUserAdmin = isLoggedInUserAdmin;
            ViewBag.isLoggedInUserSelected = isLoggedInUserSelected;


            string Message = "LOGGED IN USER ID " + sessionUserID;
            ViewBag.userID = Message;
            Message = "CURRENT GROUP ID " + sessionGroupID;
            ViewBag.groupID = Message;
            Message = "CURRENT PROJECT ID " + sessionProjectID;
            ViewBag.projectID = Message;
            timeCardList = dbhelp.getAllUserTimeCard("13");
            ViewBag.timeCardList = timeCardList;

            return View();
        }

        private bool getIsLoggedInUserAdmin(string sessionUserID)
        {
            DBHelper dBHelper = new DBHelper();
            User u = dBHelper.getUser(sessionUserID);
            if(u.isAdmin == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<User> getGroupUsers(string sessionGroupID)
        {
            DBHelper dBHelper = new DBHelper();
            return dBHelper.getGroupUsers(sessionGroupID);
        }

        private List<Group> getProjcetGroups(string sessionProjectID)
        {
            DBHelper dBHelper = new DBHelper();
            return dBHelper.getProjectGroups(sessionProjectID);
        }

        private string getProjectHoursHigh()
        {
            DBHelper dbhelp = new DBHelper();
            List<User> projectUsers = new List<User>();
            projectUsers = dbhelp.getProjectUsers(PROJECTKEY);
            float highHours = 0;
            foreach(User u in projectUsers)
            {
                List<TimeCard> timecards = new List<TimeCard>();
                timecards = dbhelp.getAllUserTimeCard(u.userID.ToString());
                float userTotalTime = 0;
                foreach (TimeCard t in timecards)
                {
                    userTotalTime += float.Parse(t.totalTime.ToString());
                }
                if(highHours < userTotalTime)
                {
                    highHours = userTotalTime;
                }
            }
            return highHours.ToString();
        }

        private string getProjectHoursLow()
        {
            DBHelper dbhelp = new DBHelper();
            List<User> projectUsers = new List<User>();
            projectUsers = dbhelp.getProjectUsers(PROJECTKEY);
            float highHours = 0;
            bool first = true;
            foreach (User u in projectUsers)
            {
                List<TimeCard> timecards = new List<TimeCard>();
                timecards = dbhelp.getAllUserTimeCard(u.userID.ToString());
                float userTotalTime = 0;
                foreach (TimeCard t in timecards)
                {
                    userTotalTime += float.Parse(t.totalTime.ToString());
                }
                if ((highHours < userTotalTime) || first)
                {
                    first = false;
                    highHours = userTotalTime;
                }
            }
            return highHours.ToString();
        }

        private string getProjectAverageHours()
        {
            DBHelper dbhelp = new DBHelper();
            List<User> projectUsers = new List<User>();
            projectUsers = dbhelp.getProjectUsers(PROJECTKEY);
            float averageHours = 0;
            float totalHours = 0;
            foreach (User u in projectUsers)
            {
                List<TimeCard> timecards = new List<TimeCard>();
                timecards = dbhelp.getAllUserTimeCard(u.userID.ToString());
                float userTotalTime = 0;
                foreach (TimeCard t in timecards)
                {
                    userTotalTime += float.Parse(t.totalTime.ToString());
                }
                totalHours += userTotalTime;
            }
            averageHours = totalHours / projectUsers.Count;
            return averageHours.ToString();
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

        private Dictionary<String, String> getGroupUserHours(String groupID)
        {
            DBHelper dbhelp = new DBHelper();
            Dictionary<String, String> groupUserHours = new Dictionary<string, string>();
            List<User> groupUsers = new List<User>();
            groupUsers = dbhelp.getGroupUsers(groupID);
            foreach(User u in groupUsers)
            {
                List<TimeCard> userTimecard = new List<TimeCard>();
                userTimecard = dbhelp.getAllUserTimeCard(u.userID, groupID);
                float userTotalTime = 0;
                foreach(TimeCard t in userTimecard)
                {
                    userTotalTime += float.Parse(t.totalTime.ToString());
                }
                groupUserHours.Add(u.name, userTotalTime.ToString());
            }
            return groupUserHours;
        }

        private String getUserTotalHours(String uID)
        {
            DBHelper dbHelper = new DBHelper();
            List<TimeCard> timecards = dbHelper.getAllUserTimeCard(uID);
            float userTotalTime = 0;
            foreach (TimeCard t in timecards)
            {
                userTotalTime += float.Parse(t.totalTime.ToString());
            }
            return userTotalTime.ToString();
        }
    }
}
