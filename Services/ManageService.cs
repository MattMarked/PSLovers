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
    public class ManageService : GenericService
    {
        private ApplicationDbContext Ctx { get; set; }
        private UserManager<IdentityUser> UserManager { get; set; }
        private FetchService FetchService { get; set; }
        private HashSet<(string GameId, int FavoriteId, string GameUrl)> CachedFavorites { get; set; }

        public ManageService(ApplicationDbContext context, UserManager<IdentityUser> manager, FetchService fetchService)
        {
            Ctx = context;
            UserManager = manager;
            FetchService = fetchService;
            CachedFavorites = new HashSet<(string GameId, int FavoriteId, string GameUrl)>();
        }

        public async Task<ServiceOutput<IEnumerable<IdentityUser>>> GetAllUser()
        {
            var output = new ServiceOutput<IEnumerable<IdentityUser>>();
            output.Result = await UserManager.Users.ToListAsync().ConfigureAwait(false);
            return output;
        }

        public async Task<ServiceOutput<bool>> DeleteUser(string userId)
        {
            var output = new ServiceOutput<bool>();
            try
            {
                var userToRemove = await UserManager.FindByIdAsync(userId).ConfigureAwait(false);
                Ctx.FavoriteForUsers.RemoveRange(Ctx.FavoriteForUsers.Where(x => x.UserId == userToRemove.Id));
                await Ctx.SaveChangesAsync();
                await UserManager.DeleteAsync(userToRemove);
            }
            catch (Exception e)
            {
                ServiceFailed(output, e);
            }
            return output;
        }

     
        

    }
}
