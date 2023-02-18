using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azure_0517.Models;
using Azure_0517.Models.Repository;
using Azure_0517.Models.ViewModel;
using Azure_0513.Models.ViewModel.Sam;

namespace Azure_0517.Controllers
{
    public class AdministratorController : Controller
    {
        private RestaurantRepository ResRepository = new RestaurantRepository();
        private MemberRepository MemRepository = new MemberRepository();
        private IRepository<Report> Prosecution_repository = new GenericRepository<Report>();
        private ProsecutionData Manage = new ProsecutionData();
        private ReportRepository ReportRepository = new ReportRepository();
        private GiftRepository repositoryGift = new GiftRepository();
        private GiftDetailRepository repositoryGift_Detail = new GiftDetailRepository();
        private CartService cart = new CartService();
        private QAsystemRepository QARepository = new QAsystemRepository();
        Taste_ItEntities db = new Taste_ItEntities();
		
		#region Jun
		public ActionResult Refresh()
        {
            List<Restaurant> restaurants = db.Restaurants.ToList();
            foreach (var x in restaurants)
            {
                IEnumerable<Comment> comments = x.Comments;
                if (comments.Count() != 0)
                {
                    x.Res_StarRate = (decimal?)comments.Select(L => L.Com_StarRate).Average();
                }

            }
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
		#endregion
	
	
        #region Chu
        public ActionResult getimage(int id, string flag)
        {
            try
            {
                if (flag == "mem")
                {
                    return File(MemRepository.getmemphotobuid(id), "image/jpeg");
                }
                else
                {
                    return File(ResRepository.getimagebyid(id), "image/jpeg");
                }
            }
            catch
            {
                return File("~/tasteitlogo/user.png", "image/png");
            }

        }

        //管理會員總覽
        public ActionResult MemberOverview()
        {
            if (_LoginInfo.GetRole(Request) != _LoginInfo._LoginRole.Administrator)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            else
            {
                ViewBag.thecount = MemRepository.Getexcludeadm().Count();
                return View(MemRepository.GetByID(Request.GetRoleKey().Value));
            }
        }

        public ActionResult memoverview(int thepage = 1)
        {
            var mem = MemRepository.Getexcludeadm().AsQueryable();
            return PartialView(mem.ToList().ToPagedList(thepage, 5));
        }

        //管理會員專區
        [HttpGet]
        public ActionResult MemberManage(int id)
        {
            return View(MemRepository.GetByID(id));
        }
        [HttpPost]
        public ActionResult MemberManage(Member target, HttpPostedFileBase myfile)
        {
            Member t = MemRepository.GetByID(target.Mem_ID);
            t.Mem_Name = target.Mem_Name;
            t.Mem_NickName = target.Mem_NickName;
            t.Mem_Account = target.Mem_Account;
            t.Mem_Password = target.Mem_Password;
            t.Mem_Address = target.Mem_Address;
            t.Mem_Birthday = target.Mem_Birthday;
            t.Mem_Mail = target.Mem_Mail;
            t.Mem_Phone = target.Mem_Phone;
            t.Mem_Point = target.Mem_Point;
            t.Mem_Exp = target.Mem_Exp;
            if (myfile != null)
            {
                int length = myfile.ContentLength;
                byte[] image = new byte[length];
                myfile.InputStream.Read(image, 0, length);
                t.Mem_Photo = image;
            }

            MemRepository.Update(t);

            return RedirectToAction("MemberOverview", "Administrator");
        }
        //管理餐廳總覽
        public ActionResult RestaurantOverview()
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Administrator)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            ViewBag.thecount = ResRepository.GetAll().Count();
            return View(MemRepository.GetByID(Request.GetRoleKey().Value));
        }

        public ActionResult resoverview(int thepage = 1)
        {
            var res = ResRepository.GetAll().AsQueryable();
            return PartialView(res.ToList().ToPagedList(thepage, 50));
        }

