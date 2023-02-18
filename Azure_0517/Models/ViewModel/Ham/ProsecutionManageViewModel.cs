using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Azure_0517.Models;

namespace Azure_0517.Models.ViewModel
{
    public class ProsecutionManageViewModel
    {



        //Report
        [DisplayName("檢舉事件編號")]
        public int Rep_ID { get; set; }
        //檢舉編號
        [DisplayName("檢舉日期")]
        public DateTime Rep_CreatedDate { get; set; }
        //檢舉日期
        [DisplayName("回報內容")]
        public string Rep_Content { get; set; }
        //回報內容
        public int ProsecutionLocation_ID { get; set; }
        //查看

        //Member
        public string MemberName { get; set; }
        //會員檢舉人名字

        //Restaurant
        public string RestaurantName { get; set; }
        //餐廳檢舉人名字
        public string AdmName { get; set; }
        //管理員檢舉人名字
        //管理員檢舉人名字目前除非做進一步的權限管理設計，否則不會用到
        //AccuseType
        [DisplayName("檢舉類型")]
        public string AccuseTypeName { get; set; }
        //檢舉類型名稱

        //自創的屬性
        [DisplayName("處理狀態")]
        public string situation { get; set; }
        //處理狀態

        //檢舉事件編號 檢舉日期   檢舉類型名稱  回報內容 處理狀態     檢舉人    查看 編輯狀態(改成已編輯這樣)

        //public ProsecutionManageViewModel(int id)
        //{
        //    Report target = db.Reports.Find(id);
        //    this.Rep_ID = target.Rep_ID;
        //    this.Rep_CreatedDate = target.Rep_CreatedDate;

        //    this.MemberName = target.Member.Mem_Name;
        // }修成寫的，不明所以

    }
}