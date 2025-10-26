using CosmeticClinicManagement.Services;
using CosmeticClinicManagement.Services.Dtos.Store;
using CosmeticClinicManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CosmeticClinicManagement.Pages.Stores.RawMaterials
{
    public class DeleteModel : PageModel
    {
        private readonly IStoreAppService _storeAppService;

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid StoreId { get; set; }

        public RawMaterialDto? RawMaterial { get; set; }

        public DeleteModel(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id, Guid storeId)
        {
            Id = id;
            StoreId = storeId;

            try
            {
                RawMaterial = await _storeAppService.GetRawMaterialAsync(id);
            }
            catch
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _storeAppService.DeleteRawMaterialAsync(Id);
            return RedirectToPage("/Stores/Details", new { id = StoreId });
        }
    }
}