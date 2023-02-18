using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models;
using Azure_0517.Models.ViewModel;

namespace Azure_0517.Models.Repository
{
    public partial class QAsystemRepository : GenericRepository<QA_system>
    {
        private Taste_ItEntities db = new Taste_ItEntities();
        public AdmQAViewModel GetByMemberID(int? MemberID, int? RestaurantID, HttpRequestBase Request)
        {
            AdmQAViewModel vm = new AdmQAViewModel();
            //--------//-----------------QALeftChat用的排序對話紀錄-------------------------
            List<QA_system> qA_SystemsNotice = new List<QA_system>();
            List<QA_system> qA_SystemsMemNotice = new List<QA_system>();
            List<QA_system> qA_SystemsResNotice = new List<QA_system>();

            var MemberGroup = dbContext.QA_system.Where(p => p.QA_AdministratorID == 3 && p.QA_MemberID != null)
           .GroupBy(p => p.QA_MemberID);
            var ResGroup = dbContext.QA_system.Where(p => p.QA_AdministratorID == 3 && p.QA_RestaurantID != null)
          .GroupBy(p => p.QA_RestaurantID);


            foreach (var item in MemberGroup)
            {
                var x = item.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
                qA_SystemsNotice.Add(x);
            }
            foreach (var item in ResGroup)
            {
                var x = item.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
                qA_SystemsNotice.Add(x);
            }

            vm.qA_SystemsNotice = qA_SystemsNotice.OrderByDescending(p => p.QA_CreateDate);
            //-------------------------排會員---------------------------------------
            //  var groupMember = dbContext.QA_system.Where(p => p.QA_AdministratorID == 3 && p.QA_MemberID != null)
            //.GroupBy(p => p.QA_MemberID);


            //  foreach (var item in groupMember)
            //  {
            //      var x = item.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
            //      qA_SystemsMemNotice.Add(x);
            //  }
            //  vm.qA_SystemsMemNotice = qA_SystemsMemNotice.OrderByDescending(p => p.QA_CreateDate);

            //-------------------------排會員---------------------------------------

            //-------------------------排餐廳---------------------------------------
            //  var groupRestaurant = dbContext.QA_system.Where(p => p.QA_AdministratorID == 3 && p.QA_RestaurantID != null)
            //.GroupBy(p => p.QA_RestaurantID);


            //  foreach (var item in groupRestaurant)
            //  {
            //      var x = item.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
            //      qA_SystemsResNotice.Add(x);
            //  }
            //  vm.qA_SystemsResNotice = qA_SystemsResNotice.OrderByDescending(p => p.QA_CreateDate);

            //-------------------------排餐廳---------------------------------------

            //-----//--------------------QALeftChat用的排序對話紀錄-------------------------

            //var queryMem = dbContext.QA_system.Where(p => p.QA_RestaurantID == null && p.QA_AdministratorID == 3)
            //    .GroupBy(p => p.QA_MemberID);
            //var queryRes = dbContext.QA_system.Where(p => p.QA_MemberID == null && p.QA_AdministratorID == 3)
            //    .GroupBy(p => p.QA_RestaurantID);

            //Dictionary<int?, string> MEMname = new Dictionary<int?, string>();
            //Dictionary<int?, string> RESname = new Dictionary<int?, string>();
            //foreach (var item in queryMem)
            //{
            //    var x = item.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
            //    MEMname.Add(x.QA_MemberID, x.Member.Mem_Name);
            //}
            //foreach (var item in queryRes)
            //{
            //    var x = item.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
            //    RESname.Add(x.QA_RestaurantID, x.Restaurant.Res_Name);
            //}
            //vm.QA_SystemsNameData = MEMname;
            //vm.QA_SystemsResNameData = RESname;
            //IEnumerable<QA_system> entities = dbContext.Set<QA_system>().ToList();
            //var admId = Convert.ToInt32(Request.Cookies["Key"].Value);
            var admId = 3;
            IEnumerable<QA_system> entities = null;
            IEnumerable<Member> member = null;
            IEnumerable<Restaurant> restaurants = null;
            if (MemberID != null)
            {
                entities = db.QA_system.Where(p => p.QA_MemberID == MemberID
                && p.QA_AdministratorID == admId)
               .OrderBy(p => p.QA_CreateDate);
                member = db.Members.Where(p => p.Mem_ID == MemberID);
                vm.Id = member.FirstOrDefault().Mem_ID;
                vm.Name = member.FirstOrDefault().Mem_Name;
                vm.RoleId = member.FirstOrDefault().Mem_RoleID;
            }
            else if (RestaurantID != null)
            {
                entities = db.QA_system.Where(p => p.QA_RestaurantID == RestaurantID
                 && p.QA_AdministratorID == admId)
                .OrderBy(p => p.QA_CreateDate);
                restaurants = db.Restaurants.Where(p => p.Res_ID == RestaurantID);
                vm.Id = restaurants.FirstOrDefault().Res_ID;
                vm.Name = restaurants.FirstOrDefault().Res_Name;
                vm.RoleId = restaurants.FirstOrDefault().Res_RoleID;
            }
            vm.QA_SystemsData = entities;
            return vm;
        }
        public AdmQAViewModel GetByIDForMember(int? AdmID, int? ResID, HttpRequestBase Request)
        {
            AdmQAViewModel vm = new AdmQAViewModel();
            List<QA_system> qA_SystemsMemNotice = new List<QA_system>();
            var MemId = Convert.ToInt32(Request.Cookies["Key"].Value);
            var group = dbContext.QA_system.Where(p => p.QA_MemberID == MemId && p.QA_AdministratorID == null)
        .GroupBy(p => p.QA_RestaurantID);
            foreach (var item in group)
            {
                var x = item.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
                qA_SystemsMemNotice.Add(x);
            }
            vm.qA_SystemsMemNotice = qA_SystemsMemNotice.OrderByDescending(p => p.QA_CreateDate);
            var queryRes = from qaList in db.QA_system
                           where qaList.QA_AdministratorID == null && qaList.QA_MemberID == MemId
                           orderby qaList.QA_CreateDate descending
                           group qaList by new { qaList.QA_RestaurantID, qaList.QA_MemberID } into pg
                           join
                           s in db.Restaurants on pg.Key.QA_RestaurantID equals s.Res_ID

                           select new { s.Res_Name, pg.Key.QA_RestaurantID };

            Dictionary<int?, string> ADMname = new Dictionary<int?, string>();
            Dictionary<int?, string> RESname = new Dictionary<int?, string>();
            ADMname.Add(3, "管理員");
            foreach (var item in queryRes)
            {
                RESname.Add(item.QA_RestaurantID, item.Res_Name);
            }
            vm.QA_SystemsNameData = ADMname;
            vm.QA_SystemsResNameData = RESname;
            //IEnumerable<QA_system> entities = dbContext.Set<QA_system>().ToList();

            IEnumerable<QA_system> entities = null;

            IEnumerable<Restaurant> restaurants = null;
            if (AdmID != null)
            {
                entities = db.QA_system.Where(p => p.QA_MemberID == MemId
                && p.QA_AdministratorID == AdmID)
               .OrderBy(p => p.QA_CreateDate);

                vm.Id = 3;
                vm.Name = "管理員";
                vm.RoleId = 3;
            }
            else if (ResID != null)
            {
                entities = db.QA_system.Where(p => p.QA_RestaurantID == ResID
                 && p.QA_MemberID == MemId)
                .OrderBy(p => p.QA_CreateDate);
                restaurants = db.Restaurants.Where(p => p.Res_ID == ResID);
                vm.Id = restaurants.FirstOrDefault().Res_ID;
                vm.Name = restaurants.FirstOrDefault().Res_Name;
                vm.RoleId = restaurants.FirstOrDefault().Res_RoleID;
            }
            vm.QA_SystemsData = entities;
            return vm;
        }
        public AdmQAViewModel GetByIDForRestaurant(int? MemID, int? AdmID, HttpRequestBase Request)
        {

            AdmQAViewModel vm = new AdmQAViewModel();
            List<QA_system> qA_SystemsResNotice = new List<QA_system>();
            var ResId = Convert.ToInt32(Request.Cookies["Key"].Value);
            var group = dbContext.QA_system.Where(p => p.QA_RestaurantID == ResId && p.QA_AdministratorID == null)
    .GroupBy(p => p.QA_MemberID);
            foreach (var item in group)
            {
                var x = item.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
                qA_SystemsResNotice.Add(x);
            }
            vm.qA_SystemsResNotice = qA_SystemsResNotice.OrderByDescending(p => p.QA_CreateDate);
            var queryMem = from qaList in db.QA_system
                           where qaList.QA_AdministratorID == null && qaList.QA_RestaurantID == ResId
                           orderby qaList.QA_CreateDate descending
                           group qaList by new { qaList.QA_RestaurantID, qaList.QA_MemberID } into pg
                           join
                           s in db.Members on pg.Key.QA_MemberID equals s.Mem_ID

                           select new { s.Mem_Name, pg.Key.QA_MemberID };
            Dictionary<int?, string> ADMname = new Dictionary<int?, string>();
            Dictionary<int?, string> MemName = new Dictionary<int?, string>();
            ADMname.Add(3, "管理員");
            foreach (var item in queryMem)
            {
                MemName.Add(item.QA_MemberID, item.Mem_Name);
            }
            vm.QA_SystemsNameData = ADMname;
            vm.QA_SystemsResNameData = MemName;
            //IEnumerable<QA_system> entities = dbContext.Set<QA_system>().ToList();

            IEnumerable<QA_system> entities = null;

            IEnumerable<Member> members = null;
            if (AdmID != null)
            {
                entities = db.QA_system.Where(p => p.QA_RestaurantID == ResId
                && p.QA_AdministratorID == AdmID)
               .OrderBy(p => p.QA_CreateDate);

                vm.Id = 3;
                vm.Name = "管理員";
                vm.RoleId = 3;
            }
            else if (MemID != null)
            {
                entities = db.QA_system.Where(p => p.QA_RestaurantID == ResId
                 && p.QA_MemberID == MemID)
                .OrderBy(p => p.QA_CreateDate);
                members = db.Members.Where(p => p.Mem_ID == MemID);
                vm.Id = members.FirstOrDefault().Mem_ID;
                vm.Name = members.FirstOrDefault().Mem_Name;
                vm.RoleId = members.FirstOrDefault().Mem_RoleID;
            }
            vm.QA_SystemsData = entities;
            return vm;
        }

