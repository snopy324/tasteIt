using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Azure_0517.Models;
using Azure_0517.Models.ViewModel;
using Azure_0517.Models.Repository;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Azure_0517.Models.Repository
{

    public class searchresult
    {
        public string Belong { get; set; }
        public int value { get; set; }
        public string content { get; set; }
        public string matchkey { get; set; }
    }
    public class spage
    {
        public int takeint { get; set; }

    }

    public partial class MemberRepository : GenericRepository<Member>
    {
        public Member adm_mem_searchbynumber(int keyword)
        {
            return dbContext.Members.Find(keyword);
        }

        //Rep_RestaurantID = rid, Rep_MemberID = mid, Rep_Status = 2, Rep_Content = "like", Rep_CreatedDate = DateTime.Now
        public string likeresornot(int mid, int rid)
        {
            string result = "";
            if (dbContext.Reports.Where(r => r.Rep_RestaurantID == rid && r.Rep_MemberID == mid && r.Rep_Content == "like").Count() > 0)
            {
                result = "like";
            }
            else
            {
                result = "notlike";
            }
            return result;
        }

        public IEnumerable<Member> Getexcludeadm()
        {
            IEnumerable<Member> mem = dbContext.Members.Where(m => m.Role.Role_ID < 3).ToList();
            return mem;
        }

        public byte[] getmemphotobuid(int id)
        {
            try
            {
                byte[] result = dbContext.Members.Find(id).Mem_Photo;
                return result;
            }
            catch
            {
                return null;
            }

        }

        public IEnumerable<Restaurant> mylikeres(int memberid)
        {
            IEnumerable<Restaurant> mylikeres = dbContext.Reports.Where(r => r.Rep_MemberID == memberid && r.Restaurant != null && r.Rep_Status.Value == 2).Select(res => res.Restaurant);
            return mylikeres;
        }
        public IEnumerable<News> mylikenews(int memberid)
        {
            IEnumerable<News> likenew = dbContext.News.Where(n => n.Member.Mem_ID == memberid);
            return likenew;
        }
    }

    public partial class TypeDetailRepository : GenericRepository<Type_Detail>
    {
        public new SelectList GetType()
        {
            IEnumerable<TheType> result = dbContext.TheTypes.Distinct().ToList();
            var x = new SelectList(result, "ResType_ID", "ResType_Style");
            return x;
        }

        public SelectList GetSubtype()
        {
            IEnumerable<SubType> result = dbContext.SubTypes.Distinct().ToList();
            var x = new SelectList(result, "SubT_ID", "SubT_Contect");
            return x;
        }
    }

    public partial class CityRepository : GenericRepository<City>
    {
        public SelectList Getcities()
        {
            IEnumerable<City> result = dbContext.Cities.ToList();
            var x = new SelectList(result, "Cit_ID", "Cit_Name");
            return x;
        }
    }

    public partial class RestaurantRepository : GenericRepository<Restaurant>
    {
        public int take_res_like_counts(int resid)
        {
            try
            {
                int result = dbContext.Reports.Where(r => r.Rep_RestaurantID == resid && r.Rep_Status == 2).Count();
                return result;
            }
            catch
            {
                return 0;
            }
        }
        public IEnumerable<Discount> r_t_d(int resid)
        {
            return dbContext.Discounts.Where(d => d.Dis_RestaurantID == resid).AsEnumerable();
        }
        public IEnumerable<ReservationOpened> r_t_r(int resid)
        {
            return dbContext.ReservationOpeneds.Where(r => r.RVOpen_RestaurantID == resid).AsEnumerable();
        }
        public IEnumerable<Picture> r_t_p(int resid)
        {
            //var target=dbContext.Pictures.Where(p => p.Pic_Restaurant == resid && p.Pic_NewsID == null && p.Pic_title != null && p.Pic_Content != null).AsEnumerable();
            //return target.Count() <= 6 ? target : target.Take(6);
            //return dbContext.Pictures.Where(p => p.Pic_Restaurant == resid && p.Pic_NewsID == null && p.Pic_title != null && p.Pic_Content != null).AsEnumerable();
            return dbContext.Pictures.Where(p => p.Pic_Restaurant == resid && p.Pic_NewsID == null).AsEnumerable();
        }
        public IEnumerable<News> r_t_n(int resid)
        {
            //var target=dbContext.News.Where(n => n.New_RestaurantID == resid).AsEnumerable();
            //return target.Count() <= 5 ? target : target.Take(5);
            return dbContext.News.Where(n => n.New_RestaurantID == resid && n.New_OrderDate == null).AsEnumerable();
        }
        public IEnumerable<Comment> r_t_c(int resid)
        {
            //var target= dbContext.Comments.Where(c => c.Com_RestaurantID == resid).AsEnumerable();
            //return target.Count() <= 5 ? target : target.Take(5);
            return dbContext.Comments.Where(c => c.Com_RestaurantID == resid).AsEnumerable();
        }


        //public IEnumerable<int> rhp_take_viewmodel(int resid,string status)
        //{
        //    switch (status)
        //    {
        //        case "dis":
        //            IEnumerable<int?> r1 = dbContext.Discounts.Where(d => d.Dis_RestaurantID == resid).Select(d => (int?)d.Dis_ID);
        //            return r1;
        //        case "res":
        //            IEnumerable<int?> r2 = dbContext.ReservationOpeneds.Where(r => r.RVOpen_RestaurantID == resid).Select(r =>(int?)r.RVOpen_ID);
        //            return r2;
        //        case "pho":
        //            IEnumerable<int?> r3 = dbContext.Pictures.Where(p => p.Pic_Restaurant == resid && p.Pic_NewsID == null&&p.Pic_title!=null&&p.Pic_Content!=null).Select(p => (int?)p.Pic_ID);
        //            return r3;
        //        case "news":
        //            IEnumerable<int?> r4 = dbContext.News.Where(n => n.New_RestaurantID == resid).Select(n => (int?)n.New_ID);
        //            return r4;
        //        case "com":
        //            IEnumerable<int?> r5 = dbContext.Comments.Where(c => c.Com_RestaurantID == resid).Select(c => (int?)c.Com_ID);
        //            return r5;
        //    }
        //    return null;
        //}

        public void c_save_res_subintroduction(Picture target, HttpPostedFileBase thephoto, int resid)
        {
            int length = thephoto.ContentLength;
            byte[] image = new byte[length];
            thephoto.InputStream.Read(image, 0, length);
            dbContext.Pictures.Add
                (
                new Picture
                {
                    Pic_Restaurant = resid,
                    Pic_title = target.Pic_title,
                    Pic_Content = target.Pic_Content,
                    Pic_Picture = image
                }
                );
            dbContext.SaveChanges();
        }

        public void resdataupdata(Restaurant target, Restaurant source, HttpPostedFileBase homepage = null)
        {
            target.Res_Name = source.Res_Name;
            target.Res_Address = source.Res_Address;
            target.Res_Phone = source.Res_Phone;
            target.Res_Email = source.Res_Email;
            target.Res_City = source.Res_City;
            target.Res_Account = source.Res_Account;
            target.Res_Password = source.Res_Password;
            target.Res_MaxConsumption = source.Res_MaxConsumption;
            target.Res_MinConsumption = source.Res_MinConsumption;
            target.Res_ParkingSeats = source.Res_ParkingSeats;
            target.Res_Seats = source.Res_Seats;
            target.Res_TaxID = source.Res_TaxID;
            target.Res_WorkingDate = source.Res_WorkingDate;
            target.Res_TenPercentService = source.Res_TenPercentService;
            target.Res_QAEnable = source.Res_QAEnable;
            target.Res_IntroductionContext = source.Res_IntroductionContext;
            if (homepage != null)
            {
                int length = homepage.ContentLength;
                byte[] image = new byte[length];
                homepage.InputStream.Read(image, 0, length);
                target.Res_HomePagePicture = image;
            }
            dbContext.SaveChanges();
        }


        public IEnumerable<City> takeallcity()
        {
            return dbContext.Cities;
        }

        public void addrestolikeorcancel(int rid, int mid, int flagstatus)
        {
            if (flagstatus == 1)//移除
            {
                var target = dbContext.Reports.Where(r => r.Rep_RestaurantID == rid && r.Rep_MemberID == mid && r.Rep_Status == 2 && r.Rep_Content == "like").First();
                dbContext.Reports.Remove(target);
            }
            else//增加
            {
                Report target = new Report { Rep_RestaurantID = rid, Rep_MemberID = mid, Rep_Status = 2, Rep_Content = "like", Rep_CreatedDate = DateTime.Now };
                dbContext.Reports.Add(target);
            }
            dbContext.SaveChanges();
        }

        public int? memcomornot(int resid, int? memberid)
        {
            int? comornot = null;
            if (memberid != null)
            {
                comornot = dbContext.Comments.Where(c => c.Com_RestaurantID == resid && c.Com_MemberID == memberid && c.Com_Content != null).Select(c => c.Com_ID).FirstOrDefault();
            }
            return memberid == null ? 0 : comornot == null ? 1 : 2;
        }

        public int? flagmemberlikeornot(int resid, int? memberid)
        {
            int? ml = null;
            if (memberid != null)
            {
                ml = dbContext.Reports.Where(r => r.Rep_RestaurantID == resid && r.Rep_MemberID == memberid).Select(r => r.Rep_Status).FirstOrDefault();
            }
            //return ml == null ? 0 : 1;
            return memberid == null ? 0 : ml == null ? 1 : 2;
        }

        public int flagrole(string cok)
        {
            int flag = 0;
            switch (cok)
            {
                case "Member":
                    flag = 1;
                    break;
                case "Restaurant":
                    flag = 2;
                    break;
                case "Administrator":
                    flag = 3;
                    break;
            }
            return flag;
        }

        public string takeres_stag(int resid)
        {
            string result = "";
            var tsource = dbContext.Type_Detail.Where(t => t.ResTypeDet_RestaurantID == resid && t.ResTypeDet_ResTypeID == null).ToList();

            if (tsource.Count() > 0)
            {
                List<string> ts = tsource.Select(t => t.SubType.SubT_Contect).ToList();
                for (int i = 0, max = ts.Count(); i < max; i++)
                {
                    string t2 = ts[i];
                    result += "," + t2;
                }
                return result;
            }
            else
            {
                return "無小標籤";
            }
        }

        public string takeres_tag(int resid)
        {
            string result = "";
            var tsource = dbContext.Type_Detail.Where(t => t.ResTypeDet_RestaurantID == resid && t.ResTypeDet_SubTypeID == null).ToList();

            if (tsource.Count() > 0)
            {
                List<string> ts = tsource.Select(t => t.TheType.ResType_Style).ToList();
                for (int i = 0, max = ts.Count(); i < max; i++)
                {
                    string t2 = ts[i];
                    result += "," + t2;
                }
                return result;
            }
            else
            {
                return "無大標籤";
            }
        }

        public IEnumerable<Restaurant> getreslist(IEnumerable<int> residlist)
        {
            var reslist = dbContext.Restaurants.Where(r => residlist.Contains(r.Res_ID));
            return reslist;
        }
        public IEnumerable<Restaurant> getreslist2(IEnumerable<int?> residlist)
        {
            if (residlist.Count() > 0)
            {
                var reslist = dbContext.Restaurants.Where(r => residlist.Contains(r.Res_ID));
                return reslist;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<int?> searchkeyword(string keyword)
        {
            List<int?> s_name = dbContext.Restaurants.Where(r => r.Res_Name.Contains(keyword)).Select(r => (int?)r.Res_ID).ToList();
            List<int?> s_type = dbContext.Type_Detail.Where(t => t.TheType.ResType_Style.Contains(keyword)).Select(r => r.ResTypeDet_RestaurantID).ToList();
            List<int?> s_stype = dbContext.Type_Detail.Where(s => s.SubType.SubT_Contect.Contains(keyword)).Select(r => r.ResTypeDet_RestaurantID).ToList();
            List<int?> s_city = dbContext.Restaurants.Where(c => c.City.Cit_Name.Contains(keyword)).Select(r => (int?)r.Res_ID).ToList();
            List<int?> s_con = dbContext.Restaurants.Where(i => i.Res_IntroductionContext.Contains(keyword)).Select(r => (int?)r.Res_ID).ToList();
            List<int?> result = new List<int?>();
            if (s_name.Count > 0) { result.AddRange(s_name); };
            if (s_type.Count > 0) { result.AddRange(s_type); };
            if (s_stype.Count > 0) { result.AddRange(s_stype); };
            if (s_city.Count > 0) { result.AddRange(s_city); };
            if (s_con.Count > 0) { result.AddRange(s_con); };
            result = result.Distinct().ToList();
            return result;
        }

        public IEnumerable<int> advanvedsearchresult(List<searchresult> source)
        {
            List<int?> rest = dbContext.Restaurants.Select(L => (int?)L.Res_ID).ToList();

            List<int?> typetarget = new List<int?>();
            List<int?> subtupetarget = new List<int?>();
            List<int?> citytarget = new List<int?>();
            int? s_max = dbContext.Restaurants.Max(m => m.Res_MaxConsumption);
            bool D_flag = true; bool R_flag = true; bool P_flag = true; bool T_flag = false; int minc = 0; int maxc = 0;
            //IQueryable<int?> typematch,subtypematch; IQueryable<int> citymatch, Allresid;
            foreach (searchresult item in source)
            {
                switch (item.Belong)
                {
                    case "istype": typetarget.Add(item.value); break;
                    case "issubtupe": subtupetarget.Add(item.value); break;
                    case "iscity": citytarget.Add(item.value); break;
                    case "dis": if (item.value == 1) { D_flag = true; } else { D_flag = false; } break;
                    case "reservation": if (item.value == 1) { R_flag = true; } else { R_flag = false; } break;
                    case "parkingseats": if (item.value == 1) { P_flag = true; } else { P_flag = false; } break;
                    case "tenpercent": if (item.value == 1) { T_flag = true; } else { T_flag = false; } break;
                    case "minprice": minc = item.value; break;
                    case "maxprice": maxc = item.value; break;
                }
            }

            if (typetarget.Count > 0)
            {
                typetarget = dbContext.Type_Detail.Where(t => typetarget.Contains(t.ResTypeDet_ResTypeID)).Select(rid => rid.ResTypeDet_RestaurantID).ToList();
            }
            else
            {
                typetarget = rest.ToList();
            }
            if (subtupetarget.Count > 0)
            {
                subtupetarget = dbContext.Type_Detail.Where(s => subtupetarget.Contains(s.ResTypeDet_SubTypeID)).Select(rid => rid.ResTypeDet_RestaurantID).ToList();
            }
            else
            {
                subtupetarget = rest.ToList();
            }
            if (citytarget.Count > 0)
            {
                citytarget = dbContext.Restaurants.Where(c => citytarget.Contains(c.Res_City.Value)).Select(rid => (int?)rid.Res_ID).ToList();
            }
            else
            {
                citytarget = rest.ToList();
            }

            var q = typetarget.Intersect<int?>(subtupetarget).Intersect<int?>(citytarget);

            if (P_flag)
            {

            }
            if (T_flag)
            {

            }
            if (minc != 0 | maxc != s_max)
            {
                List<int?> sp = dbContext.Restaurants.Where(r => r.Res_MinConsumption >= minc && r.Res_MaxConsumption <= maxc).Select(r => (int?)r.Res_ID).ToList();
                q = q.Intersect<int?>(sp);
            }
            return q.Select(L => L.Value).ToList();
        }

        public IEnumerable<TheType> F_astype()
        {
            return dbContext.TheTypes.AsQueryable();
        }
        public IEnumerable<SubType> F_assubtupe()
        {
            return dbContext.SubTypes.AsQueryable();
        }
        public IEnumerable<City> F_ascity()
        {
            return dbContext.Cities.AsQueryable();
        }
        public IEnumerable<Discount> F_asdiscount()
        {
            return dbContext.Discounts.AsQueryable();
        }
        public IEnumerable<ReservationOpened> F_asro()
        {
            return dbContext.ReservationOpeneds.AsQueryable();
        }
        public int F_maxcost()
        {
            return dbContext.Restaurants.Max(c => c.Res_MaxConsumption).Value;
        }


        public byte[] getimagebyid(int id)
        {
            byte[] image = dbContext.Restaurants.Find(id).Res_HomePagePicture;
            return image;
        }

        public IEnumerable<Picture> getselfpicture(int id)
        {
            IEnumerable<Picture> selfpicture = dbContext.Pictures.Where(p => p.Restaurant.Res_ID == id && p.Pic_NewsID == null).ToList();
            return selfpicture;
        }

        public IEnumerable<Picture> getnewspicture(int id)
        {
            IEnumerable<Picture> newspicture = dbContext.Pictures.Where(p => p.Restaurant.Res_ID == id && p.Pic_NewsID != null).ToList();
            return newspicture;
        }

        public int getstarbyresid(int id)
        {
            try
            {
                int stars = (dbContext.Comments.Where(c => c.Com_RestaurantID == id).Sum(s => s.Com_StarRate)) / (dbContext.Comments.Where(c => c.Com_RestaurantID == id).Count());
                return stars;
            }
            catch
            {
                return 0;
            }
        }

        public IEnumerable<string> res_type(int resid)
        {
            var target = dbContext.Type_Detail.Where(s => s.ResTypeDet_RestaurantID == resid && s.ResTypeDet_ResTypeID.HasValue).Select(t => t.TheType.ResType_Style);
            if (target.Count() > 0)
            {
                return target;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> res_subtype(int resid)
        {
            var target = dbContext.Type_Detail.Where(s => s.ResTypeDet_RestaurantID == resid && s.ResTypeDet_SubTypeID.HasValue).Select(s => s.SubType.SubT_Contect);
            if (target.Count() > 0)
            {
                return target;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> res_nontype(int resid)
        {
            if (res_type(resid) == null)
            {
                return dbContext.TheTypes.Select(t => t.ResType_Style);
            }
            else
            {
                var g = res_type(resid);
                var target = dbContext.TheTypes.Where(t => !g.Contains(t.ResType_Style)).Select(t => t.ResType_Style);
                return target;
            }
        }

        public IEnumerable<string> res_nonsubtype(int resid)
        {
            if (res_subtype(resid) == null)
            {
                return dbContext.SubTypes.Select(s => s.SubT_Contect);
            }
            else
            {
                var g = res_subtype(resid);
                var target = dbContext.SubTypes.Where(s => !g.Contains(s.SubT_Contect)).Select(s => s.SubT_Contect);
                return target;
            }
        }

        public void savetype(int resid, string belong, string content)
        {
            Type_Detail target;
            int k;
            if (belong == "t_type")
            {
                k = Convert.ToInt32(dbContext.TheTypes.Where(t => t.ResType_Style == content).Select(t => t.ResType_ID).First());
                target = new Type_Detail { ResTypeDet_RestaurantID = resid, ResTypeDet_NewsID = null, ResTypeDet_ResTypeID = k, ResTypeDet_SubTypeID = null };
            }
            else
            {
                k = Convert.ToInt32(dbContext.SubTypes.Where(s => s.SubT_Contect == content).Select(t => t.SubT_ID).First());
                target = new Type_Detail { ResTypeDet_RestaurantID = resid, ResTypeDet_NewsID = null, ResTypeDet_ResTypeID = null, ResTypeDet_SubTypeID = k };
            }
            dbContext.Set<Type_Detail>().Add(target);
            dbContext.SaveChanges();
        }

        public void clear_type(int resid)
        {
            var target = dbContext.Type_Detail.Where(t => t.ResTypeDet_RestaurantID == resid).ToList();
            if (target != null)
            {
                dbContext.Set<Type_Detail>().RemoveRange(target);
                dbContext.SaveChanges();
            }

        }

        //public IEnumerable<Restaurant> SearchByType(int typeid)//???????????給搜尋用
        //{
        //    var result = dbContext.Restaurants.Where(r => r.Type_Detail.Select(t => t.TheType.ResType_ID == typeid).First()).ToList();
        //    return result;
        //}

        //public IEnumerable<Restaurant> SearchBySubtype(int subtypeid)//???????????給搜尋用
        //{
        //    var result = dbContext.Restaurants.Where(r => r.Type_Detail.Select(s => s.SubType.SubT_ID == subtypeid).First()).ToList();
        //    return result;
        //}
    }

    public partial class PictureRepository : GenericRepository<Picture>
    {
        public IEnumerable<Picture> pidarray(IEnumerable<int> source)
        {
            List<Picture> target = new List<Picture>();
            foreach (int pid in source)
            {
                target.Add(dbContext.Pictures.Find(pid));
            }
            return target.AsEnumerable();
        }

        public void resself_chagephoto(Picture source, HttpPostedFileBase photo)
        {
            Picture target = dbContext.Pictures.Find(source.Pic_ID);
            target.Pic_title = source.Pic_title;
            target.Pic_Content = source.Pic_Content;
            if (photo != null)
            {
                int length = photo.ContentLength;
                byte[] image = new byte[length];
                photo.InputStream.Read(image, 0, length);
                target.Pic_Picture = image;
            }
            dbContext.SaveChanges();
        }

        public void deletephotobyid(int pid)
        {
            var target = dbContext.Pictures.Find(pid);
            dbContext.Pictures.Remove(target);
            dbContext.SaveChanges();
        }

        public IEnumerable<Picture> getres_allphoto(int resid)
        {
            //var result = dbContext.Pictures.Where(p => p.Pic_Restaurant == resid && p.Pic_NewsID == null && p.Pic_title != null);
            //return result;
            var result = dbContext.Pictures.Where(p => p.Pic_Restaurant == resid && p.Pic_NewsID == null);
            return result;
        }

        public byte[] getimagebyid(int id)
        {
            byte[] image = dbContext.Pictures.Find(id).Pic_Picture;
            return image;
        }
        public void updatethepicture(string[] x, byte[] y, int id)
        {
            var target = dbContext.Pictures.Find(id);
        }
    }

}