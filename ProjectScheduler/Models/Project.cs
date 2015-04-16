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

        [Required(ErrorMessage="You must enter a {0}")]
        [Display(Name = "Project Name")]
        public string Title { get; set; }

        [Display(Name = "Start Date")]
        //[Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        
        [Display(Name = "Project Manager")]        
        public string PM { get; set; }
        
        //[Required]
        public string Resource{ get; set; }
        
        [StringLength(40, ErrorMessage = "Notes cannot be longer than 40 characters.")]
        public string Notes { get; set; }

    }

    public class ProjectDBContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
    }
}