using ParcingLogic.Models;

namespace ParcingLogic
{
    public class DbController
    {
        public List<Subject> Subjects 
        {
            get
            {
                using var db = new ScheduleDbContext();

                return db.Subjects.ToList();
            } 
        }

        private PgupsScedule _schedule; 

        public DbController()
        {
            _schedule = new PgupsScedule();
        }

        public void StartUpdating(int UpdateRateSeconds)
        {
            while (true)
            {
                Update();

                Task.Delay(UpdateRateSeconds * 1000);
            }
        }

        public void Update()
        {
            using var db = new ScheduleDbContext();

            db.RemoveRange(db.Subjects);
            db.Subjects.AddRange(_schedule.GetAllSubjects());
            db.SaveChanges();

            Console.WriteLine("Table Updated");
        }
    }
}
