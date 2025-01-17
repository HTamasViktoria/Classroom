using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Classroom.Model.DataModels
{
    public class Student : User
    {
        public DateTime BirthDate { get; init; }
        public string BirthPlace { get; init; }
        public string StudentNo { get; init; }

        public ICollection<Grade> Grades { get; init; } = new List<Grade>();
        public ICollection<NotificationBase> Notifications { get; init; } = new List<NotificationBase>();
    }
}