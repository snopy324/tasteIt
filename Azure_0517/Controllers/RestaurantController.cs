using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azure_0517.Models;
using Azure_0517.Models.Repository;
using Azure_0517.Models.ViewModel;

namespace Azure_0517.Controllers
{
    public class RestaurantController : Controller
    {
        ReservationOpenedRepository OpenedRepository = new ReservationOpenedRepository();
        ReservationBookedRepository BookedRepository = new ReservationBookedRepository();
        GenericRepository<Member> MemRepository = new GenericRepository<Member>();
        DiscountRepository DiscountRepository = new DiscountRepository();
        private RestaurantRepository restaurantrepository = new RestaurantRepository();
        private PictureRepository picturerepository = new PictureRepository();
        Taste_ItEntities db = new Taste_ItEntities();
        CommentRepository CommentRepository = new CommentRepository();
        ReportRepository ReportRepository = new ReportRepository();
        NewsRepository NewsRepository = new NewsRepository();
        QAsystemRepository QARepository = new QAsystemRepository();


        #region Jun
        [HttpPost]
        public ActionResult ReservationBookCheckIn(int id)
        {
            BookedRepository.CheckIn(id);
            return Content("Done");
        }


        [HttpGet]
        public ActionResult ReservationBookRepeat(int openid)
        {
            ReservationOpened opened = OpenedRepository.GetByID(openid);
            Member mem = MemRepository.GetByID(Request.GetRoleKey().Value);
            int teamHeader = opened.RVOpen_TeamHeader;
            string str = "確認訂位?";
            IEnumerable<ReservationBooked> bookRepeats = BookedRepository.RepeatInDay(opened, mem.Mem_ID);
            if (bookRepeats.Count() > 0)
            {
                string tempstr = "";
                foreach (ReservationBooked book in bookRepeats)
                {
                    tempstr += $"編號:{book.RVBooked_ID}, 時間:{book.ReservationOpened.RVOpen_DateTime.ToString("HH:mm")}, {book.RVBooked_Seats}位\n";
                }
                str = $"你在的選定日期已有該餐廳的訂位，確定要繼續嗎?" +
                    $"已存在訂位:\n{tempstr}";
                ReservationBooked booked = BookedRepository.RepeatInOpen(opened, Request.GetRoleKey().Value);
                if (booked != default(ReservationBooked))
                {
                    str = $"你在選定的時間已有訂位紀錄，若要繼續將會將數量累加?\n原有訂位數量:{booked.RVBooked_Seats}";
                }
            }

            return Content(str, "text/plain");
        }

        [HttpGet]//訂位頁面
        public ActionResult ReservationBooking(int id)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                Response.StatusCode = 400;
                return Content("Naughty");
            }
            ReservationOpened target = OpenedRepository.GetByID(id);
            //Member member = MemRepository.GetByID(Convert.ToInt32(Request.Cookies["Login_Info"].Value.Split(',')[1]));
            Member member = MemRepository.GetByID(Request.GetRoleKey().Value);

