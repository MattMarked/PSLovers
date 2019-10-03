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
    public class FetchService : GenericService
    {
        private Dictionary<string, Tuple<DateTime, GameDetail>> CachedGameDetails { get; set; }
        private string Url { get; set; }
        private string Country { get; set; }
        private string Lang { get; set; }
        private int ContentType { get; set; }



        public FetchService() => CachedGameDetails = new Dictionary<string, Tuple<DateTime, GameDetail>>();
        public void Initialize(string country, string lang, int? contentType = null)
        {
            Country = country;
            Lang = lang;
            ContentType = contentType ?? 2;
        }
        public void CheckLanguageAndCountry()
        {
            if (String.IsNullOrEmpty(Country) || String.IsNullOrEmpty(Lang))
                throw new Exception("Initialize service First");
        }
        public async Task<ServiceOutput<GameDetail>> FetchGameDetails(string gameId, string gameUrl)
        {
            var output = new ServiceOutput<GameDetail>();
            if (CachedGameDetails.ContainsKey(gameId) && (CachedGameDetails[gameId].Item1.AddMinutes(15) > DateTime.UtcNow))
            {
                output.Result = CachedGameDetails[gameId].Item2;
            }
            else
            {
                Url = $"https://store.playstation.com/store/api/chihiro/00_09_000/container/{Country}/{Lang}/19/";
                
                using var httpClient = new HttpClient() { BaseAddress = new Uri(Url) };
                try
                {
                    CheckLanguageAndCountry();

                    var encoded = HttpUtility.UrlEncode(gameId);
                    HttpResponseMessage request = await httpClient.GetAsync($"{encoded}");
                    var payload = await request.Content.ReadAsStringAsync();
                    if (request.IsSuccessStatusCode)
                    {
                        json_game_detail response = JsonConvert.DeserializeObject<json_game_detail>(payload);
                        output.Result = new GameDetail(response, Url+encoded);
                        if (CachedGameDetails.ContainsKey(gameId))
                        {
                            CachedGameDetails[gameId] = new Tuple<DateTime, GameDetail>(DateTime.UtcNow, output.Result);
                        }
                        else
                        {
                            CachedGameDetails.Add(gameId, new Tuple<DateTime, GameDetail>(DateTime.UtcNow, output.Result));

                        }
                    }
                    else
                    {
                        ServiceFailed(output, $"API call failed: {payload}");
                    }
                }
                catch (Exception ex)
                {
                    ServiceFailed(output, ex);
                }
            }

            return output;
        }
        public async Task<ServiceOutput<IEnumerable<Game>>> FetchGames(string query)
        {
            Url = "https://store.playstation.com/chihiro-api/bucket-search/" + Country + "/" + Lang + "/19/";
            var output = new ServiceOutput<IEnumerable<Game>>();  
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(Url) })
            {
                try
                {
                    CheckLanguageAndCountry();
                    var encoded = HttpUtility.UrlEncode(query);
                    HttpResponseMessage request = await httpClient.GetAsync($"{encoded}?size=9999&start=0");
                    var payload = await request.Content.ReadAsStringAsync();
                    if (request.IsSuccessStatusCode)
                    {
                        api_response response = JsonConvert.DeserializeObject<api_response>(payload);
                        output.Result = response?.categories?.games?.links.Select(x => new Game(x));
                        if (ContentType == 2)
                        {
                            output.Result = output.Result.Where(x => (x.GameContentKey?.ToUpper().Contains("GAME") == true || x.GameContentType?.ToUpper().Contains("BUNDLE") == true) && x.GameContentType?.ToUpper().Contains("VIDEO") == false);
                        }
                        if (ContentType == 3)
                        {
                            output.Result = output.Result.Where(x => (x.GameContentKey?.ToUpper().Contains("GAME") == false || x.GameContentType?.ToUpper().Contains("VIDEO") == true) && x.GameContentType?.ToUpper().Contains("BUNDLE") == false);
                        }
                    }
                    else
                    {
                        ServiceFailed(output, $"API call failed: {payload}");
                    }
                }
                catch (Exception ex)
                {
                    ServiceFailed(output, ex);
                } 
            }
            return output;
        }
        public async Task<IEnumerable<Game>> YieldGameDetail(Data_dotw data)
        {
            var output = new List<Game>();
            foreach (var d in data.relationships?.children?.data)
            {
                var tmp = await (FetchGameDetails(d.id, "").ConfigureAwait(false));
                if (tmp.Success)
                {
                    output.Add(tmp.Result);
                }
            }
            return output;
        }
        public async Task<ServiceOutput<IEnumerable<Game>>> DealsOfTheWeek()
        {
            var output = new ServiceOutput<IEnumerable<Game>>();

            Url = "https://store.playstation.com/valkyrie-api/" + Country + "/" + Lang + "/19/container/STORE-MSF75508-DOTW1";
            using var httpClient = new HttpClient() { BaseAddress = new Uri(Url) };
            try
            {
                CheckLanguageAndCountry();
                HttpResponseMessage request = await httpClient.GetAsync($"?size=30&bucket=games&start=0");
                var payload = await request.Content.ReadAsStringAsync();
                if (request.IsSuccessStatusCode)
                {
                    dealoftheweek_json response = JsonConvert.DeserializeObject<dealoftheweek_json>(payload);
                    if(response != null)
                    {
                        output.Result = await YieldGameDetail(response.data).ConfigureAwait(false);
                    }
                    else
                    {
                        ServiceFailed(output, $"API call failed: {payload}");
                    }
                    //output.Result = new GameDetail(response, gameUrl);
                    //CachedGameDetails.Add(gameId, new Tuple<DateTime, GameDetail>(DateTime.UtcNow, output.Result));
                }
                else
                {
                    ServiceFailed(output, $"API call failed: {payload}");
                }
            }
            catch (Exception ex)
            {
                ServiceFailed(output, ex);
            }


            return output;
        }
        public async Task<ServiceOutput<IEnumerable<Game>>> LastPriceDrops()
        {
            var output = new ServiceOutput<IEnumerable<Game>>();

            Url = "https://store.playstation.com/valkyrie-api/" + Country + "/" + Lang + "/19/container/STORE-MSF75508-PRICEDROPSCHI";
            using var httpClient = new HttpClient() { BaseAddress = new Uri(Url) };
            try
            {
                CheckLanguageAndCountry();
                HttpResponseMessage request = await httpClient.GetAsync($"?size=30&bucket=games&start=0");
                var payload = await request.Content.ReadAsStringAsync();
                if (request.IsSuccessStatusCode)
                {
                    dealoftheweek_json response = JsonConvert.DeserializeObject<dealoftheweek_json>(payload);
                    if (response != null)
                    {
                        output.Result = await YieldGameDetail(response.data).ConfigureAwait(false);
                    }
                    else
                    {
                        ServiceFailed(output, $"API call failed: {payload}");
                    }
                    //output.Result = new GameDetail(response, gameUrl);
                    //CachedGameDetails.Add(gameId, new Tuple<DateTime, GameDetail>(DateTime.UtcNow, output.Result));
                }
                else
                {
                    ServiceFailed(output, $"API call failed: {payload}");
                }
            }
            catch (Exception ex)
            {
                ServiceFailed(output, ex);
            }


            return output;
        }
    }
}
