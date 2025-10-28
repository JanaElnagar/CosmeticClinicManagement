namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class AdminDashboardDto
    {
        public long TotalPatients { get; set; }
        public long TotalDoctors { get; set; }
        public long ActiveTreatmentPlans { get; set; }
        public long ClosedTreatmentPlans { get; set; }
        public decimal TotalRevenue { get; set; }
        
    }
}
