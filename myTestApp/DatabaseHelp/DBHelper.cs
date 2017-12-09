using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using myTestApp.Models;

namespace myTestApp.DatabaseHelp
{
    public class DBHelper
    {
        public DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

        public DBHelper()
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite("Data Source=myDatabase.db");
        }

        public List<TimeCard> getAllTimeCard()
        {
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {
                List<TimeCard> timeCardList = new List<TimeCard>();
                timeCardList = db.Timecards.FromSql("SELECT * FROM timeCard").ToList();
                return timeCardList;
            }
        }
        //timeCardID
        //startTime
        //stopTime
        //userID
        //totalTime
        //revisionHistory
        //lastModDate
        //comments
        public List<TimeCard> getAllUserTimeCard(String userID)
        {
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {
                List<TimeCard> timeCardList = new List<TimeCard>();
                timeCardList = db.Timecards.FromSql("SELECT * FROM timeCard WHERE userID = {0}", userID).ToList();
                return timeCardList;
            }
        }

        public List<Project> getAllProject()
        {
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {
                List<Project> projList = new List<Project>();
                projList = db.Projects.FromSql("SELECT * FROM Project").ToList();
                return projList;
            }
        }


        public List<Group> getProjectGroups(String projectID)
        {
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {
                List<Group> groupList = new List<Group>();
                groupList = db.Groups.FromSql("SELECT * FROM Group WHERE ProjectID = {0}", projectID).ToList();
                return groupList;
            }
        }

        public int insertUser(User u)
        {
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {
                db.User.Add(u);
                return db.SaveChanges();
            }
        }

        public int insertGroup(Group g)
        {
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {
                db.Groups.Add(g);
                return db.SaveChanges();
            }
        }

        public int insertProject(Project p)
        {
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {
                db.Projects.Add(p);
                return db.SaveChanges();
            }
        }

        public int insertTimeCard(TimeCard t)
        {
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {
                //timeCardID
                //startTime
                //stopTime
                //userID
                //totalTime
                //revisionHistory
                //lastModDate
                //comments
                string[] array = new string[] { t.startTime, t.stopTime, t.userID.ToString(), t.totalTime.ToString(), t.revisionHistory, t.lastModDate, t.comments };
                db.Database.ExecuteSqlCommand("INSERT INTO timeCard (startTime, stopTime, userID, totalTime, revisionHistory, lastModDate, comments)" +
                                              " VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6})", array);
                //db.Timecards.Add(t);
                return db.SaveChanges();
            }
        }

        public int updateProject(Project p)
        {
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {

                db.Database.ExecuteSqlCommand("UPDATE dbo.Project SET Name = '{1}', isActive = {2} WHERE projectID = {0}", p.projectID, p.name, p.activeStatus);
                //db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChanges();
            }

        }
    }
}
