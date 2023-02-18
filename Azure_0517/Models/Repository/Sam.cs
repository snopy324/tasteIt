using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using Azure_0517.Models;
using Azure_0517.Models.ViewModel;

namespace Azure_0517.Models.Repository
{
    public partial class GiftDetailRepository : GenericRepository<Gift_Detail>
    {
        //搜尋購物車清單
        public IEnumerable<Gift_Detail> SearchList(int memID)
        {
            var list = dbContext.Gift_Detail.Where(s => s.GiftDet_MemberID == memID && s.GiftDet_GetDate == null).ToList();
            return list;
        }

        //刪除購物車品項
        public bool DeleteCartItem(int GiftID ,int MemID)
        {
            var item = dbContext.Gift_Detail.FirstOrDefault(g => g.GiftDet_GiftID == GiftID && g.GiftDet_GetDate == null && g.GiftDet_MemberID== MemID);
            dbContext.Entry(item).State = System.Data.Entity.EntityState.Deleted;
            dbContext.SaveChanges();

            return true;
        }

        //清空購物車品項
        public bool ClearCart(int memID)
        {
            var list = SearchList(memID);
            if (list != null)
            {
                foreach (var item in list)
                {
                    var gift = GetByID(item.GiftDet_ID);
                    Delete(gift);
                }
                return true;
            }

            return false;
        }

        //結帳
        public bool CheckOut(int memID)
        {
            //條件x ,訂單總點數cpt ,會員剩餘點數lastPt  //剩餘數量 - 訂單數量 
            var member = dbContext.Members.Find(memID);
            var memPt = member.Mem_Point;

            var x = dbContext.Gift_Detail.Where(m => m.GiftDet_MemberID == memID && m.GiftDet_GetDate == null);
            var cpt = x.Select(g => g.Gift.Gift_Point * g.GiftDet_GetQuantity).Sum();
            var lastPt = memPt - cpt;

            if (lastPt >= 0)
            {
                
                var list = x.ToList(); //GiftDetail- Member沒結帳的購物車訂單
                for (int i = 0; i < list.Count; i++)
                {
                    var gift = dbContext.Gifts.Find(list[i].GiftDet_GiftID); //Member table
                    var GDQ = list[i].GiftDet_GetQuantity;
                    var LQ = gift.Gift_Quantity;

                    gift.Gift_Quantity = LQ - GDQ;
                    if (gift.Gift_Quantity >= 0)
                    {
                        list[i].GiftDet_GetDate = DateTime.Now;
                        dbContext.Entry(gift).State = System.Data.Entity.EntityState.Modified; //Gift table
                        dbContext.Entry(list[i]).State = System.Data.Entity.EntityState.Modified; //GiftDetail table
                    }
                    else
                    {
                        return false;
                    }
                }
                member.Mem_Point = lastPt;
                dbContext.Entry(member).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //====================================================================================================================

        //搜尋會員訂單 依日期分組 搜尋條件
        public IEnumerable<IGrouping<DateTime, CartViewModel>> ExchangeLogSearchDate(int memID,string sortOrder ,DateTime? date1 ,DateTime? date2)
        {
            
            var list = dbContext.Gift_Detail.Where(d => d.GiftDet_MemberID == memID && d.GiftDet_GetDate != null)
                       .Select(d => new CartViewModel
                       {
                           GiftID = d.GiftDet_GiftID,
                           GetDate = d.GiftDet_GetDate,
                           Picture = d.Gift.Gift_Picture,
                           Name = d.Gift.Gift_Name,
                           Point = d.Gift.Gift_Point,
                           GetQuantity = d.GiftDet_GetQuantity
                       })
                       //.GroupBy(d =>d.GetDate)
                       //.OrderByDescending(d => d.Key)
                       .ToList();

            if (date1 != null && date2 != null)
            {
                if (date1 > date2)
                {
                    var t = date1;
                    date1 = date2;
                    date2 = t;
                }
                 list = dbContext.Gift_Detail.Where(d => d.GiftDet_MemberID == memID && d.GiftDet_GetDate != null&& d.GiftDet_GetDate>=date1 && d.GiftDet_GetDate<=date2)
                      .Select(d => new CartViewModel
                      {
                          GiftID = d.GiftDet_GiftID,
                          GetDate = d.GiftDet_GetDate,
                          Picture = d.Gift.Gift_Picture,
                          Name = d.Gift.Gift_Name,
                          Point = d.Gift.Gift_Point,
                          GetQuantity = d.GiftDet_GetQuantity
                      })
                      //.GroupBy(d =>d.GetDate)
                      //.OrderByDescending(d => d.Key)
                      .ToList();
            }

            if (String.IsNullOrEmpty(sortOrder))
            {
               
                var list1 = list.GroupBy(d => d.GetDate.Value.Date).OrderByDescending(L => L.Key).ToList();
                return list1;
            }

            else
            {
                var list1 = list.GroupBy(d => d.GetDate.Value.Date).OrderBy(L => L.Key).ToList();
                return list1;
            }
        }
    }

    public partial class GiftRepository : GenericRepository<Gift>
    {
        public IEnumerable<Gift> Search(string sortOrder, string searchString)
        {
            var list = this.GetAll().AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(g => g.Gift_Name.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(sortOrder))
            {
                list = list.OrderByDescending(g => g.Gift_Point);
            }
            else
            {
                list = list.OrderBy(g => g.Gift_Point);
            }

            return list;

        }
    }

}