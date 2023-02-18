using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models.ViewModel;
using Azure_0517.Models.Repository;
using Azure_0517.Models;
using System.Data.Entity;

namespace Azure_0517.Models.Repository
{
    public partial class MemberRepository : GenericRepository<Member>
    {
        public Member M_LoginCheck(LoginVM loginVM)
        {
            return dbContext.Members.FirstOrDefault(m => m.Mem_Account == loginVM.username && m.Mem_Password == loginVM.password);
        }

        public Member ThirdPartyLoginCheck(string from, string thirdUserId)
        {
            return dbContext.Members.FirstOrDefault(m => m.Mem_Source == from && m.Mem_SourceID == thirdUserId);
        }


        public Member Admin_LoginCheck(LoginVM loginVM)
        {
            return dbContext.Members.FirstOrDefault(m => m.Mem_Account == loginVM.username && m.Mem_Password == loginVM.password && m.Mem_RoleID == 3);
        }

        public Member M_ForgotPasswordCheck(ForgotPass forgotPass)
        {
            return dbContext.Members.FirstOrDefault(m => m.Mem_Account == forgotPass.Account && m.Mem_Mail == forgotPass.Email);
        }
        //public Member M_GetUser()
        //{
        //    return dbContext.Members.FirstOrDefault(m => m.Mem_Account);
        //}
    }


    public partial class RestaurantRepository : GenericRepository<Restaurant>
    {
        public Restaurant R_LoginCheck(LoginVM loginVM)
        {
            return dbContext.Restaurants.FirstOrDefault(r => r.Res_Account == loginVM.username && r.Res_Password == loginVM.password);
        }

        public Restaurant R_ForgotPasswordCheck(ForgotPass forgotPass)
        {
            return dbContext.Restaurants.FirstOrDefault(r => r.Res_Account == forgotPass.Account && r.Res_Email == forgotPass.Email);
        }

        public IEnumerable<Restaurant> R_Special()
        {
            var d = dbContext.Restaurants.Where(r => r.Res_IntroductionContext != null);
            var m = d.OrderBy(x => Guid.NewGuid()).ToList();
            return m.Take(5);
        }

        public IEnumerable<Restaurant> R_Flipper()
        {
            var star = dbContext.Restaurants.Where(r => r.Res_StarRate >= 4);
            var m = star.OrderBy(x => Guid.NewGuid()).ToList();
            return m.Take(8);
        }

    }


    public partial class NewsRepository : GenericRepository<News>
    {
        public IEnumerable<News> News_Newin()
        {
            return dbContext.News.Where(r => r.New_CreateDate >= new DateTime(2019, 5, 10) && r.New_CreateDate <= new DateTime(2019, 5, 29)).Take(7).ToList();
        }

        public IEnumerable<News> News_HotIssue()
        {
            var m = dbContext.News.Where(n=>n.New_MemberID==3).OrderBy(x => Guid.NewGuid()).ToList();
            var a= m.Where(x => x.GetImg.Count != 0).Take(3);
            return a.OrderBy(x => x.New_CreateDate);
        }

        public IEnumerable<News> News_ClickRate()
        {
            var c = dbContext.News.Where(r => r.New_ClickRate >= 30);
            var m = c.OrderBy(x => Guid.NewGuid()).ToList();
            return m.Where(x => x.GetImg.Count != 0).Take(4);
        }
    }

    public partial class DiscountRepository : GenericRepository<Discount>
    {
        public IEnumerable<Discount> Discount_Cube()
        {
            var d=dbContext.Discounts.Where(L=>L.Dis_Start == new DateTime(2019,3,1)).OrderBy(x => Guid.NewGuid()).ToList();
            return d.Take(4);
        }
        public byte[] GetDiscountImageByid(int id)
        {
            byte[] image = dbContext.Discounts.Find(id).Dis_Picture;
            return image;


        }

    }


  

}