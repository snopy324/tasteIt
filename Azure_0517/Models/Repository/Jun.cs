using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using Azure_0517.Models;
using Azure_0517.Models.ViewModel;

namespace Azure_0517.Models.Repository
{
    public partial class CityRepository : GenericRepository<City>
    {
        public IQueryable<City> GetAllForQuery()
        {
            return dbContext.Cities;
        }
        public int? GetCityIDbyString(string name)
        {
            List<City> cities = this.GetAll().ToList();
            foreach (var city in cities)
            {
                if (name.Contains(city.Cit_Name.Substring(0, 2)))
                {
                    return city.Cit_ID;
                }
            }
            return (int?)null;
        }
    }
    
    public partial class ReservationBookedRepository : GenericRepository<ReservationBooked>
    {
        ReservationOpenedRepository OpenedRepository = new ReservationOpenedRepository();

        public bool CheckIn(int id)
        {
            ReservationBooked booked = this.GetByID(id);
            if(!booked.RVBooked_CheckinTime.HasValue)
            {
                booked.RVBooked_CheckinTime = DateTime.Now;
                if(booked.ReservationOpened.RVOpen_OffPrice.First() == '+')
                {
                    char [] str = {'+','點' };
                    booked.Member.Mem_Point += Convert.ToInt32(booked.ReservationOpened.RVOpen_OffPrice.Split(str)[1]);
                }
            }
            else
            {
                booked.RVBooked_CheckinTime = null;
                if (booked.ReservationOpened.RVOpen_OffPrice.First() == '+')
                {
                    char[] str = { '+', '點' };
                    booked.Member.Mem_Point -= Convert.ToInt32(booked.ReservationOpened.RVOpen_OffPrice.Split(str)[1]);
                }
            }

            dbContext.SaveChanges();
            return true;
        }

        public override bool Insert(ReservationBooked entity)
        {

            return OpenedRepository.BookedRepositoryChange(entity, ReservationOpenedRepository.BookedStatus.Insert).HasValue ? base.Insert(entity) : false;
            //return TryCatch<ReservationBooked>.RepositoryTryCatch(
            //    E =>
            //    {
            //        return OpenedRepository.BookedRepositoryUpdate(E,true) ? base.Insert(E) : false;                        
            //    }
            //    , entity);
        }

        public override bool Delete(ReservationBooked entity)
        {
            return OpenedRepository.BookedRepositoryChange(entity, ReservationOpenedRepository.BookedStatus.Delete).HasValue ? 
                base.Delete(entity) : false;
            //return TryCatch<ReservationBooked>.RepositoryTryCatch(
            //    E =>
            //    {
            //        return OpenedRepository.BookedRepositoryChange(entity, ReservationOpenedRepository.BookedStatus.Delete) ? base.Delete(E) : false;
            //    }
            //    , entity);
        }

        public override bool Update(ReservationBooked entity)
        {
            OpenedRepository.BookedRepositoryChange(entity, ReservationOpenedRepository.BookedStatus.Update);
            return true;
        }

        public int? UpdateGetNewID(ReservationBooked entity)
        {
            return OpenedRepository.BookedRepositoryChange(entity, ReservationOpenedRepository.BookedStatus.Update);
            
        }


        public IEnumerable<ReservationBooked> GetByMemberID(int id)
        {
            return dbContext.ReservationBookeds.Where(L => L.RVBooked_MemberID == id).OrderByDescending(L => L.RVBooked_ID).ToList();
        }

        public IEnumerable<ReservationBooked> GetByMemberID(int id, int dateTime)
        {
            switch (dateTime)
            {
                case 0:
                    return dbContext.ReservationBookeds.Where(L => L.RVBooked_MemberID == id).OrderByDescending(L => L.ReservationOpened.RVOpen_DateTime).ToList();
                case 1:
                    return dbContext.ReservationBookeds.Where(L => L.RVBooked_MemberID == id && L.ReservationOpened.RVOpen_DateTime < DateTime.Now).OrderByDescending(L => L.ReservationOpened.RVOpen_DateTime).ToList();
                case 2:
                    return dbContext.ReservationBookeds.Where(L => L.RVBooked_MemberID == id && L.ReservationOpened.RVOpen_DateTime >= DateTime.Now).OrderByDescending(L => L.ReservationOpened.RVOpen_DateTime).ToList();
                default:
                    return dbContext.ReservationBookeds.Where(L => L.RVBooked_MemberID == id).OrderByDescending(L => L.ReservationOpened.RVOpen_DateTime).ToList();
            }
        }

