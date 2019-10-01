using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace PSLovers2.Data
{
    public class Favorite
    {
        public int Id { get; set; }
        public string GameId { get; set; }
        public string GameName { get; set; }
        public string GameUrl { get; set; }
        public string GameCurrentPrice { get; set; }
        public int GameCurrentFullPrice { get; set; }
        public string GamePastPrice { get; set; }
        public int GamePastFullPrice { get; set; }
        public string GameImage { get; set; }
        public string GamePlatforms { get; set; }
        public string ApiLanguage { get; set; }
        public string ApiCountry { get; set; }
        public int DiscountPercentage { get; set; }       
        public DateTime? DiscountedUntil { get; set; }
        public virtual ICollection<IdentityUser> Users { get; set; }
        

        public Favorite()
        {

        }
        public Favorite(GameDetail game, string lang, string country)
        {
            GameId = game.Id;
            GameName = game.Name;
            GameUrl = game.Url;
            GameCurrentPrice = game.DiscountedPrice ?? game.Price;
            DiscountPercentage = game.DiscountPercentage;
            GameCurrentFullPrice = DiscountPercentage > 0 ? game.FullPrice : game.FullDiscountedPrice;
            GamePastPrice = game.Price;
            GamePastFullPrice = game.FullPrice;
            GameImage = game.Image;
            GamePlatforms = game.Platforms;
            ApiLanguage = lang;
            ApiCountry = country;            
            DiscountedUntil = game.DiscountedUntil;
        }

    }


}
