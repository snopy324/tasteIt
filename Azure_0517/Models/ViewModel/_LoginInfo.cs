using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Azure_0517.Models;

namespace Azure_0517.Models
{
    static public class _LoginInfo
    {
        static Taste_ItEntities dbContext = new Taste_ItEntities();
        public enum _LoginRole
        {
            Member,
            Restaurant,
            Administrator,
            Guest
        }

        static public _LoginRole GetRole(this HttpRequestBase Request)
        {
            if(Request.Cookies["Role"] == null)
            {
                return _LoginRole.Guest;
            }
            switch(Request.Cookies["Role"].Value)
            {
                case "Member":
                    return _LoginRole.Member;
                case "Restaurant":
                    return _LoginRole.Restaurant;
                case "Administrator":
                    return _LoginRole.Administrator;
                default:
                    return _LoginRole.Guest;
            }
        }

        static public int? GetRoleKey(this HttpRequestBase Request)
        {
            if (Request.Cookies["Key"] == null)
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(Request.Cookies["Key"].Value);
            }
        }

        static public void SetRole(this HttpResponseBase Response, _LoginRole _LoginRole)
        {
            Response.Cookies["Role"].Value = _LoginRole.ToString();
        }

        static public void SetKey(this HttpResponseBase Response, int Key)
        {
            Response.Cookies["Key"].Value = Key.ToString();
        }

        static public string GetRoleName(this HttpRequestBase Request)
        {
            if (Request.Cookies["Role"] == null)
            {
                return _LoginRole.Guest.ToString();
            }
            int? key = Request.GetRoleKey().Value;
            switch (Request.Cookies["Role"].Value)
            {
                case "Member":
                    return dbContext.Members.Find(key).Mem_Name;
                case "Restaurant":
                    return dbContext.Restaurants.Find(key).Res_Name;
                case "Administrator":
                    return dbContext.Members.Find(key).Mem_Account;
                default:
                    return _LoginRole.Guest.ToString();
            }
        }

        static public int GetRolePoint(this HttpRequestBase Request)
        {
            int? key = Request.GetRoleKey().Value;
            if (Request.Cookies["Role"].Value == "Member")
            {
                dbContext = new Taste_ItEntities();
                return dbContext.Members.Find(key).Mem_Point;
            }
            return 0;
        }


    }
}