        public ReservationBooked RepeatInOpen(ReservationOpened opened, int Mem_ID)
        {
            return dbContext.ReservationBookeds
                .Where(L => L.RVBooked_MemberID == Mem_ID)
                .Where(L => L.RVBooked_ReservationOpenedID == opened.RVOpen_ID).FirstOrDefault();
        }

        public IEnumerable<ReservationBooked> RepeatInDay(ReservationOpened opened, int Mem_ID)
        {
            return dbContext.ReservationBookeds.Where(L => L.RVBooked_MemberID == Mem_ID).Where(L => L.ReservationOpened.RVOpen_TeamHeader == opened.RVOpen_TeamHeader).ToList();
        }

    }

    public partial class ReservationOpenedRepository : GenericRepository<ReservationOpened>
    {

        public enum BookedStatus
        {
            Update,
            Insert,
            Delete
        }

        
        public bool ChangeSeats(ReservationOpened entity, int Seats)
        {
            if (Seats >= entity.RVOpen_Seats - entity.RVOpen_SeatsRemain)
            {
                entity.RVOpen_SeatsRemain = Seats - entity.RVOpen_Seats + entity.RVOpen_SeatsRemain;
                entity.RVOpen_Seats = Seats;
                return Update(entity);
            }
            else
                return false;
        }

        public int? BookedRepositoryChange(ReservationBooked NewBooked, BookedStatus bookedStatus)
        {
            ReservationOpened opened = dbContext.ReservationOpeneds.Find(NewBooked.RVBooked_ReservationOpenedID);
            ReservationBookedRepository reservationBooked = new ReservationBookedRepository();
            switch (bookedStatus)
            {
                case BookedStatus.Insert:
                    if (opened.RVOpen_SeatsRemain >= NewBooked.RVBooked_Seats)
                    {
                        opened.RVOpen_SeatsRemain -= NewBooked.RVBooked_Seats;
                        dbContext.SaveChanges();
                        return NewBooked.RVBooked_ID;
                    }
                    else
                    {
                        NewBooked.RVBooked_ReminderContext = $"BackUpSeats:{GetLatestBackUpNumber(opened.RVOpen_ID)};" + NewBooked.RVBooked_ReminderContext;
                        return NewBooked.RVBooked_ID;
                    }

                case BookedStatus.Delete:
                    if (NewBooked.BackUpNo is null)
                    {
                        opened.RVOpen_SeatsRemain += NewBooked.RVBooked_Seats;
                        Report notice = dbContext.Reports.Where(L => L.Rep_MemberID == NewBooked.RVBooked_MemberID && L.Rep_AccuseTypeID == 9).FirstOrDefault();
                        if(notice != null)
                        {
                            dbContext.Reports.Remove(notice);
                        }
                    }
                    SerialBackUpNumber(NewBooked);
                    dbContext.SaveChanges();
                    return NewBooked.RVBooked_ID;

                case BookedStatus.Update:
                    
                    ReservationBooked oldbooked = reservationBooked.GetByID(NewBooked.RVBooked_ID);

                    ReservationBooked insertbook = new ReservationBooked
                    {
                        RVBooked_MemberID = oldbooked.RVBooked_MemberID,
                        ReminderText = NewBooked.ReminderText,
                        RVBooked_BookedDate = oldbooked.RVBooked_BookedDate,
                        RVBooked_ReservationOpenedID = oldbooked.RVBooked_ReservationOpenedID,
                        RVBooked_Seats = NewBooked.RVBooked_Seats
                    };
                    reservationBooked.Delete(oldbooked);
                    reservationBooked.Insert(insertbook);
                    return insertbook.RVBooked_ID;

            default:
                    return null ;
            }



        }

