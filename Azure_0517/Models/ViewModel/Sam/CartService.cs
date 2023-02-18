using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Azure_0517.Models;
using Azure_0517.Models.ViewModel;

namespace Azure_0517.Models.ViewModel
{
    public class CartService
    {
        private Taste_ItEntities db = new Taste_ItEntities();
        
        //加入購物車
        public void AddToCart(int MemberId, int GiftId)
        {
            Gift_Detail cart = CheckCart(MemberId, GiftId);
            if (cart == null)
            {
                cart = new Gift_Detail();
                cart.GiftDet_MemberID = MemberId;
                cart.GiftDet_GiftID = GiftId;
                cart.GiftDet_GetDate = null;
                cart.GiftDet_GetQuantity = 1;
                db.Gift_Detail.Add(cart);
            }
            else
            {
                cart.GiftDet_GetQuantity += 1;
                db.Entry(cart).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
        }

        //檢查是否有同品項物品存在購物車
        public Gift_Detail CheckCart(int MemberId, int GiftId)
        {
            var item = db.Gift_Detail.Where(s => s.GiftDet_MemberID == MemberId && s.GiftDet_GiftID == GiftId &&s.GiftDet_GetDate==null).FirstOrDefault();
            return item;
        }

        //購物車清單
        public List<CartViewModel> CartList(int MemberId)
        {
            List<CartViewModel> cartItems = new List<CartViewModel>();
            var items = db.Gift_Detail.Where(g => g.GiftDet_MemberID == MemberId && g.GiftDet_GetDate==null);
            foreach (var item in items)
            {
                CartViewModel cartItem = new CartViewModel();
                //Member
                cartItem.MemberID = item.Member.Mem_ID;
                cartItem.MemberPoint = db.Members.Find(cartItem.MemberID).Mem_Point;

                //GiftDetail
                cartItem.GiftDet_ID = item.GiftDet_ID;
                cartItem.GiftID = item.GiftDet_GiftID;
                cartItem.GetQuantity = item.GiftDet_GetQuantity;

                //Gift
                var gift = db.Gifts.Find(item.GiftDet_GiftID);
                cartItem.Name = gift.Gift_Name;
                cartItem.Picture = gift.Gift_Picture;
                cartItem.Point = gift.Gift_Point;
                cartItem.LastQuantity = (int)gift.Gift_Quantity;

                //cartItem.Amount = (item.GiftDet_GetQuantity * gift.Gift_Point);
                //cartItem.isDelete = false;

                cartItems.Add(cartItem);
            }
            return cartItems;
        }
            
    }
}