using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PSLovers2.Data;
using PSLovers2.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace PSLovers2.Services
{
    public class FavoriteService : GenericService
    {
        private ApplicationDbContext Ctx { get; set; }
        private UserManager<IdentityUser> UserManager { get; set; }
        private FetchService FetchService { get; set; }
        private HashSet<(string GameId, int FavoriteId, string GameUrl)> CachedFavorites { get; set; }

        public FavoriteService(ApplicationDbContext context, UserManager<IdentityUser> manager, FetchService fetchService)
        {
            Ctx = context;
            UserManager = manager;
            FetchService = fetchService;
            CachedFavorites = new HashSet<(string GameId, int FavoriteId, string GameUrl)>();
        }

        private (string, int, string) ReturnTupleFromFav(Favorite favorite) => (favorite.GameId, favorite.Id, favorite.GameUrl);

        public async ValueTask<HashSet<(string GameId, int FavoriteId, string GameUrl)>> FavoriteForUser(string email)
        {

            IdentityUser user = await UserManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user != null)
            {
                if (CachedFavorites == null || CachedFavorites.Count == 0)
                {
                    var tmp = Ctx.FavoriteForUsers.Include(x=>x.Favorite).Where(x => x.UserId == user.Id).ToList();
                    CachedFavorites = tmp.Select(x => ReturnTupleFromFav(x.Favorite)).ToHashSet();
                }
            }
            return CachedFavorites;
        }

        public async Task<ServiceOutput<int>> IsFavorite(string gameId, string email)
        {

            HashSet<(string GameId, int FavoriteId, string GameUrl)> favorites = await FavoriteForUser(email).ConfigureAwait(false);
            var output = new ServiceOutput<int>();
            try
            {
                output.Result = -1;
                if (favorites.Any(x => x.GameId == gameId))
                {
                    output.Result = favorites.First(x => x.GameId == gameId).FavoriteId;
                }
            }
            catch (Exception e)
            {
                ServiceFailed(output, e);
            }
            return output;

        }


        public async Task<ServiceOutput<int>> AddFavorite(string email, string gameId, string gameUrl, string lang, string country)
        {
            var output = new ServiceOutput<int>();
            try
            {
                IdentityUser user = await UserManager.FindByEmailAsync(email).ConfigureAwait(false);
                FetchService.Initialize(country, lang, 2);
                ServiceOutput<GameDetail> game = await FetchService.FetchGameDetails(gameId, gameUrl).ConfigureAwait(false);

                int favId;
                Favorite favorite;
                if (!Ctx.Favorites.Any(x => x.GameId == game.Result.Id && x.ApiCountry == country && x.ApiLanguage == lang))
                {
                    favorite = new Favorite(game.Result, lang, country);
                    Ctx.Favorites.Add(favorite);
                    await Ctx.SaveChangesAsync().ConfigureAwait(false);
                    favId = favorite.Id;
                }
                else
                {
                    favorite = Ctx.Favorites.FirstOrDefault(x => x.GameId == game.Result.Id && x.ApiCountry == country && x.ApiLanguage == lang);
                    favId = favorite.Id;
                }
                int favForUserId;
                if(!Ctx.FavoriteForUsers.Any(x=>x.UserId==user.Id && x.FavoriteId == favId))
                {
                    var ffu = new FavoriteForUser()
                    {
                        FavoriteId = favId,
                        LastUpdateTime = DateTime.Now,
                        UserId = user.Id
                    };
                    Ctx.FavoriteForUsers.Add(ffu);
                    await Ctx.SaveChangesAsync().ConfigureAwait(false);
                    favForUserId = ffu.Id;
                }
                else
                {
                    favForUserId = Ctx.FavoriteForUsers.FirstOrDefault(x => x.UserId == user.Id && x.FavoriteId == favId).Id;
                }
                
                output.Result = favForUserId;
                CachedFavorites.Add(ReturnTupleFromFav(favorite));

                

            }
            catch (Exception e)
            {
                ServiceFailed(output, e);
            }
            return output;
        }

        public async Task<ServiceOutput<bool>> RemoveFavorite(string email, int favoriteId)
        {
            var output = new ServiceOutput<bool>();
            try
            {
                IdentityUser user = await UserManager.FindByEmailAsync(email).ConfigureAwait(false);
                FavoriteForUser favorite = await Ctx.FavoriteForUsers.FindAsync(favoriteId).ConfigureAwait(false);
                (string, int, string) tmp = ReturnTupleFromFav(favorite.Favorite);

                if (favorite.UserId == user.Id)
                {
                    Ctx.FavoriteForUsers.Remove(favorite);
                    await Ctx.SaveChangesAsync().ConfigureAwait(false);
                    if (CachedFavorites.Contains(tmp))
                    {
                        CachedFavorites.Remove(tmp);
                    }

                }
                else
                {
                    ServiceFailed(output, "This favorite is not yours");
                }
            }
            catch (Exception e)
            {
                ServiceFailed(output, e);
            }
            return output;
        }

        public async Task<ServiceOutput<bool>> SwapFavoriteOwned(string email, int favoriteId)
        {
            var output = new ServiceOutput<bool>();
            try
            {
                IdentityUser user = await UserManager.FindByEmailAsync(email).ConfigureAwait(false);
                FavoriteForUser favorite = await Ctx.FavoriteForUsers.FindAsync(favoriteId).ConfigureAwait(false);
                if (favorite.UserId == user.Id)
                {
                    favorite.Owned = !favorite.Owned;
                    await Ctx.SaveChangesAsync().ConfigureAwait(false);
                    output.Result = favorite.Owned;
                }
                else
                {
                    ServiceFailed(output, "This favorite is not yours");
                }
            }
            catch (Exception e)
            {
                ServiceFailed(output, e);
            }
            return output;
        }

        public async Task<ServiceOutput<List<Favorite>>> CompleteFavoriteListForUser(string email, string lang, string country)
        {
            FetchService.Initialize(country, lang);
            var output = new ServiceOutput<List<Favorite>>();
            try
            {
                HashSet<(string GameId, int FavoriteId, string GameUrl)> idList = await FavoriteForUser(email).ConfigureAwait(false);
                IdentityUser user = await UserManager.FindByEmailAsync(email).ConfigureAwait(false);
                var gameList = new List<Favorite>();
                foreach ((var GameId, var FavoriteId, var GameUrl) in idList)
                {
                    ServiceOutput<GameDetail> serviceOutput = await FetchService.FetchGameDetails(GameId, GameUrl).ConfigureAwait(false);
                    if (serviceOutput.Success)
                        gameList.Add(new Favorite(serviceOutput.Result, lang, country));
                }
                output.Result = gameList;
            }
            catch (Exception e)
            {
                ServiceFailed(output, e);
            }
            return output;
        }

        public async Task<bool> UpdateFavorite(string gameId, GameDetail game, string country, string lang)
        {
            var ora = DateTime.UtcNow;
            var favs = Ctx.Favorites.Where(x => x.GameId == gameId && x.ApiCountry == country && x.ApiLanguage == lang);
            foreach (var fav in favs)
            {
                fav.GameName = game.Name;
                fav.GameUrl = game.Url;
                fav.GameCurrentPrice = game.DiscountedPrice ?? game.Price;
                fav.DiscountPercentage = game.DiscountPercentage;
                fav.GameCurrentFullPrice = fav.DiscountPercentage > 0 ? game.FullPrice : game.FullDiscountedPrice;
                //GamePastPrice = game.Price;
                //GamePastFullPrice = game.FullPrice;
                fav.GameImage = game.Image;
                fav.GamePlatforms = game.Platforms;
                //ApiLanguage = lang;
                //ApiCountry = country;
                //UserId = userId;
                fav.DiscountedUntil = game.DiscountedUntil;
            }
            await Ctx.SaveChangesAsync();
            return true;
        }
        

    }
}