        public int GetLatestBackUpNumber(int id) {
            var q = dbContext.ReservationOpeneds.Find(id).ReservationBookeds.OrderByDescending(m => m.RVBooked_BookedDate).AsQueryable();
            foreach(var book in q)
            {
                if(book.RVBooked_ReminderContext != null && book.RVBooked_ReminderContext.Split(';')[0].Contains("BackUpSeats:"))
                {
                    return Convert.ToInt32(book.RVBooked_ReminderContext.Split(';')[0].Split(':')[1]) + 1;
                }
            }
            
            return 1;
        }

        public void SerialBackUpNumber(ReservationBooked booked)
        {
            var q = dbContext.ReservationOpeneds.Find(booked.RVBooked_ReservationOpenedID.Value).ReservationBookeds.OrderBy(m => m.RVBooked_BookedDate).AsQueryable();

            int BackupNumber = 1;
            foreach(var book in q)
            {
                if(book.RVBooked_ID == booked.RVBooked_ID)
                {
                    continue;
                }

                if (book.BackUpNo != null && book.RVBooked_Seats <= book.ReservationOpened.RVOpen_SeatsRemain)
                {
                    
                    book.BackUpNo = null;
                    book.ReservationOpened.RVOpen_SeatsRemain -= book.RVBooked_Seats;
                    ///


                    Report report = new Report
                    {
                        Rep_Status = 5,////通知
                        Rep_AccuseTypeID = 9,////補位通知
                        Rep_MemberID = book.RVBooked_MemberID,
                        Rep_CreatedDate = DateTime.Now,
                        Rep_Content = $"訂位編號 {book.RVBooked_ID} ：您於 {book.ReservationOpened.Restaurant.Res_Name}" +
                        $"的訂位候補到了!! 祝您用餐愉快。",
                    };
                    this.dbContext.Reports.Add(report);


                    ///
                }
                else if (book.BackUpNo != null) 
                {
                    book.BackUpNo = BackupNumber;
                    BackupNumber++;
                }
            }
            dbContext.SaveChanges();

        }
        
        public int GetNewTeamHeader()
        {
            return TryCatch<ReservationOpened>.RepositoryTryCatch(() =>
            {
                return dbContext.ReservationOpeneds
                        .OrderByDescending(L => L.RVOpen_TeamHeader)
                        .FirstOrDefault()
                        .RVOpen_TeamHeader + 1;
            });
        }

        public IGrouping<int, ReservationOpened> GetByTeamHeaderID(int id)
        {

            return dbContext.ReservationOpeneds.Where(L => L.RVOpen_TeamHeader == id).GroupBy(L => L.RVOpen_TeamHeader).FirstOrDefault();

        }
        //ForNow and Later
        public IEnumerable<IGrouping<int, ReservationOpened>> GetTeamHeadersByRestaurantID(int id, DateTime? date=null)
        {
            if (date.HasValue)
            {
                return dbContext.ReservationOpeneds
                    .Where(L => L.RVOpen_RestaurantID == id)
                    .Where(L=>DbFunctions.TruncateTime(L.RVOpen_DateTime) == DbFunctions.TruncateTime(date))
                    .Where(L=>L.RVOpen_DateTime > DateTime.Now)
                    .OrderByDescending(L => L.RVOpen_DateTime).GroupBy(L => L.RVOpen_TeamHeader);

            }
            else
            {
                return dbContext.ReservationOpeneds
                .Where(L => L.RVOpen_RestaurantID == id)
                .GroupBy(L => L.RVOpen_TeamHeader)
                .Where(L => L.FirstOrDefault().RVOpen_DateTime > DbFunctions.TruncateTime(DateTime.Now))
                .OrderBy(L=>L.FirstOrDefault().RVOpen_DateTime);
            }

        }
        //ForPass
        public IEnumerable<IGrouping<int, ReservationOpened>> GetTeamHeadersByRestaurantID(int id, DateTime? date = null, DateTime? enddate = null)
        {
            
            return dbContext.ReservationOpeneds
            .GroupBy(L => L.RVOpen_TeamHeader)
            .Where(L => L.FirstOrDefault().RVOpen_RestaurantID == id )
            .Where(L=>DbFunctions.TruncateTime(L.FirstOrDefault().RVOpen_DateTime) < DbFunctions.TruncateTime(DateTime.Now))
            .OrderByDescending(L => L.FirstOrDefault().RVOpen_DateTime);
        }


