using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Azure_0517.Models;
using Azure_0517.Models.Repository;
using Azure_0517.Models.ViewModel;
using System.Threading;

namespace Azure_0517.Controllers
{
    public class HomeController : Controller
    {
        TypeDetailRepository TypeDetailRepository = new TypeDetailRepository();
        TheTypeRepository TheTypeRepository = new TheTypeRepository();
        RestaurantRepository RestaurantRepository = new RestaurantRepository();
        ReservationOpenedRepository ReservationOpened = new ReservationOpenedRepository();
        CityRepository CityRepository = new CityRepository();
        DiscountRepository DiscountRepository = new DiscountRepository();
        MemberRepository MemberRepository = new MemberRepository();
        NewsRepository NewsRepository = new NewsRepository();
        ReportRepository ReportRepository = new ReportRepository();
        SubTypeRepository SubTypeRepository = new SubTypeRepository();
        QAsystemRepository QARepository = new QAsystemRepository();
        Taste_ItEntities db = new Taste_ItEntities();
        private GenericRepository<Gift> repositoryGift = new GenericRepository<Gift>();
        private GiftDetailRepository repositoryGift_Detail = new GiftDetailRepository();
        private CartService cart = new CartService();


      
        /// <summary>
        /// BackEnd Nav Bar
        /// </summary>
        /// <returns></returns>
        /// 

        #region Nav

    public ActionResult NavNoticeCount()
    {
        IEnumerable<object>  notice = new List<object>();
            
        notice = ReportRepository.GetNoticeByID(Request).Select(L =>
        new
        {
            L.Rep_Content,
            L.Rep_AccuseTypeID,
            L.Rep_RestaurantID,
            L.Rep_SolvedDate,
            L.Rep_ID,
        });

        return Json(notice,JsonRequestBehavior.AllowGet);

    }

    //public ActionResult NavMessageCount()
    //{
    //    IEnumerable<Report> notice = new List<Report>();
    //    switch (Request.GetRole())
    //    {
    //        case _LoginInfo._LoginRole.Administrator:
    //            break;
    //        case _LoginInfo._LoginRole.Member:
    //            notice = ReportRepository.GetNoticeByID(Request.GetRoleKey().Value);
    //            break;
    //        case _LoginInfo._LoginRole.Restaurant:

    //            break;
    //        default:
    //            break;
    //    }

    //    return (notice.Count() == 0) ? Content("") : Content(notice.Count().ToString());
    //}

    [HttpPost]
    public void NavNoticeRead(int Rep_id)
    {
        ReportRepository.NoticeRead(Rep_id);
        return;
    }
    #endregion


        #region Jun
        //[HttpGet]
        //public ActionResult TestChrome(string myLocation, int get = 10)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    IEnumerable<Restaurant> rest = RestaurantRepository.GetAll().Take(get);
        //    List<Thread> threads = new List<Thread>();

        //    List<string> str = new List<string>();



        //    foreach (var r in rest)
        //    {
        //        Thread thread = new Thread(() =>
        //        {
        //            IWebDriver _driver;
        //            ChromeOptions chromeBrowserOptions = new ChromeOptions();
        //            chromeBrowserOptions.AddArguments(new List<string>() { "headless", "disable-gpu" });
        //            _driver = new ChromeDriver(@"C:\inetpub\wwwroot\tasteit\ChromeDriver", chromeBrowserOptions);
        //            _driver.Manage().Cookies.DeleteAllCookies();
        //            _driver.Navigate().GoToUrl($"https://www.google.com/maps/dir/{myLocation}/{r.Res_Address}");//1
        //            var ele = _driver.FindElement(By.XPath("//*[@id='section-directions-trip-0']/div[2]/div[not(contains(@style,'display:none'))]/div[1]"));
        //            //*[@id="section-directions-trip-0"]/div[2]/div[1]/div[1]/div[1]/span[1]
        //            string x = ele.Text;
        //            str.Add($"{r.Res_Name}   {r.Res_Address}  : {x}");
        //            _driver.Close();
        //            _driver.Dispose();
        //        });
        //        threads.Add(thread);
        //        thread.Start();

        //    }

        //    foreach (Thread t in threads)
        //    {
        //        t.Join();
        //    }

        //    str = str.OrderBy(L => L.Length).ToList();


        //    sw.Stop();
        //    ViewBag.from = myLocation;
        //    ViewBag.timespan = sw.ElapsedMilliseconds;
        //    return PartialView(str);
        //    //GetAll() 30400
        //    //Take(5) 5314
        //    //Take(10) 7677

        //}

        public ActionResult GoogleMap()
        {
            GoogleMapApi googleMapApi = new GoogleMapApi();

            ViewBag.geometry = googleMapApi.Geolocation("大安站");

            return View();
        }