            ReservationOpen_Book open_Book = ReservationOpen_Book.NewObject(target, member);
            return PartialView(open_Book);


        }
        [HttpPost]
        public ActionResult ReservationBooking(ReservationOpen_Book open_Book)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }

            ReservationBooked repeat = BookedRepository.RepeatInOpen(OpenedRepository.GetByID(open_Book.Booked.RVBooked_ReservationOpenedID.Value), Request.GetRoleKey().Value);
            if (repeat != null)
            {
                open_Book.Booked.RVBooked_Seats += repeat.RVBooked_Seats;
                BookedRepository.Delete(repeat);
            }

            open_Book.Booked.RVBooked_BookedDate = DateTime.Now;
            if (BookedRepository.Insert(open_Book.Booked))
            {
                ReservationOpened reservation = OpenedRepository.GetByID(open_Book.Booked.RVBooked_ReservationOpenedID.Value);
                if (open_Book.Booked.RVBooked_ReminderContext != null && open_Book.Booked.RVBooked_ReminderContext.Contains("BackUpSeats:"))
                {
                    //TempData["message"] = $"You are Backup No.{open_Book.Booked.RVBooked_ReminderContext.Split(';')[0].Split(':')[1]}";
                }
                return Content("訂位成功");
            }
            TempData["message"] = "ReservationError";
            return Content("訂位失敗");
        }
        [HttpGet]//訂位總覽
        public ActionResult ReservationOverview()
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Restaurant)
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }

            //int key = Convert.ToInt32(Request.Cookies["Login_Info"].Value.Split(',')[1]);


            return PartialView();
        }
        [HttpGet]
        public ActionResult ReservationOverviewBlock(DateTime? date = null)
        {
            int? id = Request.GetRoleKey().Value;
            return PartialView(OpenedRepository.GetTeamHeadersByRestaurantID(id.Value, date));
        }

        public ActionResult ReservationOverviewBlockForPass(DateTime? date = null)
        {
            int? id = Request.GetRoleKey().Value;
            return PartialView("ReservationOverviewBlock", OpenedRepository.GetTeamHeadersByRestaurantID(id.Value, enddate: date));
        }
        [HttpGet]//新增訂位
        public ActionResult ReservationNew()
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Restaurant)
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }
            int? id = Request.GetRoleKey().Value;
            ViewBag.RestaurantID = id;
            return PartialView();
        }
        [HttpPost]
        public ActionResult ReservationNew(ReservationOpened reservation, ReservationNewModel newModel)
        {
            if (newModel.StepTime <= 0)
            {
                ViewBag.message = "Fail";
                return RedirectToAction("ReservationOverview", "Restaurant");
            }///////Temp
            OpenedRepository.InsertNewBlock(reservation, newModel);
            return Content("新增完成");
        }
        [HttpGet]//優惠管理
        public ActionResult DiscountManage(int id)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Restaurant)
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }

            return PartialView(DiscountRepository.GetByID(id));
        }
        [HttpPost]
        public ActionResult DiscountManage(Discount discount, HttpPostedFileBase Picture)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Restaurant)
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }
            Discount oldDis = DiscountRepository.GetByID(discount.Dis_ID);
            if (oldDis == null)
            {
                return Content("");
            }
            if (Picture != null)
            {
                int length = Picture.ContentLength;
                byte[] image = new byte[length];
                Picture.InputStream.Read(image, 0, length);
                oldDis.Dis_Picture = image;
            }

            oldDis.Dis_Content = discount.Dis_Content;
            oldDis.Dis_End = discount.Dis_End;
            oldDis.Dis_Start = discount.Dis_Start;
            oldDis.Dis_Title = discount.Dis_Title;


            DiscountRepository.Update(oldDis);
            return RedirectToAction("DiscountOverview");
        }
        [HttpGet]//優惠總覽
        public ActionResult DiscountOverview()
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Restaurant)
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }
            int? id = Request.GetRoleKey().Value;
            return PartialView(DiscountRepository.GetbyRestID(id.Value));
        }
        [HttpGet]
        public ActionResult DiscountNew()
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Restaurant)
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }
            int? id = Request.GetRoleKey().Value;
            ViewBag.RestaurantID = id.Value;
            return PartialView();
        }
        [HttpPost]
        public ActionResult DiscountNew(Discount discount, HttpPostedFileBase Picture)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Restaurant)
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }

            int length = Picture.ContentLength;
            byte[] image = new byte[length];
            Picture.InputStream.Read(image, 0, length);

            discount.Dis_Picture = image;

            DiscountRepository.Insert(discount);
            return RedirectToAction("DiscountOverview");
        }
        [HttpGet]
        public ActionResult DiscountPage(int id)
        {


            return View(DiscountRepository.GetByID(id));
        }
        //訂位框
        public ActionResult ReservationBlock(IGrouping<int, ReservationOpened> team, bool Manageable = false)
        {
            ViewBag.Manageable = Manageable;
            return PartialView(team);
        }
        //優惠框
        public ActionResult DiscountBlock(Discount discount, bool Manage = false, int? listCount=4)
        {
            ViewBag.listCount = listCount;
            ViewBag.Manage = Manage;
            return PartialView(discount);
        }
        //訂位管理框
        public ActionResult HomePageReservation(int id, DateTime? date = null)
        {
			if (date == DateTime.Now.Date || date == null)
            {
                date = DateTime.Now;
            }
            var x = OpenedRepository.GetTeamHeadersByRestaurantID(id, date).ToList();
            return PartialView(x);
        }

        public void DeleteTeamHeader(int key)
        {
            OpenedRepository.DeleteTeamHeader(key);
            return;
        }

        public ActionResult DiscountDelete(int id)
        {
            DiscountRepository.Delete(DiscountRepository.GetByID(id));
            return Content("");
        }

        public ActionResult GetDiscountImg(int id)
        {
            try
            {
                return File(DiscountRepository.GetImg(id), "image/jpeg");
            }
            catch
            {
                return File("~/tasteitlogo/logo.png", "image/png");
            }

        }

        public ActionResult NewsMetroBlock(int id)
        {
            return PartialView(NewsRepository.GetByID(id));
        }

        public ActionResult HomePageDiscounts(int id,int? page)
        {
            const int pageItems = 3;
            page = page.HasValue ? page : 0;
            var model = DiscountRepository.GetbyRestID(id).Skip(page.Value*pageItems).Take(pageItems);

            return PartialView(model);
        }
        #endregion

        #region Chu

        //餐廳主頁//!!!!!!!!!!!!!!!!
        public ActionResult RestaurantHomePage(int id, int self = 0)
        {
            ViewBag.stagstring = restaurantrepository.takeres_stag(id);
            ViewBag.tagstring = restaurantrepository.takeres_tag(id);
            
            int flag = 0; int key = 0; int? key2 = null;
            if (Request.Cookies["Role"] != null)
            {
                if (_LoginInfo.GetRole(Request) != _LoginInfo._LoginRole.Member | _LoginInfo.GetRole(Request) != _LoginInfo._LoginRole.Administrator)
                {
                    key2 = Convert.ToInt32(Request.Cookies["Key"].Value);
                    key2 = Convert.ToInt32(Request.Cookies["Key"].Value);
                }
                flag = restaurantrepository.flagrole(Request.Cookies["Role"].Value);
                key = Convert.ToInt32(Request.Cookies["Key"].Value);
            }
            Restaurant restaurant = restaurantrepository.GetByID(id);
            GoogleMapApi googleMapApi = new GoogleMapApi();
            CommentViewmodel fuviewmodel = new CommentViewmodel
            {
                resself = self,
                Restaurant = restaurantrepository.GetByID(id),
                geometry = googleMapApi.Geolocation(restaurant.Res_Address),
                therole = flag,
                thekey = key,
                mlove = restaurantrepository.flagmemberlikeornot(id, key2),
                mcom = restaurantrepository.memcomornot(id, key2)
            };
            return View(fuviewmodel);
        }

        [HttpGet]
        public ActionResult managedata(int id)
        {
            var cities = restaurantrepository.takeallcity();
            ViewBag.citues = cities;
            return PartialView(restaurantrepository.GetByID(id));
        }

        [HttpPost]
        public ActionResult managedata(Restaurant source, HttpPostedFileBase c_photo)
        {
            int resid = source.Res_ID;
            Restaurant target = restaurantrepository.GetByID(resid);
            
            restaurantrepository.resdataupdata(target, source, c_photo);
            return Content("修改成功");
        }

        [HttpGet]
        public ActionResult create_n(int resid)
        {
            ViewBag.createphoto_resid = resid;
            return PartialView();
        }

        [HttpPost]
        public ActionResult create_n(Picture target, HttpPostedFileBase thephoto, int c_r_resid)
        {
            restaurantrepository.c_save_res_subintroduction(target, thephoto, c_r_resid);
            return Content("新增成功");
        }

        [HttpPost]
        public ActionResult addtolikeorcancel(int resid,int memid,int flagstatus)
        {
            restaurantrepository.addrestolikeorcancel(resid, memid, flagstatus);
            return Content(flagstatus==0? "已加到收藏": "已取消收藏");
        }

        [HttpGet]
        public ActionResult TypeManage(int id)
        {
            if (_LoginInfo.GetRole(Request) != _LoginInfo._LoginRole.Restaurant)
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }
            else
            {
                TypemanageViewmodel typemanage = new TypemanageViewmodel
                {
                    res = restaurantrepository.GetByID(id),
                    Resthetypes = restaurantrepository.res_type(id),
                    RessubTypes = restaurantrepository.res_subtype(id),
                    nonthetype = restaurantrepository.res_nontype(id),
                    nonsubtype = restaurantrepository.res_nonsubtype(id)
                };
                return PartialView(typemanage);
            }
        }

        public ActionResult savetypechanged(int id, string content, string t_type)
        {
            restaurantrepository.savetype(id, t_type, content);
            return Content("儲存成功!");
        }

        public ActionResult cleartype(int id)
        {
            restaurantrepository.clear_type(id);
            return Content("");
        }

        //餐廳的-照片管理
        public ActionResult r_selfoverview(int resid)
        {
            var target= picturerepository.getres_allphoto(resid);
            return PartialView(target);
            //return View(target);
        }

        [HttpPost]
        public ActionResult c_resself_deletephoto(int p_id)
        {
            picturerepository.deletephotobyid(p_id);
            return Content("刪除成功");
        }

        [HttpPost]
        public ActionResult c_resself_updataphoto(Picture source, HttpPostedFileBase thephoto,int c_r_resid)
        {
            picturerepository.resself_chagephoto(source, thephoto);
            return Content("修改成功");
        }

        [HttpGet]
        public ActionResult rhp_default(int resid)
        {
            rhp_default_viewmodel themodel = new rhp_default_viewmodel
            {
                _resid = resid,
                all_discount =restaurantrepository.r_t_d(resid),
                all_reservation = restaurantrepository.r_t_r(resid),
                all_photo = restaurantrepository.r_t_p(resid),
                all_news = restaurantrepository.r_t_n(resid),
                all_comments = restaurantrepository.r_t_c(resid),
            };
            return PartialView(themodel);
        }


        [HttpGet]
        public ActionResult rhp_photo_overview(int resid)
        {
            rhp_photo_overview_viewmodel themodel = new rhp_photo_overview_viewmodel
            {
                rhp_po_news= restaurantrepository.r_t_n(resid),
                rhp_po_self= restaurantrepository.r_t_p(resid),
            };
            return PartialView(themodel);
        }

        [HttpGet]
        public ActionResult rhp_newandcomment(int resid)
        {
            rhp_newsandcoms themodel = new rhp_newsandcoms
            {
                r_news= restaurantrepository.r_t_n(resid),
                r_coms=restaurantrepository.r_t_c(resid),
            };
            return PartialView(themodel);
        }
        //========================================================================                                   
        //照片總覽
        //[HttpGet]
        //public ActionResult PhotoOverview(int id)
        //{
        //    return PartialView(restaurantrepository.GetByID(id));
        //}

        ////照片管理pw
        //[ChildActionOnly]
        //public ActionResult PhotoManage(string pictureby, int id)
        //{
        //    if (pictureby == "self")
        //    {
        //        ViewBag.bywho = "self";
        //        ViewBag.resid = id;
        //        return PartialView(restaurantrepository.getselfpicture(id));
        //    }
        //    else
        //    {
        //        return PartialView(restaurantrepository.getnewspicture(id));
        //    }
        //}

        //public ActionResult deletepicture(int id, int foroverview)
        //{
        //    picturerepository.Delete(picturerepository.GetByID(id));
        //    return RedirectToAction("PhotoOverview", "Restaurant", new { id = foroverview });
        //}

        //[HttpGet]
        //public ActionResult updatepicture(int id, int foroverview)
        //{
        //    ViewBag.resid = foroverview;
        //    return View(picturerepository.GetByID(id));
        //}

        //[HttpPost]
        //public ActionResult updatepicture(Picture target, HttpPostedFileBase myfile, int foroverview, int picid)
        //{
        //    if (myfile != null)
        //    {
        //        #region tag
        //        ////step1
        //        //string strPath = Request.PhysicalApplicationPath + @"ProductImages\" + ProductImage.FileName;
        //        //ProductImage.SaveAs(strPath);
        //        ////step2
        //        //product.ProductImage = ProductImage.FileName;
        //        ////step3
        //        //int length = ProductImage.ContentLength;
        //        //byte[] image = new byte[length];
        //        //ProductImage.InputStream.Read(image, 0, length);
        //        //product.BytesImage = image;
        //        //db.Products.Add(product);
        //        //db.SaveChanges();
        //        //return RedirectToAction("Index");
        //        #endregion
        //        int length = myfile.ContentLength;
        //        byte[] image = new byte[length];
        //        myfile.InputStream.Read(image, 0, length);
        //        Picture t = picturerepository.GetByID(picid);
        //        t.Pic_title = target.Pic_title;
        //        t.Pic_Content = target.Pic_Content;
        //        t.Pic_Picture = image;
        //        picturerepository.Update(t);

        //        return RedirectToAction("PhotoOverview", "Restaurant", new { id = foroverview });
        //    }
        //    else
        //    {
        //        Picture t = picturerepository.GetByID(picid);
        //        t.Pic_title = target.Pic_title;
        //        t.Pic_Content = target.Pic_Content;
        //        picturerepository.Update(t);
        //        return RedirectToAction("PhotoOverview", "Restaurant", new { id = foroverview });
        //    }

        //}

        public ActionResult getdbimage(string table, int id)
        {
            try
            {
                switch (table)
                {
                    case "restaurant":
                        return File(restaurantrepository.getimagebyid(id), "image/jpeg");
                    case "picture":
                        return File(picturerepository.getimagebyid(id), "image/jpeg");
                }
                return File(restaurantrepository.getimagebyid(id), "image/jpeg");
            }
            catch
            {
                return File("~/tasteitlogo/logo.png", "image/png");
            }
            
        }
        #endregion


        #region Wei
        public ActionResult RestaurantIndex()
        {

            return View();
        }
        #endregion


        #region Han
        public ActionResult ResQAShow(int? AdmID, int? MemID)
        {
            return View(QARepository.GetByIDForRestaurant(MemID, AdmID, Request));

        }
        [HttpPost]
        public ActionResult ResAddToQa(string QA_Content, int? QA_AdministratorID, int? QA_MemberID)
        {
            QARepository.AddToQA(QA_Content, QA_MemberID, QA_AdministratorID, null, Request);
            return RedirectToAction("ResQAShow");
        }

        public ActionResult ResChatPartial(int? AdmID, int? MemID)
        {
            return PartialView(QARepository.GetByIDForRestaurant(MemID, AdmID, Request));

        }
        public ActionResult ResLeftChatPartial()
        {
            var x = QARepository.GetByIDForRestaurant(null, null, Request);
            return PartialView(x);
        }
        #endregion


        #region Fu

        public ActionResult RestNewsOverView(int id)
        {
            var q = NewsRepository.GetByRestID(id);
            var x = q.OrderByDescending(L => L.New_CreateDate);
            if (x != null)
            {
                if (Request.GetRole() == _LoginInfo._LoginRole.Member)
                {
                    ViewBag.MemberID = Request.GetRoleKey().Value;
                    return PartialView(x.Take(3));

                }
                else if (Request.GetRole() == _LoginInfo._LoginRole.Guest)
                {
                    ViewBag.Guest = "Guest";
                    return PartialView(x.Take(3));
                }
            }
            return PartialView(x.Take(3));
        }

        public ActionResult test()
        {
            return View();
        }

        //申請廣告
        public ActionResult BuyNews()
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Restaurant)
            {
                Restaurant RestN = restaurantrepository.GetByID(Request.GetRoleKey().Value);
                ViewBag.RestName = RestN.Res_Name;
                ViewBag.RestID = Request.GetRoleKey().Value;
                return View();
            }
            else
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }
        }
        [HttpPost]
        public ActionResult BuyNews(News news)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Restaurant)
            {
                News CreateNews = new News();
                CreateNews.New_Title = "BuyNews";
                CreateNews.New_RestaurantID = news.New_RestaurantID;
                CreateNews.New_OrderDate = DateTime.Now;
                NewsRepository.Insert(CreateNews);
                Report CreateReport = new Report();
                if (news.New_Content != null)
                {
                    CreateReport.Rep_NewsID = CreateNews.New_ID;
                    CreateReport.Rep_RestaurantID = news.New_RestaurantID;
                    CreateReport.Rep_Content = "商案申請" + CreateNews.New_ID + ":" + news.New_Content;
                    CreateReport.Rep_Status = 1008;
                    CreateReport.Rep_AccuseTypeID = 11;
                    CreateReport.Rep_CreatedDate = DateTime.Now;
                    ReportRepository.Insert(CreateReport);
                    //return RedirectToAction("RestaurantIndex", "Restaurant");
                    return Content("申請完成");
                }
                else
                {
                    return Content("請輸入商案需求");
                }
            }
            else
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }

        }
        //廣告總覽
        public ActionResult RestaurantBlogOverview()
        {
            //要抓登入的餐廳ID
            //var q = from n in db.News
            //        where n.New_RestaurantID == id
            //        select n;
            if (Request.GetRole() == _LoginInfo._LoginRole.Restaurant)
            {
                var q = NewsRepository.GetByRestID(Request.GetRoleKey().Value);

                return View(q);
            }
            else
            {
                return RedirectToAction("RestaurantLogin", "Home");
            }
        }
        //查看本餐廳的所有評論
        public ActionResult RestaurantCommentOverview(int id)
        {
            //var q = CommentRepository.GetAll();
            //return View(q);
            //if (Request.GetRole() == _LoginInfo._LoginRole.Restaurant)
            //
            //int id = Request.GetRoleKey().Value;
            var q = CommentRepository.GetByRestID(id);
            q = q.OrderByDescending(L => L.Com_CreateDate).ToList();
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                q = q.Where(L => L.Com_MemberID != Request.GetRoleKey().Value);
            }

            return PartialView(q.Take(3));
            //}
            //else
            //{
            //    return RedirectToAction("RestaurantLogin", "Home");
            //}
        }
        //評論框
        public ActionResult CommentBlock(int id)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }

            ViewBag.RestID = id;

            return PartialView();
        }

        [HttpPost]
        public ActionResult CommentBlock(Comment comment)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            if (comment.Com_Content != null)
            {
                comment.Com_CreateDate = DateTime.Now;
                comment.Com_MemberID = Request.GetRoleKey().Value;
                CommentRepository.Insert(comment);
                return Content("已完成評論");
            }
            else
            {
                return Content("請輸入內容");
            }
        }
        //餐廳檢舉對自己的不當食記
        [HttpPost]
        public ActionResult ReportNews_Restaurant(Report report)
        {
            Report CreateReport = new Report();
            var q = ReportRepository.GetByNewsID((int)report.Rep_NewsID);
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                q = q.Where(m => m.Rep_MemberID == Request.GetRoleKey().Value && m.Rep_Status == 1);
                if (q.FirstOrDefault() == null)
                {
                    CreateReport.Rep_MemberID = Request.GetRoleKey().Value;
                    CreateReport.Rep_AccuseTypeID = 8;
                    CreateReport.Rep_Content = report.Rep_Content;
                    CreateReport.Rep_CreatedDate = DateTime.Now;
                    CreateReport.Rep_Status = 1;
                    CreateReport.Rep_NewsID = report.Rep_NewsID;
                    ReportRepository.Insert(CreateReport);
                    return Content("進入審核階段");
                }
                else
                {
                    return Content("已經檢舉過囉!!!");
                }
            }
            if (Request.GetRoleKey().Value == report.Rep_RestaurantID)
            {
                q = q.Where(m => m.Rep_RestaurantID == Request.GetRoleKey().Value && m.Rep_Status == 1);
                if (q.FirstOrDefault() == null)
                {
                    CreateReport.Rep_RestaurantID = report.Rep_RestaurantID;
                    CreateReport.Rep_AccuseTypeID = 8;
                    CreateReport.Rep_Content = report.Rep_Content;
                    CreateReport.Rep_CreatedDate = DateTime.Now;
                    CreateReport.Rep_Status = 1;
                    CreateReport.Rep_NewsID = report.Rep_NewsID;
                    ReportRepository.Insert(CreateReport);
                    return Content("進入審核階段");
                }
                else
                {
                    return Content("已經檢舉過囉!!!");
                }
            }
            return null;
        }

        //餐廳檢舉對自己的不當評論
        [HttpPost]
        public ActionResult ReportComment_Restaurant(Report report)
        {

            var q = ReportRepository.GetByCommentID((int)report.Rep_CommentID);
            Report CreateReport = new Report();
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {

                q = q.Where(m => m.Rep_MemberID == Request.GetRoleKey().Value && m.Rep_Status == 1);
                if (q.FirstOrDefault() == null)
                {
                    CreateReport.Rep_MemberID = Request.GetRoleKey().Value;
                    CreateReport.Rep_Content = report.Rep_Content;
                    CreateReport.Rep_CreatedDate = DateTime.Now;
                    CreateReport.Rep_Status = 1;
                    CreateReport.Rep_CommentID = report.Rep_CommentID;
                    CreateReport.Rep_AccuseTypeID = 8;
                    ReportRepository.Insert(CreateReport);
                    return Content("進入審核階段");
                }
                else
                {
                    return Content("已經檢舉過囉!!!");
                }
            }

            if (Request.GetRoleKey().Value == report.Rep_RestaurantID)
            {
                q = q.Where(m => m.Rep_RestaurantID == Request.GetRoleKey().Value && m.Rep_Status == 1);

                if (q.FirstOrDefault() == null)
                {
                    CreateReport.Rep_RestaurantID = report.Rep_RestaurantID;
                    CreateReport.Rep_Content = report.Rep_Content;
                    CreateReport.Rep_CreatedDate = DateTime.Now;
                    CreateReport.Rep_Status = 1;
                    CreateReport.Rep_CommentID = report.Rep_CommentID;
                    CreateReport.Rep_AccuseTypeID = 8;
                    ReportRepository.Insert(CreateReport);
                    return Content("進入審核階段");
                }
                else
                {
                    return Content("已經檢舉過囉!!!");
                }
            }

            return null;
        }
        #endregion




    }
}