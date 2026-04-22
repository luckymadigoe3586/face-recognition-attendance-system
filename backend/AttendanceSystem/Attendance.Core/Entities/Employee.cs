using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;

        public string FaceImagePath { get; set; } = string.Empty;
        public string FaceEncodingPath { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
        //public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = [];
    }
}
