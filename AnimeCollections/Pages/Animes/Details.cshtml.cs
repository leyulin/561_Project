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
    public class DetailsModel : DI_BasePageModel
    {

        public DetailsModel(
          ApplicationDbContext context,
          IAuthorizationService authorizationService,
          UserManager<ApplicationUser> userManager)
          : base(context, authorizationService, userManager)
        {
        }

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
            return Page();
        }



        public async Task<IActionResult> OnPostAsync(int id, Status status)
        {
            var Anime = await Context.Anime.FirstOrDefaultAsync(
                                                      m => m.ID == id);

            if (Anime == null)
            {
                return NotFound();
            }

            var contactOperation = (status == Status.Allowed)
                                                       ? Operations.Approve
                                                       : Operations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Anime,
                                        contactOperation);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }
            Anime.Status = status;
            Context.Anime.Update(Anime);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

    