        public IEnumerable<IGrouping<int, ReservationOpened>> GetAllTeamHeaders()
        {

            return dbContext.ReservationOpeneds.GroupBy(L => L.RVOpen_TeamHeader);

        }

        public IEnumerable<IGrouping<int, ReservationOpened>> GetAllTeamHeaders(DateTime startTime)
        {

            return dbContext.ReservationOpeneds.Where(L=>L.RVOpen_DateTime>=startTime).OrderByDescending(L=>L.RVOpen_DateTime).GroupBy(L => L.RVOpen_TeamHeader).ToList();

        }

        public IEnumerable<IGrouping<int, ReservationOpened>> GetTeamHeadersBySearch(HttpRequestBase httpRequest)
        {
            var q = this.GetAllTeamHeaders(DateTime.Now).AsQueryable();
            DateTime date = new DateTime();
            

            if (DateTime.TryParse(httpRequest.QueryString["Date"], out date))
            {
                q = q.AsEnumerable().Where(L => L.First().RVOpen_DateTime.Date == date).AsQueryable();
            }



            //ViewBag.Manageable = false;
            if (httpRequest.QueryString.AllKeys.Where(str => str.Contains("C")).Count() != 0)
            {
                foreach (var city in dbContext.Cities.ToList())
                {
                    if (httpRequest.QueryString[$"C{city.Cit_ID}"] != null)
                    {
                        continue;
                    }
                    q = q.Where(L => L.FirstOrDefault().Restaurant.Res_City != city.Cit_ID).AsQueryable();

                }
            }

            RestaurantRepository restaurantRepository = new RestaurantRepository();
            var t = q.ToList();
            List<IGrouping<int, ReservationOpened>> t2 = new List<IGrouping<int, ReservationOpened>>();
            if (httpRequest.QueryString.AllKeys.Where(str => str.Contains("T")).Count() != 0)
            {
                foreach (var theType in dbContext.TheTypes.ToList())
                {
                    if (httpRequest.QueryString[$"T{theType.ResType_ID}"] != null)
                    {
                        foreach (var x in t.Where(L => restaurantRepository.HastheType(L.FirstOrDefault().RVOpen_RestaurantID, theType.ResType_ID)).ToList())
                        {
                            t2.Add(x);
                        }
                    }
                }
                q = t2.Distinct().AsQueryable();
            }

            
            var s = q.ToList();
            List<IGrouping<int, ReservationOpened>> s2 = new List<IGrouping<int, ReservationOpened>>();
            if (httpRequest.QueryString.AllKeys.Where(str => str.Contains("S")).Count() != 0)
            {
                
                foreach (var subType in dbContext.SubTypes.ToList())
                {
                    if (httpRequest.QueryString[$"S{subType.SubT_ID}"] != null)
                    {
                        foreach (var x in s.Where(L => restaurantRepository.HassubType(L.FirstOrDefault().RVOpen_RestaurantID, subType.SubT_ID)).ToList())
                        {
                            s2.Add(x);
                        }
                    }
                }
                q = s2.Distinct().AsQueryable();
            }
            restaurantRepository.Dispose();

            var p = q.ToList();
            List<IGrouping<int, ReservationOpened>> p2 = new List<IGrouping<int, ReservationOpened>>();
            int? price = Convert.ToInt32(httpRequest.QueryString["Price"]);
            switch (price)
            {
                case 1:
                    price = 0;
                    break;
                case 2:
                    price = 100;
                    break;
                case 3:
                    price = 200;
                    break;
                case 4:
                    price = 500;
                    break;
                case 5:
                    price = 1000;
                    break;
                case 6:
                    price = 1500;
                    break;
                case 7:
                default:
                    price = null;
                    break;
            }
            if (price == 0)
            {
                return q.ToList();
            }
            if (price == null)
            {
                foreach (var team in p)
                {
                    if (team.First().Restaurant.Res_MinConsumption >= 1500)
                    {
                        p2.Add(team);
                    }
                }
                q = p2.AsQueryable();
                return q.ToList();
            }

            foreach (var team in p)
            {
                if (team.First().Restaurant.Res_MinConsumption <= price.Value && team.First().Restaurant.Res_MaxConsumption >= price.Value)
                {
                    p2.Add(team);
                }
            }
            q = p2.AsQueryable();
            return q.ToList();

        }

