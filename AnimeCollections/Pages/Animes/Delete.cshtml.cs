using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AnimeCollections.Models;
using AnimeCollections.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AnimeCollections.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AnimeCollections.Pages.Animes
{
    public class DeleteModel : DI_BasePageModel
    {
 

        public DeleteModel(
          ApplicationDbContext context,
          IAuthorizationService authorizationService,
          UserManager<ApplicationUser> userManager)
          : base(context, authorizationService, userManager)
        {
        }


        [BindProperty]
        public Anime Anime { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Anime = await Context.Anime.SingleOrDefaultAsync(m => m.ID == id);

            if (Anime == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                    User, Anime,
                                    Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Anime = await Context.Anime.FindAsync(id);

            var anime = await Context
            .Anime.AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

            if (anime == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                          User, anime,
                          Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }


                Context.Anime.Remove(Anime);
                await Context.SaveChangesAsync();
  

            return RedirectToPage("./Index");
        }
    }
}