        public ActionResult GoogleRestaurantJson(string address = "大安站")
        {

            GoogleMapApi googleMapApi = new GoogleMapApi();

            int? cityID = CityRepository.GetCityIDbyString(address);
            if (!cityID.HasValue)
            {
                cityID = googleMapApi.GoogleApiGetCity(address);
            }
            if (!cityID.HasValue)
            {
                return Content("錯誤地址");
            }

            IEnumerable<Restaurant> restaurants = RestaurantRepository.GetByCityID(cityID.Value);

            IEnumerable<MapObj> list = googleMapApi.NearbyRestaurants(restaurants, address);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GoogleMapAddRestaurantByAddress(string address)
        {
            GoogleMapApi googleMapApi = new GoogleMapApi();

            IEnumerable<RestObj> restObjs = googleMapApi.GetNewRestaurants(address);
            Random _random = new Random();
            int account = 100;
            foreach (RestObj restObj in restObjs)
            {
                int max, min, photo;
                photo = _random.Next(100000, 999999);
                switch (restObj.PriceLevel)
                {
                    case 1:
                        min = 300;
                        max = 500;
                        break;
                    case 2:
                        min = 300;
                        max = 500;
                        break;
                    case 3:
                        min = 1000;
                        max = 2000;
                        break;
                    case 4:
                        min = 2000;
                        max = 10000;
                        break;
                    case 0:
                    default:
                        min = 0;
                        max = 300;
                        break;
                }

                Restaurant restaurant = new Restaurant
                {
                    Res_Name = restObj.RestaurantName,
                    Res_Address = restObj.RestaurantAddress,
                    Res_HomePagePicture = restObj.Photo,
                    Res_City = restObj.RestaurantCity,
                    Res_StarRate = restObj.Rating,


                    Res_CreateDate = DateTime.Now,
                    Res_Account = $"r{account}",
                    Res_Email = @"r100@gmail.com",
                    Res_ParkingSeats = 20,
                    Res_WorkingDate = "禮拜一至日",
                    Res_Password = "000",
                    Res_Phone = $"0229{photo}",
                    Res_MaxConsumption = max,
                    Res_MinConsumption = min,
                    Res_RoleID = 0,
                    Res_QAEnable = true,
                    Res_Seats = 20,
                    Res_TenPercentService = false,

                };
                RestaurantRepository.Insert(restaurant);

            }




            return Json(restObjs.Select(L => new { L.RestaurantName, L.RestaurantAddress, L.RestaurantCity, L.Rating }), JsonRequestBehavior.AllowGet);

            //return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GoogleMapAddRestaurants()
        {
            var cities = CityRepository.GetAll();


            foreach (var city in cities)
            {
                GoogleMapAddRestaurantByAddress(city.Cit_Name.Substring(0, 2));
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GoogleMapApiGetCity(string address)
        {
            GoogleMapApi googleMapApi = new GoogleMapApi();
            return Content(googleMapApi.GoogleApiGetCity(address).ToString());
        }

        public ActionResult GoogleGeometry(string address = "大安站")
        {
            GoogleMapApi googleMapApi = new GoogleMapApi();
            var result = googleMapApi.Geolocation(address);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddBlogToLike(int id)
        {
            ReportRepository.AddBlogToLike(id, Request);
            return Content("成功");
        }

        //訂位查詢        
        [HttpPost]
        public ActionResult RestaurantBlockReservation(int id, DateTime? date = null)
        {
            date = date.HasValue ? date : DateTime.Now;
            var x = ReservationOpened.GetTeamHeadersByRestaurantID(id, date).FirstOrDefault();
            return PartialView(x);
        }

        [HttpGet]
        public ActionResult DiscountBlockList()
        {
            IEnumerable<Discount> discounts;
            switch (Request.QueryString["orders"])
            {
                case "即將推出":
                    discounts = DiscountRepository.GetToStart();
                    break;
                case "即將到期":
                    discounts = DiscountRepository.GetAll(DateTime.Now).Where(L => L.Dis_End.HasValue);
                    break;
                case "熱門":
                default:
                    discounts = DiscountRepository.GetAll(DateTime.Now);
                    break;
            }


            if (Request.QueryString["searchBy"] == "Text")
            {
                string SearchText = Request.QueryString["SearchText"];
                discounts = discounts.Where(L => L.Dis_Title.Contains(SearchText) || L.Restaurant.Res_Name.Contains(SearchText)).AsEnumerable();

            }
            else if (Request.QueryString["searchBy"] == "Adv")
            {

                IEnumerable<int?> subTypes = SubTypeRepository.GetAllForQuery().ToList().Where(L => Request.QueryString[$"s{L.SubT_ID}"] != null).Select(L => (int?)L.SubT_ID);
                IEnumerable<int?> theTypes = TheTypeRepository.GetAllForQuery().ToList().Where(L => Request.QueryString[$"t{L.ResType_ID}"] != null).Select(L => (int?)L.ResType_ID);
                IEnumerable<int?> cities = CityRepository.GetAllForQuery().ToList().Where(L => Request.QueryString[$"c{L.Cit_ID}"] != null).Select(L => (int?)L.Cit_ID);
                IEnumerable<int?> subTypesR = subTypes;
                IEnumerable<int?> theTypesR = theTypes;
                if (subTypes.Count() == 0 && theTypes.Count() == 0)
                {
                    subTypesR = SubTypeRepository.GetAll().Select(L => (int?)L.SubT_ID);
                    theTypesR = TheTypeRepository.GetAll().Select(L => (int?)L.ResType_ID);
                }
                IEnumerable<int?> citiesR = cities.Count() == 0 ? CityRepository.GetAll().Select(L => (int?)L.Cit_ID) : cities;
                List<int> byCity = RestaurantRepository.GetAll().Where(L => citiesR.Contains(L.Res_City.Value)).Select(L => L.Res_ID).ToList();
                List<int> restID;
                if (subTypes.Count() == 0 && theTypes.Count() == 0)
                {
                    restID = byCity;
                }
                else
                {
                    List<int> type_Details = TypeDetailRepository.GetAll()
                        .Where(L => subTypesR.Contains(L.ResTypeDet_SubTypeID) || theTypesR.Contains(L.ResTypeDet_ResTypeID))
                        .Select(L => L.ResTypeDet_RestaurantID.Value)
                        .Distinct()
                        .ToList();

                    restID = byCity.Intersect(type_Details).ToList();
                }

                discounts = discounts.Where(L => restID.Contains(L.Dis_RestaurantID)).ToList();

            }
            else
            {
                discounts = discounts;
                //default
            }

            switch (Request.QueryString["orders"])
            {
                case "即將推出":
                    discounts = discounts.OrderBy(L => L.Dis_Start).AsEnumerable();
                    break;
                case "即將到期":
                    discounts = discounts.OrderBy(L => L.Dis_End).AsEnumerable();
                    break;
                case "熱門":
                default:
                    discounts = discounts.OrderBy(L => L.Dis_Start).AsEnumerable();
                    break;
            }
            int scorllTime = Convert.ToInt32(Request.QueryString["scorllTime"]);
            const int PageItems = 4;
            return PartialView(discounts.Skip(scorllTime * PageItems).Take(PageItems));
        }


        [HttpGet]
        public ActionResult DiscountSearching()
        {
            DisSearchPara para = new DisSearchPara();
            if (Request.QueryString.AllKeys.Count() == 0)
            {
                para.cities = CityRepository.GetAll();
                para.theTypes = TheTypeRepository.GetAll();
                para.subTypes = SubTypeRepository.GetAll();
                para.discounts = DiscountRepository.GetAll(DateTime.Now);
            }
            else
            {

            }
            return View(para);
        }



        [HttpGet]
        public ActionResult NewsBlockList()
        {
            IEnumerable<News> news = NewsRepository.GetAllVaild();

            if (Request.QueryString["searchBy"] == "Text")
            {
                string SearchText = Request.QueryString["SearchText"];
                news = news.Where(L => L.New_Title.Contains(SearchText)
                || L.Restaurant.Res_Name.Contains(SearchText)
                ).AsEnumerable();

            }
            else if (Request.QueryString["searchBy"] == "Adv")
            {
                IEnumerable<int?> subTypes = SubTypeRepository.GetAllForQuery().ToList().Where(L => Request.QueryString[$"s{L.SubT_ID}"] != null).Select(L => (int?)L.SubT_ID);
                IEnumerable<int?> theTypes = TheTypeRepository.GetAllForQuery().ToList().Where(L => Request.QueryString[$"t{L.ResType_ID}"] != null).Select(L => (int?)L.ResType_ID);
                IEnumerable<int?> cities = CityRepository.GetAllForQuery().ToList().Where(L => Request.QueryString[$"c{L.Cit_ID}"] != null).Select(L => (int?)L.Cit_ID);
                IEnumerable<int?> subTypesR = subTypes;
                IEnumerable<int?> theTypesR = theTypes;

                IEnumerable<int?> citiesR = cities.Count() == 0 ? CityRepository.GetAll().Select(L => (int?)L.Cit_ID) : cities;
                List<int> byCity = RestaurantRepository.GetAll().Where(L => citiesR.Contains(L.Res_City.Value)).Select(L => L.Res_ID).ToList();
                List<int> restID;
                if (subTypes.Count() == 0 && theTypes.Count() == 0)
                {
                    restID = byCity;
                }
                else
                {
                    List<int> type_Details = TypeDetailRepository.GetAll()
                        .Where(L => subTypesR.Contains(L.ResTypeDet_SubTypeID) || theTypesR.Contains(L.ResTypeDet_ResTypeID))
                        .Select(L => L.ResTypeDet_RestaurantID.Value)
                        .Distinct()
                        .ToList();

                    restID = byCity.Intersect(type_Details).ToList();
                }

                news = news.Where(L => restID.Contains(L.New_RestaurantID)).ToList();
            }
            else
            {
                news = news;
                //default
            }

            switch (Request.QueryString["orders"])
            {

                case "點擊率":
                    news = news.OrderByDescending(L => L.New_ClickRate);
                    break;
                case "最新":
                default:
                    news = news.OrderByDescending(L => L.New_CreateDate);
                    break;
            }

            int scorllTime = Convert.ToInt32(Request.QueryString["scorllTime"]);
            const int PageItems = 4;
            return PartialView(news.Skip(scorllTime * PageItems).Take(PageItems));

        }


        [HttpGet]
        public ActionResult NewsSearching()
        {
            NewsSearchPara para = new NewsSearchPara();
            if (Request.QueryString.AllKeys.Count() == 0)
            {
                para.cities = CityRepository.GetAll();
                para.theTypes = TheTypeRepository.GetAll();
                para.subTypes = SubTypeRepository.GetAll();
                para.news = NewsRepository.GetAll();
            }
            else
            {

            }
            return View(para);
        }




        #endregion


        #region chu


        //餐廳搜尋       
        public ActionResult advancedsearchtemplate(string searchinput="")
        {
            ViewBag.navsearch = searchinput;
            Advancedsearch adt = new Advancedsearch
            {
                theTypes = RestaurantRepository.F_astype(),
                subTypes = RestaurantRepository.F_assubtupe(),
                cities = RestaurantRepository.F_ascity(),
                maxcost = RestaurantRepository.F_maxcost()
            };
            return View(adt);
        }

        [HttpPost]
        public ActionResult jsonwork(string therequest)
        {
            var selectresult = JsonConvert.DeserializeObject<List<searchresult>>(therequest);

            return Json(selectresult, JsonRequestBehavior.AllowGet);
        }

        #region advancesearch
        [HttpPost]
        public ActionResult resultele(string therequest)
        {
            string givestrin = "";
            var selectresult = JsonConvert.DeserializeObject<List<searchresult>>(therequest);
            var result = (RestaurantRepository.advanvedsearchresult(selectresult)).ToList();
            for (int i = 0; i < result.Count(); i++)
            {
                givestrin += result[i].ToString() + ",";
            }
            ViewBag.ssstring = "";
            ViewBag.intstring = givestrin;
            ViewBag.r_count = result.Count();
            var target = RestaurantRepository.getreslist(result).AsQueryable();
            target = target.OrderBy(r => r.Res_ID);
            return PartialView(target.ToList().ToPagedList(1, 5));
        }

        [HttpPost]
        public ActionResult searchkeyword(string s_string)
        {
            string givestrin = "";
            var result = RestaurantRepository.searchkeyword(s_string).ToList();
            for (int i = 0; i < result.Count(); i++)
            {
                givestrin += result[i].ToString() + ",";
            }
            ViewBag.ssstring = "";
            ViewBag.intstring = givestrin;
            ViewBag.r_count = result.Count();
            var target = RestaurantRepository.getreslist2(result).AsQueryable();
            target = target.OrderBy(r => r.Res_ID);
            return PartialView(target.ToList().ToPagedList(1, 5));
        }

        //string r_sort="";
        [HttpPost]
        public ActionResult p_resultele(int page, string sortby, string idstring)
        {
            //r_sort = sortby;
            //ViewBag.intstring = idstring;
            List<int> l_int = new List<int>();
            string[] intstring = idstring.Split(',');
            foreach (string theid in intstring)
            {
                if (theid != "")
                {
                    l_int.Add(Convert.ToInt32(theid));
                }
            }
            var target = RestaurantRepository.getreslist(l_int).AsQueryable();
            switch (sortby)
            {
                case "desbyprice":
                    target = target.OrderByDescending(r => (r.Res_MinConsumption + r.Res_MaxConsumption) / 2);
                    break;
                case "desbybtag":
                    target = target.OrderByDescending(r => r.Type_Detail.Select(tt => tt.ResTypeDet_ResTypeID));
                    break;
                case "resbybtag":
                    target = target.OrderBy(r => r.Type_Detail.Select(tt => tt.ResTypeDet_ResTypeID));
                    break;
                case "desbystag":
                    target = target.OrderByDescending(r => r.Type_Detail.Select(tt => tt.ResTypeDet_SubTypeID));
                    break;
                case "resbystag":
                    target = target.OrderBy(r => r.Type_Detail.Select(tt => tt.ResTypeDet_SubTypeID));
                    break;
                case "desbycity":
                    target = target.OrderByDescending(r => r.Res_StarRate);
                    break;
                case "resbycity":
                    target = target.OrderBy(r => r.Res_StarRate);
                    break;
                case "resbyprice":
                    target = target.OrderBy(r => (r.Res_MinConsumption + r.Res_MaxConsumption) / 2);
                    break;
                case "":
                    target = target.OrderBy(r => r.Res_ID);
                    break;
            }
            int currentPage = page < 1 ? 1 : page;
            //ViewBag.r_count = target.Count();
            return PartialView("as_pagelist", target.ToList().ToPagedList(currentPage, 5));
        }

        [HttpPost]
        public ActionResult as_pagelist()
        {
            //ViewBag.ssstring = r_sort;
            return PartialView();
        }

        #endregion

        //餐廳小框pw
        [ChildActionOnly]
        public ActionResult RestaurantBlock(int id)
        {
            if (Request.Cookies["Role"] != null)
            {
                if (Request.Cookies["Role"].Value == "Member")
                {
                    int memid = Convert.ToInt32(Request.Cookies["Key"].Value);
                    ViewBag.flag=MemberRepository.likeresornot(memid, id);
                    ViewBag.the_mem = memid;
                }
            }
            var restaurant = RestaurantRepository.GetByID(id);
            int count = restaurant.Res_IntroductionContext == null ? (Int32)Math.Sqrt((restaurant.Res_ID + 40) * 1000) : (Int32)Math.Sqrt(Convert.ToInt32((restaurant.Res_IntroductionContext.Length + restaurant.Res_ID) * 1000));
            ViewBag.likecount = count;
            ViewBag.stagstring = RestaurantRepository.takeres_stag(id);
            ViewBag.tagstring = RestaurantRepository.takeres_tag(id);            
            ViewBag.stagstring = ViewBag.stagstring == "無小標籤" ? null : ViewBag.stagstring;
            ViewBag.tagstring = ViewBag.tagstring == "無大標籤" ? null : ViewBag.tagstring;
            return PartialView(restaurant);
        }

        [HttpGet]
        public ActionResult likeornot(int memid,int resid)
        {
            string result = MemberRepository.likeresornot(memid,resid);
            return Content($"{result}");
        }

        public ActionResult getlike_backgroundimage()
        {
            return File("~/tasteitlogo/circle_icon.jpg", "image/jpg");
        }

        public ActionResult getimage(int id)
        {
            try
            {
                return File(RestaurantRepository.getimagebyid(id), "image/jpeg");
            }
            catch
            {
                return File("~/tasteitloge/logo.png", "image/png");
            }

        }

        #endregion


	
        #region Wei
        //主頁
       public ActionResult Index()
        {
            //this.
            var qs = Request.QueryString;
            if (qs != null && qs.Count > 0)
            {
                string from = qs["from"];
                string userId = qs["id"];
                string userName = qs["name"];
               /* userName = userName.Substring(0, 10);*/// db name length 10
                // 1. check user exist and get user info
                var userInfo = MemberRepository.ThirdPartyLoginCheck(from, userId);
                if (userInfo == null)
                {
                    // 2. if not exist ,create user;
                    Member newMem = new Member()
                    {
                        Mem_Account = userName,
                        Mem_Name = userName,
                        Mem_Password = userId,
                        Mem_Birthday = DateTime.Now,
                        Mem_NickName = userName,
                        Mem_Phone = "",
                        Mem_RoleID = 1,
                        Mem_JoinDate = DateTime.Now,
                       Mem_Source = from,
                       Mem_SourceID = userId
                    };
                    MemberRegistRepository.Insert(newMem);
                }

                // 3. get user info
                userInfo = MemberRepository.ThirdPartyLoginCheck(from, userId);
                Response.Cookies.Set(new HttpCookie("Role", "Member"));
                Response.Cookies.Set(new HttpCookie("Key", userInfo.Mem_ID.ToString()));

            }
            FirstPage firstPage = new FirstPage
            {
                Newinnews = NewsRepository.News_Newin(),
                Hotnews = NewsRepository.News_HotIssue(),
                Star = RestaurantRepository.R_Flipper(),
                DiscountCube = DiscountRepository.Discount_Cube(),
                Special = RestaurantRepository.R_Special(),
                ClickRatenews = NewsRepository.News_ClickRate(),
            };
            return View(firstPage);
        }
        public ActionResult GetDiscountImage(int id)
        {
            try
            {
                return File(DiscountRepository.GetDiscountImageByid(id), "image/jpeg");
            }
            catch
            {
                return File("~/tasteitlogo/logo.png", "image/png");
            }

        }







        //會員登入
        [HttpGet]
        public ActionResult MemberLogin()
        {
            return View();
        }



        [HttpPost]

        public ActionResult MemberLogin(LoginVM _loginVM)
        {
            var member_username = MemberRepository.M_LoginCheck(_loginVM);
            var admin_username = MemberRepository.Admin_LoginCheck(_loginVM);


            if (admin_username != null)   //Administrator
            {
              

                Response.SetRole(_LoginInfo._LoginRole.Administrator);
                Response.SetKey(member_username.Mem_ID);

                if (_loginVM.remember == "yes")
                {
                    Response.Cookies["Role"].Expires = DateTime.Now.AddDays(7);
                }
                ViewBag.user = member_username.Mem_Account;
                return Content($"<script>location.href='{Url.Action("Index","Home")}'</script>");

            }

            else if (member_username != null)   //Member
            {
                
                //Response.Cookies["Role"].Value = "Mem";
                Response.SetRole(_LoginInfo._LoginRole.Member);
                Response.SetKey(member_username.Mem_ID);
                //Response.Cookies["Key"].Value = username.Mem_ID.ToString();
                if (_loginVM.remember == "yes")
                {
                    Response.Cookies["Role"].Expires = DateTime.Now.AddDays(7);
                }
                ViewBag.user = member_username.Mem_Account;
                return Content($"<script>location.href='{Url.Action("Index","Home")}'</script>");

            }



            else
            {
                ViewBag.message = "登入失敗，請確認帳號或密碼無誤";
            }
            return View();
        }

        //會員登出
        public ActionResult LogOut()
        {

            Response.Cookies["Role"].Expires = DateTime.Now.AddSeconds(-1);
            Response.Cookies["Key"].Expires = DateTime.Now.AddSeconds(-1);

            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        GenericRepository<Member> MemberRegistRepository = new GenericRepository<Member>();

        //會員註冊
        [HttpGet]
        public ActionResult MemberRegeist()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MemberRegeist(Member member, HttpPostedFileBase MemberPhoto)
        {
            if (MemberPhoto != null)
            {

                int imgSize = MemberPhoto.ContentLength;
                byte[] imgbyte = new byte[imgSize];
                MemberPhoto.InputStream.Read(imgbyte, 0, imgSize);
                member.Mem_Photo = imgbyte;

                member.Mem_RoleID = 1;
                member.Mem_JoinDate = DateTime.Now;
                MemberRegistRepository.Insert(member);
                return RedirectToAction("Index");

            }

            ViewBag.message = "請選擇檔案";

            return View();



        }


        //會員忘記密碼
        [HttpGet]
        public ActionResult MemberForget()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MemberForget(ForgotPass forgotPass)
        {
            var check_username = MemberRepository.M_ForgotPasswordCheck(forgotPass);
            if (check_username != null)
            {
                //寄信
                string MailFrom = "msit121sa@gmail.com";  //寄件者
                string MailPass = "ZAQ!xsw2";  //密碼
                string MailTo = check_username.Mem_Mail;  //收件者
                string Subject = "TasteIt 為您送來了密碼~!";  //主旨
                string Body = "您的會員密碼為:" + check_username.Mem_Password + Environment.NewLine + "建議登入後盡速更改密碼。";  //內文
                //未來有換行的問題
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress(MailFrom, "TasteIt", Encoding.UTF8);  //寄件者
                Message.To.Add(MailTo);  //收件者
                Message.Subject = Subject;  //郵件標題 
                Message.SubjectEncoding = Encoding.UTF8;  //郵件標題編碼  
                Message.Body = Body;  //郵件內容
                Message.IsBodyHtml = true;
                Message.BodyEncoding = Encoding.UTF8;  //郵件內容編碼 
                Message.Priority = MailPriority.Normal;  //郵件優先等級 

                SmtpClient Client = new SmtpClient("smtp.gmail.com", 587);  //設定SMTP主機及Port, hotmail:smtp.live.com 
                Client.Credentials = new System.Net.NetworkCredential(MailFrom, MailPass);  //設定帳號密碼
                Client.EnableSsl = true;  //使用SSL
                Client.Send(Message);  //傳送

                //


                ViewBag.message = "已寄出新密碼，請至信箱查看";
            }
            else
            {
                ViewBag.message = "請輸入正確的帳號與註冊時之電子信箱!";
            }


            return View();
        }


        //餐廳登入
        [HttpGet]
        public ActionResult RestaurantLogin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult RestaurantLogin(LoginVM _loginVM)
        {
            var username = RestaurantRepository.R_LoginCheck(_loginVM);
            if (username != null)
            {
               
                //Response.Cookies["Role"].Value = "Rest";
                Response.SetRole(_LoginInfo._LoginRole.Restaurant);
                //Response.Cookies["Key"].Value = username.Res_ID.ToString();
                Response.SetKey(username.Res_ID);
                if (_loginVM.remember == "yes")
                {
                    Response.Cookies["Role"].Expires = DateTime.Now.AddDays(7);
                }
                return Content($"<script>location.href='{Url.Action("Index","Home")}'</script>");   //轉到該餐廳主頁

            }

            else
            {
                ViewBag.message = "登入失敗，請確認帳號或密碼無誤";
            }
            return View();
        }


        //餐廳註冊

        GenericRepository<Restaurant> RestRegistRepository = new GenericRepository<Restaurant>();

        [HttpGet]
        public ActionResult RestaurantRegeist()
        {
            ViewBag.cities = CityRepository.GetAll();
            return View();
        }


        [HttpPost]
        public ActionResult RestaurantRegeist(Restaurant restaurant, HttpPostedFileBase RestPhoto)
        {
            if (RestPhoto != null)
            {

                int imgSize = RestPhoto.ContentLength;
                byte[] imgbyte = new byte[imgSize];
                RestPhoto.InputStream.Read(imgbyte, 0, imgSize);
                restaurant.Res_HomePagePicture = imgbyte;

                restaurant.Res_RoleID = 2;
                restaurant.Res_CreateDate = DateTime.Now;
                RestRegistRepository.Insert(restaurant);
                return RedirectToAction("Index");

            }
            ViewBag.cities = CityRepository.GetAll();
            ViewBag.message = "請選擇檔案";

            return View();

        }
        //餐廳忘記密碼
        [HttpGet]
        public ActionResult RestaurantForget()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RestaurantForget(ForgotPass forgotPass)
        {
            var check_username = RestaurantRepository.R_ForgotPasswordCheck(forgotPass);
            if (check_username != null)
            {
                //寄信
                string MailFrom = "msit121sa@gmail.com";  //寄件者
                string MailPass = "ZAQ!xsw2";  //密碼
                string MailTo = check_username.Res_Email;  //收件者
                string Subject = "TasteIt 為您送來了密碼~!";  //主旨
                string Body = "您的會員密碼為:" + check_username.Res_Password + Environment.NewLine + "建議登入後盡速更改密碼。";  //內文
                //未來有換行的問題
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress(MailFrom, "TasteIt", Encoding.UTF8);  //寄件者
                Message.To.Add(MailTo);  //收件者
                Message.Subject = Subject;  //郵件標題 
                Message.SubjectEncoding = Encoding.UTF8;  //郵件標題編碼  
                Message.Body = Body;  //郵件內容
                Message.IsBodyHtml = true;
                Message.BodyEncoding = Encoding.UTF8;  //郵件內容編碼 
                Message.Priority = MailPriority.Normal;  //郵件優先等級 

                SmtpClient Client = new SmtpClient("smtp.gmail.com", 587);  //設定SMTP主機及Port, hotmail:smtp.live.com 
                Client.Credentials = new System.Net.NetworkCredential(MailFrom, MailPass);  //設定帳號密碼
                Client.EnableSsl = true;  //使用SSL
                Client.Send(Message);  //傳送

                //


                ViewBag.message = "已寄出新密碼，請至信箱查看";
            }
            else
            {
                ViewBag.message = "請輸入正確的帳號與註冊時之電子信箱!";
            }


            return View();
        }


        #endregion



        #region Han
        public ActionResult QACommentsCount()
        {
            IEnumerable<object> QAComments = new List<object>();
            switch (Request.GetRole())
            {
                case _LoginInfo._LoginRole.Administrator:
                    QAComments = QARepository.GetQAGroupByID(null, null, Request.GetRoleKey().Value).Select(L => new
                    {
                        L.QA_ID,
                        L.QA_AnswerdDate,
                        L.QA_Content,
                        L.QA_RestaurantID,
                        L.QA_AdministratorID,
                        L.QA_MemberID,
                        Res_Name = L.Restaurant == null ? null : L.Restaurant.Res_Name,
                        Meb_Name = L.Member == null ? null : L.Member.Mem_Name,
                        role = "Administrator"
                    });
                    break;
                case _LoginInfo._LoginRole.Member:
                    QAComments = QARepository.GetQAGroupByID(Request.GetRoleKey().Value, null, null).Select(L => new
                    {
                        L.QA_ID,
                        L.QA_AnswerdDate,
                        L.QA_Content,
                        L.QA_RestaurantID,
                        L.QA_AdministratorID,
                        Res_Name = L.Restaurant == null ? null : L.Restaurant.Res_Name,
                        role = "Member"
                    });

                    break;
                case _LoginInfo._LoginRole.Restaurant:
                    QAComments = QARepository.GetQAGroupByID(null, Request.GetRoleKey().Value, null).Select(L => new
                    {
                        L.QA_ID,
                        L.QA_AnswerdDate,
                        L.QA_Content,
                        L.QA_RestaurantID,
                        L.QA_AdministratorID,
                        L.QA_MemberID,
                        Meb_Name = L.Member == null ? null : L.Member.Mem_Name,
                        role = "Restaurant"
                    });
                    break;
                default:
                    break;
            }

            //return (notice.Count() == 0) ? Content("") : Content(notice.Count().ToString());

            return Json(QAComments, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public void QANoticeRead(int QA_ID)
        {
            QARepository.QaNoticeRead(QA_ID);
            return;
        }

        public ActionResult Contactpersons(int? QA_MemberID, int? QA_RestaurantID)
        {
            //管理員主動跟會員講話的方法 :連結為  @Html.Action("AdmQAShow", "Administrator", new { QA_MemberID = member的id })
            //~/Administrator/AdmQAShow ? QA_MemberID=22
            //管理員主動跟餐廳講話的方法 :連結為  @Html.Action("AdmQAShow", "Administrator", new { QA_RestaurantID = member的id })
            //~/Administrator/AdmQAShow ? QA_RestaurantID = 64
            //現在差新增首次聊天後，左邊的leftchar應該要用ajax更新
            if (Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                //var x = QARepository.GetByMemberID(QA_MemberID, Request);
                //x.QA_SystemsResNameData = null;

                return PartialView(QARepository.GetByMemberID(QA_MemberID, QA_RestaurantID, Request));
            }
            else if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                //return RedirectToAction("QAShow", "Member", new { ResID = QA_RestaurantID });
                return PartialView(QARepository.GetByIDForMember(null, null, Request));
            }
            else
            {
                return PartialView(QARepository.GetByIDForRestaurant(null, null, Request));
            }

        }

        public ActionResult ContactMessange(int? QA_MemberID, int? QA_RestaurantID, int? QA_AdministratorID)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                return PartialView(QARepository.GetByMemberID(QA_MemberID, QA_RestaurantID, Request));
            }
            else if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                //return RedirectToAction("QAShow", "Member", new { ResID = QA_RestaurantID });
                return PartialView(QARepository.GetByIDForMember(QA_AdministratorID, QA_RestaurantID, Request));
            }
            else
            {
                return PartialView(QARepository.GetByIDForRestaurant(QA_MemberID, QA_AdministratorID, Request));
            }
        }
        public ActionResult ContactContent(int? QA_MemberID, int? QA_RestaurantID, int? QA_AdministratorID)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                return PartialView(QARepository.GetByMemberID(QA_MemberID, QA_RestaurantID, Request));
            }
            else if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                //return RedirectToAction("QAShow", "Member", new { ResID = QA_RestaurantID });
                return PartialView(QARepository.GetByIDForMember(QA_AdministratorID, QA_RestaurantID, Request));
            }
            else
            {
                return PartialView(QARepository.GetByIDForRestaurant(QA_MemberID, QA_AdministratorID, Request));
            }
        }

        public ActionResult ContactTitle(int? QA_MemberID, int? QA_RestaurantID, int? QA_AdministratorID)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                if (QA_RestaurantID == null)
                {
                    return PartialView(QARepository.GetByMemberID(QA_MemberID, null, Request));
                }
                else
                {
                    return PartialView(QARepository.GetByMemberID(null, QA_RestaurantID, Request));
                }
            }
            else if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                //return RedirectToAction("QAShow", "Member", new { ResID = QA_RestaurantID });
                return PartialView(QARepository.GetByIDForMember(QA_AdministratorID, QA_RestaurantID, Request));
            }
            else
            {
                return PartialView(QARepository.GetByIDForRestaurant(QA_MemberID, QA_AdministratorID, Request));
            }
        }
        public ActionResult DeleteNews(int id = 0)
        {
            QARepository.DeleteNews(id);
            return RedirectToAction("ProsecutionOverview", "Administrator");
        }
        public ActionResult DeleteComment(int id = 0)
        {
            QARepository.DeleteComment(id);
            return RedirectToAction("ProsecutionOverview", "Administrator");
        }
        #endregion


        #region Fu
        [HttpGet]
        public ActionResult RemoveBlogLike(int id)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            var q = ReportRepository.GetByNewsID(id);
            var x = q.Where(d => d.Rep_MemberID == Request.GetRoleKey().Value && d.Rep_Status == 2).FirstOrDefault();
            ReportRepository.Delete(x);
            return Content("已取消收藏");
        }

        public ActionResult BlogWrite(int id = 0)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            ViewBag.RestID = id;
            return View();
        }
        //改
        [HttpPost]

        public ActionResult BlogWrite(News news)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }

            News CreateNews = new News();
            CreateNews.New_Title = news.New_Title;
            CreateNews.New_Content = news.New_Content;
            CreateNews.New_MemberID = Request.GetRoleKey().Value;
            //要抓食記的餐廳ID
            CreateNews.New_RestaurantID = news.New_RestaurantID;
            CreateNews.New_CreateDate = DateTime.Now;
            Member member = db.Members.Find(Request.GetRoleKey().Value);
            member.Mem_Point += 134;
            db.SaveChanges();
            NewsRepository.Insert(CreateNews);
            return RedirectToAction("BlogSreach", "Member");
        }

        public ActionResult BlogPage(int id = 0)
        {
            News news = NewsRepository.GetByID(id);
            return View(news);
        }
        #endregion



    }
}