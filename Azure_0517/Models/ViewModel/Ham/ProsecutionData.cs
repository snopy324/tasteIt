using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Azure_0517.Models;
using Azure_0517.Models.ViewModel;
namespace Azure_0517.Models.ViewModel
{
    public class ProsecutionData
    {
        private Taste_ItEntities db = new Taste_ItEntities();
        public void AddToProsecution(Report report,HttpRequestBase Request)
        {
            //不知道為什麼不能寫在這，再研究看看要怎麼寫在這邊，先改寫在Action中(3/26已解決但不清楚HttpRequestBase Request，有空再回來研究這是什麼東西)
            if (Request.Cookies["Role"].Value == "Mem")
            {
                report.Rep_MemberID =Convert.ToInt32(Request.Cookies["Key"].Value);
            }
            else if (Request.Cookies["Role"].Value == "Rest")
            {
                report.Rep_RestaurantID = Convert.ToInt32( Request.Cookies["Key"].Value);
            }
            else if (Request.Cookies["Role"].Value == "Adm")
            {
                report.Rep_AdministratorID = Convert.ToInt32( Request.Cookies["Key"].Value);
                //這個部分應該調整成跳出提示管理員不需使用檢舉，請自行處理，或直接管理員看不到檢舉按鈕
            }
            //Response.Cookies["Role"].Value = "Mem";//Rest//Adm
            //Response.Cookies["Key"].Value = "id";

            report.Rep_CreatedDate = DateTime.Now;
            report.Rep_Status = 1;
            db.Reports.Add(report);
            db.SaveChanges();
        }
        public List<ProsecutionManageViewModel> ProsecutionList()
        {
            //泛型的repository完全沒用到，這樣寫一定有寫程式效率的問題，一定要再研究這邊
            var Prosecutions = db.Reports.Where(s => s.Rep_Status == 1);
            List<ProsecutionManageViewModel> ProsItems = new List<ProsecutionManageViewModel>();
            //檢舉的Status=1
            foreach (var item in Prosecutions)
            {
                ProsecutionManageViewModel ProsItem = new ProsecutionManageViewModel();
                ProsItem.Rep_ID = item.Rep_ID;
                ProsItem.Rep_CreatedDate = item.Rep_CreatedDate;
                ProsItem.Rep_Content = item.Rep_Content;
                
                var member = db.Members.Find(item.Rep_MemberID);
                var res = db.Restaurants.Find(item.Rep_RestaurantID);
                var adm = db.Members.Find(item.Rep_AdministratorID);
                if(member!=null)
                {
                    ProsItem.MemberName = member.Mem_Name;
                }
                else if(res!=null)
                {
                    ProsItem.RestaurantName = res.Res_Name;
                }
                else if (adm != null)
                {
                    ProsItem.AdmName = adm.Mem_Name;
                }
                var AccuseType = db.AccuseTypes.Find(item.Rep_AccuseTypeID);
                ProsItem.AccuseTypeName = AccuseType.RepTyp_Content;
                if(item.Rep_SolvedDate==null)
                {
                    ProsItem.situation = "未處理";
                }
                else if(item.Rep_SolvedDate!=null)
                {
                    ProsItem.situation = "已處理";
                }
                ProsItems.Add(ProsItem);
            }
            return ProsItems;
        }
    }
}