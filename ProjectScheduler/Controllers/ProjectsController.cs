﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectScheduler.Models;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace ProjectScheduler.Controllers
{
    public class ProjectsController : Controller
    {
        private ProjectDBContext db = new ProjectDBContext();
        private DateTime searchStartDate;       

        // GET: Projects
        public ActionResult Index(string projectResource, string searchString, string searchFreeStartDate, 
            string sortOrder, int? page)
        {
            const int pageSize = 10;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "resource_desc" : "";
            ViewBag.StartDateSortParm = sortOrder == "startDate" ? "startDate_desc" : "startDate";
            ViewBag.EndDateSortParm = sortOrder == "endDate" ? "endDate_desc" : "endDate";
            ViewBag.TitleSortParam = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.PmSortParam = sortOrder == "pm" ? "pm_desc" : "pm";
            
            if (!String.IsNullOrEmpty(searchFreeStartDate))
            {
                searchStartDate = Convert.ToDateTime(searchFreeStartDate);
            }

            var ResourceLst = new List<string>();

            var ResourceQry = from d in db.Projects
                              orderby d.Resource
                              select d.Resource;

            ResourceLst.AddRange(ResourceQry.Distinct());
            ViewBag.projectResource = new SelectList(ResourceLst);
            ViewBag.AvailableResources = "";

            var ResourceNames = new List<string>();
            foreach (string name in ResourceQry.Distinct())
            {
                ResourceNames.Add(name);
            }
            ViewBag.ResourceNames = ResourceNames;

            var projs = from p in db.Projects
                        select p;

            switch(sortOrder)
            {
                case "resource_desc":
                    projs = projs.OrderByDescending(s => s.Resource);
                    break;
                case "startDate":
                    projs = projs.OrderBy(s => s.StartDate);
                    break;
                case "startDate_desc":
                    projs = projs.OrderByDescending(s => s.StartDate);
                    break;
                case "endDate":
                    projs = projs.OrderBy(s => s.EndDate);
                    break;
                case "endDate_desc":
                    projs = projs.OrderByDescending(s => s.EndDate);
                    break;
                case "pm":
                    projs = projs.OrderBy(s => s.PM);
                    break;
                case "pm_desc":
                    projs = projs.OrderByDescending(s => s.PM);
                    break;
                case "Title":
                    projs = projs.OrderBy(s => s.Title);
                    break;
                case "title_desc":
                    projs = projs.OrderByDescending(s => s.Title);
                    break;
                default:  // resource ascending 
                    projs = projs.OrderBy(s => s.Resource);
                    break;
            }
            //sdate going to need to think about a range block where
            //the ranges dont include search range.. maybe show a calendar
            //with resource availability on those dates.... implement icomparable and compareto obj dates

            var StartDateQry = from d in db.Projects
                               orderby d.StartDate
                               where d.StartDate == searchStartDate
                               select d.StartDate;

            if (!String.IsNullOrEmpty(searchString))
            {
                projs = projs.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(projectResource))
            {
                projs = projs.Where(x => x.Resource == projectResource);
            }

            if (!string.IsNullOrEmpty(searchFreeStartDate))
            {
                
                if (!string.IsNullOrEmpty(StartDateQry.ToString()))
                {
                    DateTime dateQuery = Convert.ToDateTime(searchFreeStartDate);
                    //want to do my linq to entities first as daterange cant be converted to sql obviously
                    var ent = from d in db.Projects
                        select d;
                    //now work on the record, use asenumerable to make it linq to objects, get the data a return cprojs
                    var cprojs = from p in ent.AsEnumerable().
                                Where(x => DateRange.DateInRange(x.StartDate, x.EndDate, dateQuery))
                            select p;

                    //just looking for distinct resource names at the moment who are full.
                    //need to get list of all names, do a compare and bring back free.
                    //cprojs = cprojs.GroupBy(p => p.Resource).Select(g => g.First());
                    List<string> fullList = new List<string>();
                    List<string> availableResources = new List<string>();

                    //this just brings back the names for all resources & populates fullList
                    foreach (string r in ResourceNames)  
                    {
                        if (!availableResources.Contains(r))
                        {
                            availableResources.Add(r);
                            //availableResources.Add(" : ");
                        }
                    }

                    //this brings back the names of resources used ie sam & eva and removes them from availableResources
                    foreach (Project name in cprojs)  
                    {
                        if (availableResources.Contains(name.Resource))
                        {
                            availableResources.Remove(name.Resource);
                            //availableResources.Remove(":");
                        }
                    }
                    ViewBag.AvailableResources = availableResources;

                    //return our new view
                    return View(cprojs);
                }
            }
            
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
        //[Authorize(Users="derriford\\colliers")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Users = "derriford\\colliers")]
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
        //[Authorize(Users = "derriford\\colliers")]
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
        //[Authorize(Users = "derriford\\colliers")]
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
        //[Authorize(Users = "derriford\\colliers")]
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
        //[Authorize(Users = "derriford\\colliers")]
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
