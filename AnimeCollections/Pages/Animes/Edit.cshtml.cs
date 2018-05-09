using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EditModel : DI_BasePageModel
    {

        public EditModel(
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
                                                Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Context.Attach(Anime).State = EntityState.Modified;


            #region new edited
            // Fetch Anime from DB to get OwnerID.
            var anime = await Context
                .Anime.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (anime == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                 User, Anime,
                                                 Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            Anime.OwnerID = anime.OwnerID;

            Context.Attach(Anime).State = EntityState.Modified;

            if (anime.Status == Status.Allowed)
            {
                // If the Anime is updated after approval, 
                // and the user cannot approve,
                // set the status back to Uploaded so the update can be
                // checked and Allowed.
                var canApprove = await AuthorizationService.AuthorizeAsync(User,
                                        anime,
                                        Operations.Approve);

                if (!canApprove.Succeeded)
                {
                    Anime.Status = Status.Uploaded;
                }
            }
#endregion



            try
            {
                await Context.SaveChangesAsync();
                

            }
            catch (DbUpdateConcurrencyException)
            {

            }

            await Context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        private bool ContactExists(int id)
        {
            return Context.Anime.Any(e => e.ID == id);
        }
    }
}
