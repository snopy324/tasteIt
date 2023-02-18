using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models;
namespace Azure_0517.Models.Repository
{
    public partial class NewsRepository : GenericRepository<News>
    {
        public IEnumerable<News> GetByRestID(int id)
        {
            return dbContext.News.Where(L => L.New_RestaurantID == id && L.Member.Mem_RoleID == 1).ToList();
        }
        public IEnumerable<News> GetByMemberID(int id)
        {
            return dbContext.News.Where(L => L.New_MemberID == id).ToList();
        }
        public IEnumerable<News> GetByRestID_News(int id)
        {
            return dbContext.News.Where(L => L.New_RestaurantID == id && L.Member.Mem_RoleID == 3).ToList();
        }
        public IEnumerable<News> GetByMyLove(int id)
        {
            return dbContext.Reports.Where(n => n.Rep_MemberID == id && n.Rep_Content == "like" && n.Rep_NewsID.HasValue && n.Rep_Status == 2).Select(news => news.News);
        }
    }

    public partial class CommentRepository : GenericRepository<Comment>
    {
        public IEnumerable<Comment> GetByRestID(int id)
        {
            return dbContext.Comments.Where(L => L.Com_RestaurantID == id).ToList();
        }
        public IEnumerable<Comment> GetByMemberID(int id)
        {
            return dbContext.Comments.Where(L => L.Com_MemberID == id).ToList();
        }
    }
    //======================================================
    public partial class ReportRepository : GenericRepository<Report>
    {
        public IEnumerable<Report> GetByNewsID(int id)
        {
            return dbContext.Reports.Where(L => L.Rep_NewsID == id).ToList();
        }

        public IEnumerable<Report> GetByCommentID(int id)
        {
            return dbContext.Reports.Where(L => L.Rep_CommentID == id).ToList();
        }
        public IEnumerable<Report> GetByCase()
        {
            return dbContext.Reports.Where(L => L.Rep_AccuseTypeID == 11).OrderByDescending(L=>L.Rep_ID).ToList();
        }
        public IEnumerable<Report> GetstringSolvedCase(string Solved)
        {
            if (Solved == "true")
                return dbContext.Reports.Where(s => s.Rep_AccuseTypeID==11).Where(s => s.Rep_SolvedDate.HasValue);
            else
                return dbContext.Reports.Where(s => s.Rep_AccuseTypeID==11).Where(s => !s.Rep_SolvedDate.HasValue);
        }
    }

}