using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CosmeticClinicManagement.Services.Dtos.Store;
using CosmeticClinicManagement.Services.Interfaces;

namespace CosmeticClinicManagement.Pages.Stores.RawMaterials
{
    public class CreateModel : PageModel
    {
        private readonly IStoreAppService _storeAppService;

        [BindProperty]
        public CreateRawMaterialDto Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public Guid StoreId { get; set; }

        public CreateModel(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        public void OnGet(Guid storeId)
        {
            StoreId = storeId;
            Input.StoreId = storeId;
            Input.ExpiryDate = DateTime.Now.AddMonths(6);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _storeAppService.CreateRawMaterialAsync(Input);
            return RedirectToPage("/Stores/Details", new { id = Input.StoreId });
        }
    }
}   