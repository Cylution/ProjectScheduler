using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectScheduler.Models;

namespace ProjectScheduler.Controllers
{
    public class ProjectsController : Controller
    {
        private ProjectDBContext db = new ProjectDBContext();

        
        // GET: Projects
        public ActionResult Index(string projectResource, string searchString, string searchFreeStartDate)
        {
            var ResourceLst = new List<string>();

            var ResourceQry = from d in db.Projects
                              orderby d.Resource
                              select d.Resource;

            ResourceLst.AddRange(ResourceQry.Distinct());
            ViewBag.projectResource = new SelectList(ResourceLst);

            var projs = from p in db.Projects
                           select p;

            //sdate going to need to think about a range block where
            //the ranges dont include search range.. maybe show a calendar
            //with resource availability on those dates....
            var StartDateQry = from d in db.Projects
                               orderby d.StartDate
                               where (d.StartDate.ToString() == searchFreeStartDate) 
                               select d.StartDate;

            if (!String.IsNullOrEmpty(searchString))
            {
                projs = projs.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(projectResource))
            {
                projs = projs.Where(x => x.Resource == projectResource);
            }

            if (!string.IsNullOrEmpty(StartDateQry.ToString()))
            {
       //         projs = projs.Where(x => x.StartDate == StartDateQry);
            }


            //rem date ranges searching
            //will need to enter start date for a new project free resources



            return View(projs);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        //[Authorize(Roles = "derriford\\Systems")]
        [Authorize(Users="derriford\\colliers")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "derriford\\colliers")]
        public ActionResult Create([Bind(Include = "ID,Title,StartDate,EndDate,PM,Resource,Notes")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Users = "derriford\\colliers")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Users = "derriford\\colliers")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,StartDate,EndDate,PM,Resource,Notes")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Users = "derriford\\colliers")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize(Users = "derriford\\colliers")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
