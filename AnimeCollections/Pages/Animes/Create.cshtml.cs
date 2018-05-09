
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AnimeCollections.Models;
using AnimeCollections.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AnimeCollections.Authorization;

namespace AnimeCollections.Pages.Animes
{
    public class CreateModel : DI_BasePageModel
    {

        public CreateModel(
            ApplicationDbContext context,
         IAuthorizationService authorizationService,
         UserManager<ApplicationUser> userManager)
            : base(context, authorizationService, userManager) { }

    

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Anime Anime { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Anime.OwnerID = UserManager.GetUserId(User);
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                User, Anime,
                                                Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            Context.Anime.Add(Anime);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}