using lab.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace lab.ViewModel
{
    public class CourseViewModel
    {
        public int id { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        [FurtureDate]
        public string Date { get; set; }
        [Required]
        [VaildTime]
        public string Time { get; set; }
        [Required]
        public byte Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0} ,{1}", Date, Time));
        }
    }
}