namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class DashboardStatsDto
    {
        public int TotalPatients { get; set; }
        public int TotalStaff { get; set; }
        public int TotalSessions { get; set; }
        public int LowStockMaterials { get; set; }
    }
}
