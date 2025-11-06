//using AutoMapper.Internal.Mappers;
//using CosmeticClinicManagement.Pages.Sessions;
//using CosmeticClinicManagement.Services.Dtos;
//using CosmeticClinicManagement.Services.Implementation;
//using CosmeticClinicManagement.Services.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
//using Volo.Abp.ObjectMapping;

//namespace CosmeticClinicManagement.Pages.Sessions
//{
//    public class EditSessionModalModel : AbpPageModel
//    {
//        [HiddenInput]
//        [BindProperty(SupportsGet = true)]
//        public Guid Id { get; set; }
//        [BindProperty]
//        public CreateEditSessionViewModel Session { get; set; }
//        [BindProperty]
//        public string NotesText { get; set; }
//        // public SelectListItem[] Categories { get; set; }
//        private readonly ISessionAppService _sessionAppService;
//        public EditSessionModalModel(ISessionAppService sessionAppService)
//        {
//            _sessionAppService = sessionAppService;
//        }
//        public async Task OnGetAsync()
//        {
//            var sessionDto = await _sessionAppService.GetAsync(Id);
//            Session = ObjectMapper.Map<SessionDto, CreateEditSessionViewModel>(sessionDto);

//            NotesText = sessionDto.Notes != null ? string.Join("\n", sessionDto.Notes) : string.Empty;
//        }
//        public async Task<IActionResult> OnPostAsync()
//        {
//            if (!string.IsNullOrWhiteSpace(NotesText))
//                Session.Notes = NotesText.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
//            else
//                Session.Notes = new List<string>();
//            // TODO
//            await _sessionAppService.UpdateAsync(Id,
//                ObjectMapper.Map<CreateEditSessionViewModel, UpdateSessionDto>(Session)
// );
//            return NoContent();
//        }
//    }
//}
