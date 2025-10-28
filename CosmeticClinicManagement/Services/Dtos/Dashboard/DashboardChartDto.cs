namespace CosmeticClinicManagement.Services.Dtos.Dashboard
{
    public class DashboardChartDto
    {
        public List<string> Labels { get; set; } = new();
        public List<int> Values { get; set; } = new();
    }
}
