using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using Azure_0517.Models.Repository;
using System.IO;

namespace Azure_0517.Models.ViewModel
{
    public class MapObj
    {
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public int distance { get; set; }
        public int duration { get; set; }
        public Geometry geometry { get; set; }
        public int RestaurantID { get; set; }
    }

    public class Geometry
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class RestObj
    {
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public int PriceLevel { get; set; }
        public decimal? Rating { get; set; }
        public byte[] Photo { get; set; }
        public int? RestaurantCity { get; set; }
    }

    public class GoogleMapApi
    {

        string key = "AIzaSyC_WPox-HcTi5m-YQffFq59AavDZiapyVE";

        public IEnumerable<RestObj> GetNewRestaurants(string address)
        {
            CityRepository cityRepository = new CityRepository();
            List<RestObj> restObjs = new List<RestObj>();

            Geometry startPoint = this.Geolocation(address);
            string URL = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";
            StringBuilder urlParameters = new StringBuilder($"?location={startPoint.lat},{startPoint.lng}&radius=2000&type=restaurant&language=zh-TW");

            HttpResponseMessage response = GoogleApiResponse(URL, urlParameters.ToString(), key);

            var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll


            JObject jObject = JObject.Parse(dataObjects);

            var restList = jObject.SelectTokens(path: "$..results[*]");
            foreach (JToken token in restList)
            {

                RestObj restObj = new RestObj
                {
                    RestaurantName = token.SelectToken("$.name").ToString(),

                    RestaurantAddress = token.SelectToken("$.vicinity").ToString(),

                    Rating = token.SelectToken("$.rating") == null ?
                        (decimal?)null : Convert.ToDecimal(token.SelectToken("$.rating").ToString()),

                    PriceLevel = token.SelectToken("$.price_level") == null ?
                        0 : Convert.ToInt32(token.SelectToken("$.price_level").ToString()),

                    Photo = token.SelectToken("$.photos[0].photo_reference") == null ?
                        null : GooglePhotoApi(token.SelectToken("$.photos[0].photo_reference").ToString()),

                    RestaurantCity = token.SelectToken("$.plus_code.compound_code") == null ?
                        null : cityRepository.GetCityIDbyString(token.SelectToken("$.plus_code.compound_code").ToString().Split(' ')[1].Substring(0, 2)),
                };



                restObjs.Add(restObj);


            }


            return restObjs;

        }

        public Geometry Geolocation(string address = "大安站")
        {
            string startPoint = address;

            string URL = "https://maps.googleapis.com/maps/api/geocode/json";

            StringBuilder urlParameters = new StringBuilder($"?address={startPoint}");
            HttpResponseMessage response = this.GoogleApiResponse(URL, urlParameters.ToString(), key);
            var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll


            JObject jObject = JObject.Parse(dataObjects);
            var element = jObject.SelectTokens("$..location");
            Geometry geometry = new Geometry
            {
                lat = element.First().Values().First().ToString(),
                lng = element.First().Values().Last().ToString(),
            };


            return geometry;

        }

        public IEnumerable<MapObj> 
            NearbyRestaurants(IEnumerable<Restaurant> restaurants, string address = "大安站")
        {
            const int lib = 20;
            const int count = 6;
            restaurants = restaurants.ToList();
            string startPoint = address;
            string URL = "https://maps.googleapis.com/maps/api/distancematrix/json";
            List<MapObj> mapObjs = new List<MapObj>();
            List<Thread> threads = new List<Thread>();
            
            for (int i = 0; i <= (restaurants.Count()-1) / 20; i++)
            {
                Thread thread = new Thread(p =>
                {
                    StringBuilder urlParameters = new StringBuilder($"?units=imperial&origins={startPoint}&destinations=");
                    IEnumerable<Restaurant> restaurants_part = restaurants.Skip(20 * (int)p).Take(20).ToList();
                    foreach (var r in restaurants_part)
                    {
                        urlParameters.Append(r.Res_Address + "|");
                    }


                    urlParameters.Remove(urlParameters.Length - 1, 1);

                    HttpResponseMessage response = this.GoogleApiResponse(URL, urlParameters.ToString(), key);
                    var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll


                    JObject jObject = JObject.Parse(dataObjects);
                    var element = jObject.SelectTokens("$..elements[*]");
                    int x = 0;
                    foreach (var j in element)
                    {
                        Restaurant r = restaurants_part.Skip(x).FirstOrDefault();
                        MapObj mapObj = new MapObj
                        {
                            RestaurantAddress = r.Res_Address,
                            RestaurantName = r.Res_Name,
                            RestaurantID = r.Res_ID,
                            distance = j.SelectToken("$.distance.value").Value<int>(),
                            duration = j.SelectToken("$.duration.value").Value<int>(),
                            //distance = Convert.ToInt32(j.First().Values().Last().Values().First().ToString()),
                            //duration = Convert.ToInt32(j.First().Next.Values().Last().Values().First().ToString())
                        };
                        mapObjs.Add(mapObj);
                        x++;
                    }
                });
                thread.Start(i);
                threads.Add(thread);
            }

            foreach (var t in threads)
            {
                t.Join();
            }

            var result = mapObjs.OrderBy(L => L.distance).Take(lib).OrderBy(x => Guid.NewGuid()).Take(count).ToList();
           
            foreach (var r in result)
            {
                Thread thread = new Thread(() =>
                {
                    r.geometry = this.Geolocation(r.RestaurantAddress);
                });
                thread.Start();
                threads.Add(thread);
            }

            foreach (var t in threads)
            {
                t.Join();
            }

            return result;
        }

        public byte[] GooglePhotoApi(string photoreference)
        {
            string URL = "https://maps.googleapis.com/maps/api/place/photo";
            StringBuilder urlParameters = new StringBuilder($"?maxwidth=400&photoreference={photoreference}");
            urlParameters.Append("&key=AIzaSyC_WPox-HcTi5m-YQffFq59AavDZiapyVE");
            HttpResponseMessage response = GoogleApiResponse(URL, urlParameters.ToString(), key);

            var dataObjects = response.Content.ReadAsStreamAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
            MemoryStream ms = new MemoryStream();

            dataObjects.CopyTo(ms);
            byte[] Buffer = ms.ToArray();
            return Buffer;

        }

        public int? GoogleApiGetCity(string address)
        {
            string URL = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json";
            string urlParameters = $"?input={address}&inputtype=textquery&fields=plus_code&language=zh-TW";
            HttpResponseMessage response = GoogleApiResponse(URL, urlParameters, key);

            var dataObjects = response.Content.ReadAsStringAsync().Result;

            JObject jObject = JObject.Parse(dataObjects);
            var element = jObject.SelectTokens("$..compound_code").FirstOrDefault();
            CityRepository cityRepository = new CityRepository();
            return element == null ? null : cityRepository.GetCityIDbyString(element.Value<string>());
        }


        public HttpResponseMessage GoogleApiResponse(string URL, string urlParameters, string ApiKey)
        {

            urlParameters += $"&key={ApiKey}";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(urlParameters.ToString()).Result;
            return response;
        }

    }

}