namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class DashboardStatsDto
    {
        public long TotalPatients { get; set; }
        public long TotalStaff { get; set; }
        public long TotalSessions { get; set; }
        public long LowStockMaterials { get; set; }
    }
}
