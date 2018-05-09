using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AnimeCollections.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AnimeCollections.Data;
using AnimeCollections.Authorization;

namespace AnimeCollections.Pages.Animes
{
    public class IndexModel : DI_BasePageModel
    {

        public IndexModel(
         ApplicationDbContext context,
         IAuthorizationService authorizationService,
         UserManager<ApplicationUser> userManager)
         : base(context, authorizationService, userManager)
        {
        }

        public IList<Anime> Animes { get; set; }
        public SelectList Genres { get; set; }
        public string AnimeGenre { get; set; }


        public string RatingSort { get; set; }
        public string DateSort { get; set; }
        public string PriceSort { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }


        public PaginatedList<Anime> Anime { get; set; }
        public async Task OnGetAsync(string AnimeGenre, string searchString, string sortOrder, string currentFilter, int? pageIndex)
        {
            #region Sort
            CurrentSort = sortOrder;
            //so excellent good great hot will be this order
            RatingSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            PriceSort = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            CurrentFilter = searchString;
#endregion
            #region genre and name
            IQueryable<string> genreQuery = from a in Context.Anime
                                            orderby a.Genre
                                            select a.Genre;

            var AnimeList = from a in Context.Anime
                            select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                AnimeList = AnimeList.Where(s => s.Title.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(AnimeGenre))
            {
                AnimeList = AnimeList.Where(x => x.Genre == AnimeGenre);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Animes = await AnimeList.ToListAsync();
            #endregion


            var isAuthorized = User.IsInRole(Constants.ContactManagersRole) ||
                           User.IsInRole(Constants.ContactAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            // Only Allowed contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                AnimeList = AnimeList.Where(c => c.Status == Status.Allowed
                                            || c.OwnerID == currentUserId);
            }
            Animes = await AnimeList.ToListAsync();

            switch (sortOrder)
            {
                case "name_desc":
                    AnimeList = AnimeList.OrderByDescending(s => s.Rating);
                    break;
                case "Date":
                    AnimeList = AnimeList.OrderBy(s => s.ReleaseDate);
                    break;
                case "date_desc":
                    AnimeList = AnimeList.OrderByDescending(s => s.ReleaseDate);
                    break;
                case "price_desc":
                    AnimeList = AnimeList.OrderByDescending(s => s.Price);
                    break;
                default:
                    AnimeList = AnimeList.OrderBy(s => s.Rating);
                    break;
            }

            Animes = await AnimeList.AsNoTracking().ToListAsync();


            int pageSize = 4;
            Anime = await PaginatedList<Anime>.CreateAsync(
                AnimeList.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }


}
