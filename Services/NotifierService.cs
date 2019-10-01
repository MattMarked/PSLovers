using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
    public class NotifierService : GenericService
    {
        private ApplicationDbContext Ctx { get; set; }
        private UserManager<IdentityUser> UserManager { get; set; }
        private FetchService FetchService { get; set; }
        private FavoriteService FavoriteService { get; set; }
        private IEmailSender EmailSender { get; set; }

        public NotifierService(ApplicationDbContext context, UserManager<IdentityUser> manager, FetchService fetchService, FavoriteService favoriteService, IEmailSender emailSender)
        {
            Ctx = context;
            UserManager = manager;
            FetchService = fetchService;
            FavoriteService = favoriteService;
            EmailSender = emailSender;
        }

        public async Task<int> TestNotifyUser(string email)
        {
            var result = 1;

            var user = await UserManager.FindByEmailAsync(email);
            if(user != null)
            {
                //var favIds = Ctx.FavoriteForUsers.Where(x => x.UserId == user.Id).Select(x=>new { x.GameId, x.GameUrl, x.ApiCountry, x.ApiLanguage });
                //foreach (var item in favIds)
                //{
                    
                //}

            }

            return result;
        }


    }
}
