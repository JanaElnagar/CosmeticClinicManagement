namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class ReceptionistDashboardDto
    {
        public int NewPatientsThisWeek { get; set; }
        public int UpcomingAppointments { get; set; }
        public int CancelledSessions { get; set; }
    }
}
