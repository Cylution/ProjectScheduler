using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace ProjectScheduler.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Title { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public string PM { get; set; }
        public string Resource{ get; set; }
    }
    public class ProjectDBContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
    }
}