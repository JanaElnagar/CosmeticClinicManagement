namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class ReceptionistDashboardDto
    {
        public int TotalPatients { get; set; }
        public int TotalTreatmentPlans { get; set; }
        public int UpcomingSessions { get; set; }
        public int CompletedSessions { get; set; }
        public int CancelledSessions { get; set; }
    }
}