        //管理餐廳專區
        [HttpGet]
        public ActionResult RestaurantManage(int id)
        {
            if (_LoginInfo.GetRole(Request) != _LoginInfo._LoginRole.Administrator)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            else
            {
                var cities = ResRepository.takeallcity();
                ViewBag.citues = cities;
                return View(ResRepository.GetByID(id));
            }
        }
        [HttpPost]
        public ActionResult RestaurantManage(Restaurant target, HttpPostedFileBase c_photo)
        {
            Restaurant res = ResRepository.GetByID(target.Res_ID);
            res.Res_Name = target.Res_Name;
            res.Res_Account = target.Res_Account;
            res.Res_Password = target.Res_Password;
            res.Res_City = target.Res_City;
            res.Res_Address = target.Res_Address;
            res.Res_Email = target.Res_Email;
            res.Res_Phone = target.Res_Phone;
            res.Res_Seats = target.Res_Seats;
            res.Res_ParkingSeats = target.Res_ParkingSeats;
            res.Res_MaxConsumption = target.Res_MaxConsumption;
            res.Res_MinConsumption = target.Res_MinConsumption;
            if (c_photo != null)
            {
                int length = c_photo.ContentLength;
                byte[] image = new byte[length];
                c_photo.InputStream.Read(image, 0, length);
                res.Res_HomePagePicture = image;
            }

            ResRepository.Update(res);
            return RedirectToAction("RestaurantOverview", "Administrator");
        }

        #endregion


        #region Sam Part

        //獎品管理  
        public ActionResult GiftOverview(int page = 1)
        {
            var list = repositoryGift.Search(String.Empty, String.Empty);

            return View(list.ToPagedList(page, 5));
        }

        //更新獎品管理清單
        [HttpPost]
        public ActionResult RefreshList(int page, string sortOrder, string searchString)
        {
            var list = repositoryGift.Search(sortOrder, searchString);
            var result = list.ToPagedList(page, 5);

            return PartialView("_GiftManagement", result);
        }