        public void AddToQA(string QA_Content, int? QA_MemberID, int? QA_AdministratorID, int? QA_RestaurantID, HttpRequestBase Request)
        {
            QA_system qa = new QA_system();
            if (Request.Cookies["Role"].Value == "Member")
            {
                if (QA_AdministratorID != null)
                {
                    qa.QA_AdministratorID = QA_AdministratorID;
                }
                else if (QA_RestaurantID != null)
                {
                    qa.QA_RestaurantID = QA_RestaurantID;
                }
                qa.QA_MemberID = Convert.ToInt32(Request.Cookies["Key"].Value);
                qa.QA_MemkTalk = true;
                qa.QA_Content = QA_Content;

            }
            else if (Request.Cookies["Role"].Value == "Restaurant")
            {
                if (QA_AdministratorID != null)
                {
                    qa.QA_AdministratorID = QA_AdministratorID;
                }
                else if (QA_MemberID != null)
                {
                    qa.QA_MemberID = QA_MemberID;
                }
                qa.QA_RestaurantID = Convert.ToInt32(Request.Cookies["Key"].Value);
                qa.QA_ResTalk = true;
                qa.QA_Content = QA_Content;

            }
            else if (Request.Cookies["Role"].Value == "Administrator" && QA_MemberID != null)
            {
                qa.QA_AdministratorID = 3;
                qa.QA_AdmTalk = true;
                qa.QA_MemberID = QA_MemberID;
                qa.QA_Content = QA_Content;
            }
            else if (Request.Cookies["Role"].Value == "Administrator" && QA_RestaurantID != null)
            {
                qa.QA_AdministratorID = 3;
                qa.QA_AdmTalk = true;
                qa.QA_RestaurantID = QA_RestaurantID;
                qa.QA_Content = QA_Content;
            }
            //Response.Cookies["Role"].Value = "Mem";//Rest//Adm
            //Response.Cookies["Key"].Value = "id";

            qa.QA_CreateDate = DateTime.Now;
            if (qa.QA_Content == null)
            {
                qa.QA_Content = "";
            }

            db.QA_system.Add(qa);
            db.SaveChanges();
        }
        public IEnumerable<QA_system> GetQAGroupByID(int? MebID, int? ResID, int? AdmID)
        {
            List<QA_system> qA_Systems = new List<QA_system>();
            //重大突破，沒人教我絕對想不到可以這樣寫!!
            if (MebID != null)
            {
                var group = dbContext.QA_system.Where(p => p.QA_MemberID == MebID && p.QA_MemkTalk == null)
              .GroupBy(p => p.QA_RestaurantID);
                foreach (var item in group)
                {
                    var x = item.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
                    qA_Systems.Add(x);
                }
            }
            else if (ResID != null)
            {
                var group = dbContext.QA_system.Where(p => p.QA_RestaurantID == ResID && p.QA_ResTalk == null)
             .GroupBy(p => p.QA_MemberID);
                foreach (var item in group)
                {
                    var x = item.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
                    qA_Systems.Add(x);
                }
            }
            else if (AdmID != null)
            {
                var group = dbContext.QA_system.Where(p => p.QA_AdministratorID == AdmID && p.QA_AdmTalk == null)
            .GroupBy(p => p.QA_MemberID);
                //會員餐廳同時分組，這是關鍵一，取出一整個餐廳組，再細分餐廳組
                var ResGroup = group.FirstOrDefault().GroupBy(p => p.QA_RestaurantID);
                foreach (var itemResMeb in group)
                {
                    var x = itemResMeb.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
                    qA_Systems.Add(x);
                }
                //會員餐廳同時分組，這是關鍵二:刪掉第一組
                qA_Systems.RemoveAt(0);
                foreach (var itemRes in ResGroup)
                {
                    var x = itemRes.OrderByDescending(p => p.QA_CreateDate).FirstOrDefault();
                    qA_Systems.Add(x);
                }
            }
            //因為Adm只有一個才能這樣寫的樣子，有空再研究。研究完成5/4 12:19 10min

            return qA_Systems.OrderByDescending(p => p.QA_CreateDate);
        }
        public bool QaNoticeRead(int id)
        {
            this.dbContext.QA_system.Find(id).QA_AnswerdDate = DateTime.Now;
            dbContext.SaveChanges();
            return true;
        }
        public void DeleteNews(int id)
        {
            News news = new News();
            news = db.News.Find(id);

            db.News.Remove(news);
            var reports = db.Reports.Where(p => p.Rep_NewsID == id);
            if (reports != null)
            {
                foreach (var report in reports)
                {
                    db.Reports.Remove(report);
                }
            }

            db.SaveChanges();
        }
        public void DeleteComment(int id)
        {
            Comment comment = new Comment();
            comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
        }
    }
}