using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSLovers2.Data
{
    public class GameDetail : Game
    {
        private json_game_detail _game { get; set; }
        private string _url { get; set; }
        public override string Url { get { return _url; } }
        public override string Image { get { return $"{Url}/image?w=480&h=480"; } }
        public string Desc { get { return _game?.long_desc; } }
        public string Preview { get { return _game?.mediaList?.previews?.OrderBy(x => x.order).FirstOrDefault()?.url; } }
        public IEnumerable<string> Screenshots { get { return _game?.mediaList?.previews?.Select(x => x.url); } }

        public string StarCountTotal { get { return _game?.star_rating?.total; } }
        public string StarAverageScore { get { return _game?.star_rating?.score; } }

        public IEnumerable<(int, int)> Stars { get { return _game?.star_rating?.count.Select(x => (x.star, x.count)); } }


        public GameDetail(json_game_detail json, string url) : base(json)
        {
            _game = json;
            _url = url;
        }
    }




}
