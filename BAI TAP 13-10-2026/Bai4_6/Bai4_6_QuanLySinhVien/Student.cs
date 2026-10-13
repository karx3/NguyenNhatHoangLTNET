using System;

namespace Bai4_6
{
    public class Student
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string AvatarPath { get; set; }

        public Student()
        {
            StudentId = "";
            FullName = "";
            Age = 0;
            Gender = "Nam";
            AvatarPath = "";
        }
    }
}
