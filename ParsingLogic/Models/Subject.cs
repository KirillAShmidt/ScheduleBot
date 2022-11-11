using System.ComponentModel.DataAnnotations;

namespace ParcingLogic.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string? GroupName { get; set; } 
        public string? SubjectName { get; set; }
        public int SubjectNumber { get; set; }
        public string? Time { get; set; }
        public string? Room { get; set; }
        public string? DayOfWeek { get; set; }
    }
}