        //新增獎品
        [HttpGet]
        public ActionResult GiftCreate()
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                var id = Request.GetRoleKey();
                ViewBag.adm = db.Members.Where(x => x.Mem_ID == id).Select(m => m.Mem_Name).FirstOrDefault();
                return View();
            }
            else
            {
                return Content($"<script>alert('管理員請先登入');location.href = '#popupM'</script>");
            }
        }

        //新增獎品
        [HttpPost]
        public ActionResult GiftCreate([Bind(Exclude = "Gift_Picture")] Gift gift, HttpPostedFileBase Gift_Picture)
        {
            var id = Request.GetRoleKey();
            ViewBag.adm = db.Members.Where(x => x.Mem_ID == id).Select(m => m.Mem_Name).FirstOrDefault();
            if (Gift_Picture != null)
            {
                int imgSize = Gift_Picture.ContentLength;
                byte[] imgByte = new byte[imgSize];
                Gift_Picture.InputStream.Read(imgByte, 0, imgSize);
                gift.Gift_Picture = imgByte;
                gift.Gift_CreatedAdmini = (int)id;
                repositoryGift.Insert(gift);
                return RedirectToAction("GiftOverview");
            }
            else
            {
                string filename = Request.PhysicalApplicationPath + "/tasteitlogo/Gift.jpg";
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                int length = (int)fs.Length;
                byte[] image = new byte[length];
                fs.Read(image, 0, length);
                gift.Gift_Picture = image;
                gift.Gift_CreatedAdmini = (int)id;
                repositoryGift.Insert(gift);
                return RedirectToAction("GiftOverview");
            }
        }

        //刪除確認
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                Gift gift = repositoryGift.GetByID(id);
                if (gift == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    repositoryGift.Delete(gift);
                    return Content("刪除成功");
                }
            }
            else
            {
                return Content($"<script>alert('管理員請先登入');location.href='{Url.Action("MemberLogin","Home")}'</script>");
            }
        }

        //獎品修改
        [HttpGet]
        public ActionResult GiftEdit(int id)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                ViewBag.Gift_Picture = repositoryGift.GetByID(id).Gift_Picture;
                Gift gift = repositoryGift.GetByID(id);
                if (gift == null)
                {
                    return HttpNotFound();
                }
                return View(gift);
            }
            else
            {
                return Content($"<script>alert('管理員請先登入');location.href='{Url.Action("MemberLogin", "Home")}';</script>");
            }
        }

        //獎品修改
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult GiftEdit([Bind(Exclude = "Gift_Picture")]Gift gift, HttpPostedFileBase ReplacePicture)
        {
            var id = Request.GetRoleKey();
            if (ReplacePicture != null)
            {
                int imgSize = ReplacePicture.ContentLength;
                byte[] imgByte = new byte[imgSize];
                ReplacePicture.InputStream.Read(imgByte, 0, imgSize);
                gift.Gift_Picture = imgByte;
                gift.Gift_CreatedAdmini = (int)id;

                repositoryGift.Update(gift);
                return RedirectToAction("GiftOverview");
            }

            Gift newGift = repositoryGift.GetByID(gift.Gift_ID);
            //newGift.Gift_ID 
            newGift.Gift_Name = gift.Gift_Name;
            newGift.Gift_Content = gift.Gift_Content;
            newGift.Gift_Point = gift.Gift_Point;
            newGift.Gift_CreatedAdmini = (int)id;
            //newGift.Gift_CreatedDate = DateTime.Now;
            newGift.Gift_Quantity = gift.Gift_Quantity;
            //newGift.Gift_Picture

            repositoryGift.Update(newGift);
            return RedirectToAction("GiftOverview");
        }

        //會員性別比
        public ActionResult GenderPie()
        {
            int male = db.Members.Where(x => x.Mem_Gender == true).Count();
            int female = db.Members.Where(x => x.Mem_Gender == false).Count();
            GenderPie chart = new GenderPie();
            chart.Male = male;
            chart.Female = female;

            return Json(chart, JsonRequestBehavior.AllowGet);
        }

        //會員年齡分布
        public ActionResult AgeArea()
        {
            int fiftwe = db.Members.Where(m => (DateTime.Now.Year - m.Mem_Birthday.Year) > 15 && (DateTime.Now.Year - m.Mem_Birthday.Year) <= 25).Count();
            int twethi = db.Members.Where(m => (DateTime.Now.Year - m.Mem_Birthday.Year) > 25 && (DateTime.Now.Year - m.Mem_Birthday.Year) <= 35).Count();
            int thifou = db.Members.Where(m => (DateTime.Now.Year - m.Mem_Birthday.Year) > 35 && (DateTime.Now.Year - m.Mem_Birthday.Year) <= 45).Count();
            int foufif = db.Members.Where(m => (DateTime.Now.Year - m.Mem_Birthday.Year) > 45 && (DateTime.Now.Year - m.Mem_Birthday.Year) <= 55).Count();
            int fifsix = db.Members.Where(m => (DateTime.Now.Year - m.Mem_Birthday.Year) > 55 && (DateTime.Now.Year - m.Mem_Birthday.Year) <= 65).Count();
            int elder = db.Members.Where(m => (DateTime.Now.Year - m.Mem_Birthday.Year) > 65).Count();
            MemberAge chart = new MemberAge();
            chart.FifTwe = fiftwe;
            chart.TweThi = twethi;
            chart.ThiFou = thifou;
            chart.FouFif = foufif;
            chart.FifSix = fifsix;
            chart.Elder = elder;

            return Json(chart, JsonRequestBehavior.AllowGet);
        }

        //餐廳地區分布
        public ActionResult RestaurantColumn()
        {
            var query = db.Restaurants.GroupBy(r => r.City.Cit_Name)
                                      .Select(r => new { name = r.Key, count = r.Count() })
                                      .ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        //餐廳均價比
        public ActionResult PricePie()
        {
            int low = db.Restaurants.Where(r => r.Res_MinConsumption <= 200).Count();
            int medium = db.Restaurants.Where(r => r.Res_MinConsumption > 200 && r.Res_MinConsumption <= 500).Count();
            int expensive = db.Restaurants.Where(r => r.Res_MinConsumption > 500 && r.Res_MinConsumption <= 1000).Count();
            int veryexpensive = db.Restaurants.Where(r => r.Res_MinConsumption > 1000).Count();

            PricePie chart = new PricePie();
            chart.Low = low;
            chart.Medium = medium;
            chart.Expensive = expensive;
            chart.VeryExpensive = veryexpensive;
            return Json(chart, JsonRequestBehavior.AllowGet);
        }


        #region Sam Partialview
        public ActionResult _GiftView(int id = 1)
        {
            Gift gift = repositoryGift.GetByID(id);
            return PartialView(gift);
        }

        public ActionResult _GiftManagement()
        {
            return PartialView();
        }

        #endregion


        #endregion


        #region Wei
        public ActionResult AdministratorIndex()
        {
            if (Request.GetRole() != _LoginInfo._LoginRole.Administrator)
            {
                return RedirectToAction("MemberLogin", "Home");
            }
            ViewBag.NewQA = QARepository.GetByMemberID(null, null, Request).qA_SystemsNotice.Count();
            ViewBag.NewProsecutions = ReportRepository.GetSolvedProsecutions("false").Count();
            ViewBag.NewCase = ReportRepository.GetstringSolvedCase("false").Count();
            return View();
        }
        #endregion



        #region Han
        public ActionResult AdmQAShow(int? QA_MemberID, int? QA_RestaurantID)
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

                return View(QARepository.GetByMemberID(QA_MemberID, QA_RestaurantID, Request));
            }
            else if (Request.GetRole() == _LoginInfo._LoginRole.Member)
            {
                return RedirectToAction("QAShow", "Member", new { ResID = QA_RestaurantID });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            //location.href後面那個寫法要再研究，因為不能寫~/，感覺這個寫法放到IIS發布之後會出錯
        }
        [HttpPost]
        public ActionResult AdmAddToQa(string QA_Content, int? QA_MemberID, int? QA_RestaurantID)
        {

            QARepository.AddToQA(QA_Content, QA_MemberID, null, QA_RestaurantID, Request);
            return Content("");
        }
        public ActionResult AdmChatPartial(int? id, int? ResId)
        {
            if (ResId == null)
            {
                return PartialView(QARepository.GetByMemberID(id, null, Request));
            }
            else
            {
                return PartialView(QARepository.GetByMemberID(null, ResId, Request));
            }
        }
        public ActionResult AdmLeftChatPartial(string name)
        {
            var x = QARepository.GetByMemberID(2, null, Request);
            if (name == "All")
            {
                return PartialView(x);
            }
            else if (name == "Member")
            {
                x.QA_SystemsResNameData = null;
                return PartialView(x);
            }
            else
            {
                x.QA_SystemsNameData = null;
                return PartialView(x);
            }

        }


        //檢舉管理系統
        [HttpGet]
        public ActionResult ProsecutionOverview(string state)
        {
            if (string.IsNullOrEmpty(state))
                return View(ReportRepository.GetAllProsecutions());
            else
                return View(ReportRepository.GetSolvedProsecutions(state));
        }
        [HttpGet]
        public ActionResult PartialProsecutionOverview(string state)
        {
            if (string.IsNullOrEmpty(state))
                return PartialView(ReportRepository.GetAllProsecutions());
            else
                return PartialView(ReportRepository.GetSolvedProsecutions(state));
        }
        [HttpGet]
        public ActionResult ProsecutionSolved(int id, bool Solved)
        {
            Report report = ReportRepository.GetByID(id);
            if (Solved)
            {
                report.Rep_SolvedDate = DateTime.Now;
            }
            else
            {
                report.Rep_SolvedDate = null;
            }

            ReportRepository.Update(report);
            return RedirectToAction("ProsecutionOverview");
        }

        //申請檢舉
        public ActionResult AddToProsecution()
        {
            return View();
            //應該要跳出檢舉視窗，再研究怎麼弄
        }
        [HttpPost]
        public ActionResult AddToProsecution(Report report)
        {
            if (Request.Cookies["Role"] != null)
            {
                Manage.AddToProsecution(report, Request);
                return RedirectToAction("首頁的ActionName", "首頁的ControllerName");
            }
            return Content("<script>alert('請先登入再進行檢舉');location.href='/首頁的ContolarName/首頁的ActionName'</script>");
            //首頁還不知道是哪個
            //location.href後面那個寫法要再研究，因為不能寫~/，感覺這個寫法放到IIS發布之後會出錯
        }

        //檢舉管理
        public ActionResult ProsecutionManage()
        {
            return View();
        }
        #endregion


        #region Fu
        public ActionResult BusinessCase(string state)
        {
            if (Request.GetRole() == _LoginInfo._LoginRole.Administrator)
            {
                    return View(ReportRepository.GetByCase());
            }

            return RedirectToAction("MemberLogin", "Home");
        }

        public ActionResult PartialCase(string state)
        {
            if (string.IsNullOrEmpty(state))
                return PartialView(ReportRepository.GetByCase());
            else
                return PartialView(ReportRepository.GetstringSolvedCase(state));

        }
        #endregion
    

	}
}