        public bool BlockUpdate(ReservationOpened reservation, ReservationNewModel newModel)
        {
            var team = this.GetByTeamHeaderID(reservation.RVOpen_TeamHeader);
            foreach (var opened in team)
            {
                int seatBooked = opened.RVOpen_Seats - opened.RVOpen_SeatsRemain;
                if (reservation.RVOpen_Seats >= seatBooked)
                {
                    opened.RVOpen_Seats = reservation.RVOpen_Seats;
                    opened.RVOpen_SeatsRemain = opened.RVOpen_Seats - seatBooked;
                }
                else
                {
                    opened.RVOpen_Seats = seatBooked;
                    opened.RVOpen_SeatsRemain = 0;
                }
                this.Update(opened);
            }
            return true;
        }

        public bool InsertNewBlock(ReservationOpened reservation, ReservationNewModel newModel)
        {
            reservation.RVOpen_SeatsRemain = reservation.RVOpen_Seats;
            reservation.RVOpen_TeamHeader = this.GetNewTeamHeader();
            DateTime dateTime = newModel.OpenDate.AddHours(newModel.StartTime.Hour).AddMinutes(newModel.StartTime.Minute);
            DateTime endTime = newModel.OpenDate.AddHours(newModel.EndTime.Hour).AddMinutes(newModel.EndTime.Minute);
            reservation.RVOpen_DateTime = dateTime;
            while (DateTime.Compare(endTime, reservation.RVOpen_DateTime) >= 0)
            {
                this.Insert(reservation);
                reservation.RVOpen_DateTime = reservation.RVOpen_DateTime.AddMinutes(newModel.StepTime);
            }
            return true;
        }

        public bool DeleteTeamHeader(int key)
        {
            
            IGrouping<int,ReservationOpened> teams = this.GetByTeamHeaderID(key);
            foreach(ReservationOpened open in teams)
            {
                foreach(ReservationBooked booked in open.ReservationBookeds)
                {
                    Report report = new Report
                    {
                        Rep_Status = 5,////通知
                        Rep_AccuseTypeID = 10,////通知
                        Rep_MemberID = booked.RVBooked_MemberID,
                        Rep_RestaurantID = booked.ReservationOpened.RVOpen_RestaurantID,
                        Rep_CreatedDate = DateTime.Now,
                        Rep_Content = $"訂位取消 {booked.RVBooked_ID} ：您於 {booked.ReservationOpened.Restaurant.Res_Name}" +
                        $"的訂位已被餐廳取消!! 如有任何疑問請聯絡該餐廳。",
                    };
                    this.dbContext.Reports.Add(report);
                    dbContext.SaveChanges();
                }
                dbContext.ReservationBookeds.RemoveRange(open.ReservationBookeds);
                
                dbContext.SaveChanges();
                this.Delete(open);
            }

            return true;
        }

    }

    public partial class ReportRepository : GenericRepository<Report>
    {
        public IEnumerable<Report> GetAllProsecutions()
        {
            return dbContext.Reports.Where(s => s.Rep_Status == 1);
        }
            
        internal IEnumerable<Report> GetSolvedProsecutions(string Solved)
        {
            if (Solved == "true")
                return dbContext.Reports.Where(s => s.Rep_Status == 1).Where(s => s.Rep_SolvedDate.HasValue);
            else
                return dbContext.Reports.Where(s => s.Rep_Status == 1).Where(s => !s.Rep_SolvedDate.HasValue);
        }

        public IEnumerable<Report> GetNoticeByID(HttpRequestBase request)
        {

            IEnumerable<Report> reports = new List<Report>();
            _LoginInfo._LoginRole role = request.GetRole();
            if(role == _LoginInfo._LoginRole.Guest)
            {
                return reports;
            }
            int id = request.GetRoleKey().Value;
            switch (role)
            {
                case _LoginInfo._LoginRole.Member:
                    reports =  dbContext.Reports.Where(L => L.Rep_MemberID == id && L.Rep_Status.Value == 5).OrderByDescending(L => L.Rep_CreatedDate)
                    .AsQueryable().Take(5).ToList();
                    break;
                case _LoginInfo._LoginRole.Administrator:
                    reports = dbContext.Reports.Where(L=>L.Rep_Status.Value == 1008).OrderByDescending(L => L.Rep_CreatedDate)
                    .AsQueryable().Take(5);
                    break;
                case _LoginInfo._LoginRole.Restaurant:
                case _LoginInfo._LoginRole.Guest:
                default:
                    return reports;
            }
            return reports;
        }

