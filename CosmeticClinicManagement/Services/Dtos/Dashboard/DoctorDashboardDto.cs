namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class DoctorDashboardDto
    {
        public string DoctorName { get; set; } = string.Empty;
        public List<string> TodaySessions { get; set; } = new();
        public int PendingTreatmentPlans { get; set; }
    }
}
