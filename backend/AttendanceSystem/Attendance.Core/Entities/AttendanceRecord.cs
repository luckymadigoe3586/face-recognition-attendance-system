using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Entities
{

    public class AttendanceRecord
    {
        public int AttendanceRecordId { get; set; }
        public int EmployeeId { get; set; }

        public DateTime CheckInTime { get; set; }
        public DateOnly AttendanceDate { get; set; }

        public string Status { get; set; } = string.Empty;
        public double ConfidenceScore { get; set; }

        public Employee? Employee { get; set; }
    }
}