        public bool NoticeRead(int id)
        {
            this.dbContext.Reports.Find(id).Rep_SolvedDate = DateTime.Now;
            dbContext.SaveChanges();
            return true;
        }
 
        public bool AddBlogToLike(int id, HttpRequestBase Request)
        {
            Report report = new Report {
                Rep_MemberID = Request.GetRoleKey().Value,
                Rep_Status = 2,
                Rep_NewsID = id,
                Rep_Content = "like",
                Rep_CreatedDate = DateTime.Now
                 
            };
            return this.Insert(report);

        }


    }

    public partial class DiscountRepository : GenericRepository<Discount>
    {
        public IEnumerable<Discount> GetbyRestID(int id)
        {
            return dbContext.Discounts.Where(L => L.Dis_RestaurantID == id).OrderByDescending(L=>L.Dis_Start).ToList();
        }

        public IQueryable<Discount> GetAll(DateTime startTime)
        {
            return dbContext.Discounts
                .Where(L => (!L.Dis_End.HasValue || L.Dis_End.Value >= startTime.Date) && L.Dis_Start <= startTime.Date)
                .OrderByDescending(L => L.Dis_Start);
        }
        public IQueryable<Discount> GetToStart()
        {
            return dbContext.Discounts
                .Where(L => L.Dis_Start>=DateTime.Now);
        }
        public byte[] GetImg(int id)
        {
            return dbContext.Discounts.Find(id).Dis_Picture;
        }
    }

    public partial class RestaurantRepository : GenericRepository<Restaurant>
    {
        public  bool HastheType(int restaurantid, int theTypeid)
        {
            if (this.dbContext.Type_Detail.Where(L => L.ResTypeDet_RestaurantID == restaurantid && L.ResTypeDet_SubTypeID == null && L.ResTypeDet_ResTypeID == theTypeid).ToList().Count > 0)
                return true;
            else
                return false;
        }
        public  bool HassubType(int restaurantid, int subTypeid)
        {
            if (this.dbContext.Type_Detail.Where(L => L.ResTypeDet_RestaurantID == restaurantid && L.ResTypeDet_SubTypeID == subTypeid).ToList().Count > 0)
                return true;
            else
                return false;
        }
        public IEnumerable<Restaurant> GetByCityID(int id)
        {
            return this.dbContext.Restaurants.Where(L => L.Res_City == id).AsEnumerable();
        }
    }

    public partial class TheTypeRepository : GenericRepository<TheType>
    {
        public IQueryable<TheType> GetAllForQuery()
        {
            return dbContext.TheTypes;
        }
    }

    public partial class SubTypeRepository : GenericRepository<SubType>
    {
        public IQueryable<SubType> GetAllForQuery()
        {
            return dbContext.SubTypes;
        }
        public IEnumerable<object> GetForJSON(NameValueCollection queryString)
        {
            List<object> result = new List<object>();
            foreach(var theType in dbContext.TheTypes.ToList())
            {
                if (queryString[$"T{theType.ResType_ID}"] != null)
                {
                    foreach (var subType in theType.SubTypes)
                    {
                        result.Add(new { subType.SubT_ID, subType.SubT_Contect, subType.SubT_ResTypeID });
                    }
                }
            }
            return result;
        }
    }

    public partial class TypeDetailRepository: GenericRepository<Type_Detail>
    {
        
    }

    public partial class QAsystemRepository : GenericRepository<QA_system>
    {
        public IEnumerable<QA_system> GetMessageByID(int id)
        {
            return dbContext.QA_system.Where(L => L.QA_MemberID == id)
                .Where(L=>L.QA_AnswerdDate == null)
                .ToList();
        }
    }

    public partial class NewsRepository : GenericRepository<News>
    {
        public IEnumerable<News> GetAllVaild()
        {
            return dbContext.News.Where(L => L.New_CreateDate.HasValue);
        }
    }
}