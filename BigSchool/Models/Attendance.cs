using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace BigSchool.Models
{
    public class Attendance 
    {
        // GET: Attendance
        public Course Course { get; set; }

        [Key]
        [Column(Order = 1)]
        public int CourseId { get; set; }

        public Application Attendee { get; set; }

        [Key]
        [Column(Order = 2)]
        public string AttendeeId { get; set; }
    }
}