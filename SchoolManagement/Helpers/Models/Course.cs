﻿using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Helpers.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Credits { get; set; }
        public string Instructor { get; set; }
        public string Department { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
