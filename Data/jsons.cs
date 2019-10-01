using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSLovers2.Data
{
    public class json_game
    {
        public string id { get; set; }
        //
        public string name { get; set; }

        public string url { get; set; }
        public DateTime release_date { get; set; }
        public string provider_name { get; set; }
        public IEnumerable<image> images { get; set; }
        public string game_contentType { get; set; }
        public IEnumerable<gameContentTypes> gameContentTypesList { get; set; }
        public bool is_suggested { get; set; }
        public IEnumerable<string> playable_platform { get; set; }
        public sku default_sku { get; set; }
    }

    public class star_rating
    {
        public string total { get; set; }
        public string score { get; set; }
        public IEnumerable<star_count> count { get; set; }
    }

    public class star_count
    {
        public int star { get; set; }
        public int count { get; set; }
    }

    public class json_game_detail : json_game
    {
        public string long_desc { get; set; }
        public mediaList mediaList { get; set; }
        public star_rating star_rating { get; set; }

    }

    public class mediaList
    {
        public IEnumerable<media> previews { get; set; }
        public IEnumerable<media> screenshots { get; set; }

    }

    public class media
    {
        public int typeId { get; set; }
        public string type { get; set; }
        public int order { get; set; }
        public string url { get; set; }
    }

    public class gameContentTypes
    {
        public string name { get; set; }
        public string key { get; set; }
    }

    public class image
    {
        public int type { get; set; }
        public string url { get; set; }
    }

    public class sku
    {
        public string display_price { get; set; }
        public int price { get; set; }
        public int[] platforms { get; set; }
        public string name { get; set; }
        public IEnumerable<reward> rewards { get; set; }
    }
    public class reward
    {
        public int discount { get; set; }
        public int price { get; set; }
        public string display_price { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
    }

    public class api_response
    {
        public api_categories categories { get; set; }
        public int total_results { get; set; }
    }

    public class api_games
    {
        public IEnumerable<json_game> links { get; set; }

    }

    public class api_categories
    {
        public api_games games { get; set; }
    }

    public class dealoftheweek_json
    {
        public Data_dotw data {get;set;} 
    }

    public class Data_dotw
    {
        public Relationship relationships { get; set; }
    }

    public class Relationship
    {
        public Children children { get; set; }
    }

    public class Children
    {
        public ICollection<InnerData> data { get; set; }
    }

    public class InnerData
    {
        public string id { get; set; }
        public string type { get; set; }
    }
}
