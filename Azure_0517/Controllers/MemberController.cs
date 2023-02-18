using PagedList;
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
    public class MemberController : Controller
    {
        MemberRepository MemRepository = new MemberRepository();
        ReservationBookedRepository BookedRepository = new ReservationBookedRepository();
        NewsRepository NewsRepository = new NewsRepository();
        CommentRepository CommentRepository = new CommentRepository();
        GiftRepository repositoryGift = new GiftRepository();
        GiftDetailRepository repositoryGift_Detail = new GiftDetailRepository();
        CartService cart = new CartService();
        Taste_ItEntities db = new Taste_ItEntities();
        QAsystemRepository QARepository = new QAsystemRepository();
        ReportRepository ReportRepository = new ReportRepository();
        RestaurantRepository RestaurantRepository = new RestaurantRepository();

        #region Jun

        [HttpGet]
        public ActionResult RestaurantInPocket()
        {
            if (_LoginInfo.GetRole(Request) != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            else
            {
                IEnumerable<News> news = MemRepository.mylikenews(Request.GetRoleKey().Value);



                return View();
            }
        }

        public ActionResult RestaurantInPocketList()
        {

            IEnumerable<Restaurant> restaurants = MemRepository.mylikeres(Request.GetRoleKey().Value);
            string orderBy = Request.QueryString["orderBy"];
            switch (orderBy)
            {
                case "消費金額(升序)":
                    restaurants = restaurants.OrderBy(L => L.Res_MinConsumption.Value + L.Res_MaxConsumption.Value)
                        .ThenBy(L => L.Res_MinConsumption.Value);
                    break;
                case "消費金額(降序)":
                    restaurants = restaurants.OrderByDescending(L => L.Res_MinConsumption.Value + L.Res_MaxConsumption.Value)
                        .ThenByDescending(L => L.Res_MinConsumption.Value);
                    break;
                case "評價":
                default:
                    restaurants = restaurants.OrderByDescending(L => L.Res_StarRate.HasValue).ThenByDescending(L => L.Res_StarRate);
                    break;
            }

            ViewBag.PageCount = 0;
            ViewBag.PageCount = (restaurants.Count() + 4) / 5;

            int page = Request.QueryString["Page"] == null ? 0 : Convert.ToInt32(Request.QueryString["Page"]) - 1;
            const int PageItem = 5;

            return PartialView(restaurants.Skip(page * PageItem).Take(PageItem));
        }

        /// <summary>
        /// CHUCHUCHUCHUCHUCHUCHUCHUCHUCHUCHUCHUCHUCHU
        /// </summary>
        /// <returns></returns>



        [HttpGet]//訂位紀錄
        public ActionResult BookOverview()
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }

            return View();
        }

        [HttpGet]
        public ActionResult BookManage(int id)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            ReservationBooked booked = BookedRepository.GetByID(id);
            var jsonObj = new { booked.ReminderText, booked.RVBooked_Seats };
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BookManage(ReservationBooked booked)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }

            ReservationBooked edited = BookedRepository.GetByID(booked.RVBooked_ID);
            edited.ReminderText = booked.ReminderText;
            edited.RVBooked_Seats = booked.RVBooked_Seats;
            ReservationBooked OriJSON = BookedRepository.GetByID(BookedRepository.UpdateGetNewID(edited).Value);
            var jsonObj = new
            {
                OriJSON.RVBooked_ID,
                OriJSON.RVBooked_Seats,
                OriJSON.ReminderText,
                OriJSON.BackUpNo
            };
            //BookedRepository.Update(booked);
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BookedBlock(int dateTime, int? page)
        {
            int index = page.HasValue ? page.Value - 1 : 0;
            int? id = Request.GetRoleKey().Value;
            var temp = BookedRepository.GetByMemberID(id.Value, dateTime);
            ViewBag.DataCount = temp.Count();
            return PartialView(temp.Skip(index * 3).Take(3));
        }

        [HttpPost]
        public void DeleteBook(int Bookid)
        {
            BookedRepository.Delete(BookedRepository.GetByID(Bookid));
            return;// RedirectToAction("BookOverview");
        }


        #endregion


        #region Chu
        //Response.Cookies["Role"].Value = "Mem";//Rest//Adm
        //Response.Cookies["Key"].Value = "id";
        public ActionResult mem_leftview()
        {
            if (_LoginInfo.GetRole(Request) != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            else
            {
                return PartialView(MemRepository.GetByID(Request.GetRoleKey().Value));
            }
        }


        public ActionResult getdbimage(int id)
        {
            try
            {
                return File(MemRepository.getmemphotobuid(id), "image/jpeg");
            }
            catch
            {
                return File("~/tasteitlogo/user.png", "image/png");
            }

        }


        //基本資料!!!
        [HttpGet]
        public ActionResult SelfInfo()
        {
            if (_LoginInfo.GetRole(Request) != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            else
            {
                return View(MemRepository.GetByID(Request.GetRoleKey().Value));
            }
        }
        [HttpPost]
        public ActionResult SelfInfo(Member mem, HttpPostedFileBase myfile, int memid)
        {
            if (myfile != null)
            {
                #region tag
                ////step1
                //string strPath = Request.PhysicalApplicationPath + @"ProductImages\" + ProductImage.FileName;
                //ProductImage.SaveAs(strPath);
                ////step2
                //product.ProductImage = ProductImage.FileName;
                ////step3
                //int length = ProductImage.ContentLength;
                //byte[] image = new byte[length];
                //ProductImage.InputStream.Read(image, 0, length);
                //product.BytesImage = image;
                //db.Products.Add(product);
                //db.SaveChanges();
                //return RedirectToAction("Index");
                #endregion
                int length = myfile.ContentLength;
                byte[] image = new byte[length];
                myfile.InputStream.Read(image, 0, length);
                Member m = MemRepository.GetByID(memid);
                m.Mem_Photo = image;
                //m.id=mem.id....
                m.Mem_Name = mem.Mem_Name;
                m.Mem_NickName = mem.Mem_NickName;
                m.Mem_Mail = mem.Mem_Mail;
                m.Mem_Phone = mem.Mem_Phone;
                m.Mem_Password = mem.Mem_Password;
                m.Mem_Introduction = mem.Mem_Introduction;
                m.Mem_Birthday = mem.Mem_Birthday;
                m.Mem_Address = mem.Mem_Address;
                MemRepository.Update(m);
                return RedirectToAction("SelfInfo", "Member");
            }
            else
            {
                Member m = MemRepository.GetByID(memid);
                //m.id=mem.id....
                m.Mem_Name = mem.Mem_Name;
                m.Mem_NickName = mem.Mem_NickName;
                m.Mem_Mail = mem.Mem_Mail;
                m.Mem_Phone = mem.Mem_Phone;
                m.Mem_Password = mem.Mem_Password;
                m.Mem_Introduction = mem.Mem_Introduction;
                m.Mem_Birthday = mem.Mem_Birthday;
                m.Mem_Address = mem.Mem_Address;
                MemRepository.Update(m);
                return RedirectToAction("SelfInfo", "Member");
            }
        }
        //會員頭相框pw
        [ChildActionOnly]
        public ActionResult HeadPhotoBlock(int id)
        {
            return PartialView(MemRepository.GetByID(id));
        }
        #endregion


        #region Sam Part
        //獎品兌換
        public ActionResult GiftExchange(int page = 1)
        {
            var list = repositoryGift.Search(String.Empty, String.Empty);
            return View(list.ToPagedList(page, 8));
        }

        //更新獎品兌換清單
        [HttpPost]
        public ActionResult RefreshList(int page, string sortOrder, string searchString)
        {
            var list = repositoryGift.Search(sortOrder, searchString);
            var result = list.ToPagedList(page, 8);

            return PartialView("_GiftList", result);
        }

        //兌換記錄
        public ActionResult ExchangeLog()
        {

            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                var id = Convert.ToInt32(Request.Cookies["Key"].Value); //MemID
                var list = repositoryGift_Detail.ExchangeLogSearchDate(id, String.Empty, null, null);
                return View(list);
            }
            else
            {
                return Content($"<script>alert('請先登入會員');location.href='{Url.Action("MemberLogin", "Home")}'</script>"); //回登入頁面,正常情況不會執行
            }
        }

        //更新兌換記錄清單
        public ActionResult RefreshLog(string sortOrder, DateTime? date1, DateTime? date2)
        {
            var memID = Convert.ToInt32(Request.Cookies["Key"].Value);
            var list = repositoryGift_Detail.ExchangeLogSearchDate(memID, sortOrder, date1, date2);
            return PartialView("_ExchangeLogList", list);
        }

        //加入購物車
        public ActionResult AddToCart(int id = 0)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                cart.AddToCart(Convert.ToInt32(Request.Cookies["Key"].Value), id);
                return PartialView("_CartPartial");
            }
            else
            {
                return Content("請先登入會員");
            }
        }

        //購物車清單
        [HttpGet]
        public ActionResult ShoppingCartList()
        {
            ViewBag.Gift = db.Gifts;
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                var id = Convert.ToInt32(Request.Cookies["Key"].Value); //MemID

                //某個Mem有GetDate為null值的購物車清單存在
                var List = db.Gift_Detail.FirstOrDefault(x => x.GiftDet_MemberID == id && x.GiftDet_GetDate == null);
                if (List != null)
                {
                    return View(cart.CartList(id));
                }
                else
                {
                    return Content($"<script>alert('購物車沒有資料，請先購物');location.href='{Url.Action("GiftExchange", "Member")}'</script>");
                }
            }
            else
            {
                return Content($"<script>alert('請先登入會員');location.href='{Url.Action("MemberLogin", "Home")}'</script>"); //回登入頁面,正常情況不會執行
            }
        }

        //購物車結帳
        public ActionResult CartCheckOut()
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                var memID = Convert.ToInt32(Request.Cookies["Key"].Value);

                if (repositoryGift_Detail.CheckOut(memID))
                {
                    return Content("結帳完成，回到兌換頁面");
                }
                else
                {
                    return Content("點數不足");
                }
            }
            else
            {
                return Content("請先登入會員"); //回登入頁面,正常情況不會執行
            }
        }


        //購物車頁面輸入數量變動
        public ActionResult GiftQuantityChange(int GD_ID, int Qty)
        {
            var x = repositoryGift_Detail.GetByID(GD_ID);
            x.GiftDet_GetQuantity = Qty;
            repositoryGift_Detail.Update(x);
            return Content($"{x.GiftDet_GetQuantity * x.Gift.Gift_Point}");
        }

        //刪除購物車品項
        public ActionResult DeleteCartItem(int id = 0)
        {
            var memID = Convert.ToInt32(Request.Cookies["Key"].Value);
            repositoryGift_Detail.DeleteCartItem(id, memID);

            return PartialView("_CartPartial");
        }

        //清空購物車品項
        public ActionResult ClearCart()
        {
            var memID = Request.GetRoleKey().Value;
            repositoryGift_Detail.ClearCart(memID);

            return PartialView("_CartPartial");
        }



        #region Sam PartialView
        //商品框
        public ActionResult GiftBlock(int id = 1)
        {
            Gift gift = repositoryGift.GetByID(id);
            return PartialView(gift);
        }

        public ActionResult _CartPartial()
        {
            //var memID = Convert.ToInt32(Request.Cookies["Key"].Value);
            //var list = repositoryGift_Detail.SearchList(memID);
            //var sum = 0;
            //var count = 0;

            //foreach (var item in list)
            //{
            //    sum += item.Gift.Gift_Point * item.GiftDet_GetQuantity;
            //    count += item.GiftDet_GetQuantity;
            //}
            //ViewBag.sum = sum;
            //ViewBag.count = count;

            //var result = list.ToPagedList(1, 99);
            return PartialView(/*result*/);
        }

        public ActionResult _ExchangeLogList()
        {
            return PartialView();
        }
        #endregion


        #endregion


        #region Wei
        public ActionResult MemberIndex()
        {

            return View();
        }
        #endregion


        #region Han
        public ActionResult QAShow(/*int? MemberID,*/int? AdmID, int? ResID)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                return PartialView(QARepository.GetByIDForMember(AdmID, ResID, Request));
            }
            else if (Request.GetRole() == _LoginInfo._LoginRole.Guest)
            {
                return Content("<script>alert('請先登入再進行提問');location.href='/Home/MemberLogin'</script>");
            }
            else if (Request.GetRole() == _LoginInfo._LoginRole.Restaurant)
            {
                return RedirectToAction("ResQAShow", "Restaurant", new { AdmID = 3 });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            //首頁還不知道是哪個
            //location.href後面那個寫法要再研究，因為不能寫~/，感覺這個寫法放到IIS發布之後會出錯
        }
        [HttpPost]
        public ActionResult AddToQa(string QA_Content, int? QA_AdministratorID, int? QA_RestaurantID)
        {
            QARepository.AddToQA(QA_Content, null, QA_AdministratorID, QA_RestaurantID, Request);
            return RedirectToAction("QAShow");
        }
        public ActionResult MemberChatPartial(int? AdmId, int? ResId)
        {
            return PartialView(QARepository.GetByIDForMember(AdmId, ResId, Request));
        }
        public ActionResult MemLeftChatPartial()
        {
            var x = QARepository.GetByIDForMember(null, null, Request);
            return PartialView(x);
        }
        #endregion


        #region Fu

        [HttpGet]
        public ActionResult BlogInPocket()
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                //int id = Request.GetRoleKey().Value;
                var q = NewsRepository.GetByMyLove(Request.GetRoleKey().Value);
                return View(q);
            }
            else
            {
                return RedirectToAction("MemberLogin", "Home");
            }

        }
        [HttpGet]
        public ActionResult BlogInPocketList(BlogSreach blogSreach)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                var q = (NewsRepository.GetByMyLove(Request.GetRoleKey().Value));
                //int id = Request.GetRoleKey().Value;

                ViewBag.PageCount = 0;


                if (blogSreach.BlogResturant != null)
                {
                    q = q.Where(L => L.Restaurant.Res_Name.Contains(blogSreach.BlogResturant));
                }
                if (blogSreach.BlogYear != 0)
                {
                    q = q.Where(L => L.New_CreateDate.Value.Year == blogSreach.BlogYear);
                }
                if (blogSreach.BlogMonth != 0)
                {
                    q = q.Where(L => L.New_CreateDate.Value.Month == blogSreach.BlogMonth);
                }
                if (blogSreach.myorder == "up")
                {
                    q = q.OrderByDescending(L => L.New_CreateDate);
                }
                if (blogSreach.myorder == "down")
                {
                    q = q.OrderBy(L => L.New_CreateDate);
                }
                ViewBag.PageCount = (q.Count() + 4) / 5;
                int page = Request.QueryString["Page"] == null ? 0 : Convert.ToInt32(Request.QueryString["Page"]) - 1;
                const int PageItem = 5;

                return PartialView("BlogInPocketList", q.Skip(page * PageItem).Take(PageItem));

            }

            else
            {
                return RedirectToAction("MemberLogin", "Home");
            }
        }

        public ActionResult BlogSreach()
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                //int id = Request.GetRoleKey().Value;
                var q = NewsRepository.GetByMemberID(Request.GetRoleKey().Value);
                return View(q);
            }
            else
            {
                return RedirectToAction("MemberLogin", "Home");
            }

        }

        //刪除食記 有改!
        public ActionResult DeleteNews_Member(int id = 0)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            var report = ReportRepository.GetByNewsID(id);

            foreach (var item in report)
            {
                ReportRepository.Delete(item);
            }

            News news = NewsRepository.GetByID(id);
            NewsRepository.Delete(news);
            return RedirectToAction("BlogSreach", "Member");
        }
        //刪除評論 有改!
        public ActionResult DeleteComment_Member(int id = 0)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member && Request.GetRole() != _LoginInfo._LoginRole.Administrator)
            {
                //int id = Request.GetRoleKey().Value;
                return RedirectToAction("MemberLogin", "Home");
            }
            var report = ReportRepository.GetByCommentID(id);

            foreach (var item in report)
            {
                ReportRepository.Delete(item);
            }
            Comment comment = CommentRepository.GetByID(id);
            var x = comment.Com_RestaurantID;
            CommentRepository.Delete(comment);

            return Content("");
        }
        //食記管理

        public ActionResult BlogOverview(BlogSreach blogSreach)
        {
            //if(Request.Cookies["Role"].Value == "Mem")
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                var q = (NewsRepository.GetByMemberID(Request.GetRoleKey().Value));
                //int id = Request.GetRoleKey().Value;

                ViewBag.PageCount = 0;


                if (blogSreach.BlogResturant != null)
                {
                    q = q.Where(L => L.Restaurant.Res_Name.Contains(blogSreach.BlogResturant));
                }
                if (blogSreach.BlogYear != 0)
                {
                    q = q.Where(L => L.New_CreateDate.Value.Year == blogSreach.BlogYear);
                }
                if (blogSreach.BlogMonth != 0)
                {
                    q = q.Where(L => L.New_CreateDate.Value.Month == blogSreach.BlogMonth);
                }
                if (blogSreach.myorder == "up")
                {
                    q = q.OrderByDescending(L => L.New_CreateDate);
                }
                if (blogSreach.myorder == "down")
                {
                    q = q.OrderBy(L => L.New_CreateDate);
                }
                ViewBag.PageCount = (q.Count() + 4) / 5;
                int page = Request.QueryString["Page"] == null ? 0 : Convert.ToInt32(Request.QueryString["Page"]) - 1;
                const int PageItem = 5;

                return PartialView("_BlogOverview", q.Skip(page * PageItem).Take(PageItem));

            }

            else
            {
                return RedirectToAction("MemberLogin", "Home");
            }

        }
        //評論管理
        public ActionResult CommentOverview(int id)
        {
            //var q = CommentRepository.GetAll();
            //return View(q);
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                //int id = Request.GetRoleKey().Value;
                var q = CommentRepository.GetByMemberID(Request.GetRoleKey().Value);
                var x = q.Where(L => L.Com_RestaurantID == id).FirstOrDefault();
                Member m = MemRepository.GetByID(Request.GetRoleKey().Value);
                ViewBag.NickName = m.Mem_NickName;
                ViewBag.MyPhoto = m.Mem_Photo;
                return PartialView(x);
            }
            else
            {
                return null;
            }
        }

        //修改食記
        public ActionResult BlogUpdate(int id)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Member || Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                //int id = Request.GetRoleKey().Value;
                News news = NewsRepository.GetByID(id);
                return View(news);
            }
            else
            {
                return RedirectToAction("MemberLogin", "Home");
            }
        }

        public ActionResult BlogDemo(int id)
        {
            News news = NewsRepository.GetByID(id);
            return Content(news.New_Content);
        }

        [HttpPost]
        public ActionResult BlogUpdate(News news)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member && Request.GetRole() != _LoginInfo._LoginRole.Administrator)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            News CreateNews = NewsRepository.GetByID(news.New_ID);
            CreateNews.New_Title = news.New_Title;
            CreateNews.New_Content = news.New_Content;
            CreateNews.New_MemberID = Request.GetRoleKey().Value;
            CreateNews.New_RestaurantID = news.New_RestaurantID;
            CreateNews.New_CreateDate = DateTime.Now;
            NewsRepository.Update(CreateNews);
            if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("BlogSreach", "Member");
            }
            else if (Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                return RedirectToAction("BusinessCase", "Administrator");
            }
            return RedirectToAction("MemberLogin", "Home");
        }

        public ActionResult UpdateComment(int id)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Member || Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                //int id = Request.GetRoleKey().Value;
                Comment com = CommentRepository.GetByID(id);
                return View(com);
            }
            else
            {
                return RedirectToAction("MemberLogin", "Home");
            }
        }
        [HttpPost]
        public ActionResult UpdateComment(Comment comment)
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Member && Request.GetRole() != _LoginInfo._LoginRole.Administrator)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            Comment CreateCom = CommentRepository.GetByID(comment.Com_ID);
            CreateCom.Com_StarRate = comment.Com_StarRate;
            CreateCom.Com_Content = comment.Com_Content;
            CreateCom.Com_CreateDate = DateTime.Now;
            CreateCom.Com_RestaurantID = comment.Com_RestaurantID;
            CommentRepository.Update(CreateCom);

            var q = CommentRepository.GetByMemberID(Request.GetRoleKey().Value);
            var x = q.Where(L => L.Com_RestaurantID == comment.Com_RestaurantID).FirstOrDefault();
            return Content("修改成功");


        }
        #endregion




    }
}