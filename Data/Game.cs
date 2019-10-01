using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSLovers2.Data
{
    public class Game
    {
        private json_game JsonGame { get; set; }
        public string Id { get { return JsonGame.id; } }
        public string Name { get { return JsonGame.name; } }
        public virtual string Url { get { return JsonGame.url; } }
        public DateTime ReleaseDate { get { return JsonGame.release_date; } }
        public string ProviderName { get { return JsonGame.provider_name; } }
        public string GameContentType { get { return JsonGame.game_contentType; } }
        public string GameContentKey { get { return JsonGame?.gameContentTypesList?.Any() == true ? String.Join(", ", JsonGame?.gameContentTypesList?.Select(x => x.key)) : ""; } }
        public string Price { get { return JsonGame?.default_sku?.display_price; } }
        public int FullPrice { get { return JsonGame?.default_sku?.price ?? 0; } }
        public int FullDiscountedPrice { get { return JsonGame?.default_sku?.rewards?.FirstOrDefault(x => x.start_date <= DateTime.Now && x.end_date >= DateTime.Now)?.price ?? 0; } }
        public virtual string Image { get { return $"{Url}/image?w=480&h=480"; } }
        public string Platforms { get { return String.Join(", ", JsonGame?.playable_platform); } }
        public int DiscountPercentage { get { return JsonGame?.default_sku?.rewards?.FirstOrDefault(x => x.start_date <= DateTime.Now && x.end_date >= DateTime.Now)?.discount ?? 0; } }
        public string DiscountedPrice { get { return JsonGame?.default_sku?.rewards?.FirstOrDefault(x => x.start_date <= DateTime.Now && x.end_date >= DateTime.Now)?.display_price; } }
        public DateTime? DiscountedUntil { get { return JsonGame?.default_sku?.rewards?.FirstOrDefault(x => x.start_date <= DateTime.Now && x.end_date >= DateTime.Now)?.end_date; } }
        public Game(json_game game) => JsonGame = game;
     

    }

}
