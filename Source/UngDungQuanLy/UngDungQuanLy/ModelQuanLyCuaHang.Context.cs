﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UngDungQuanLy
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class QuanLyCuaHangEntities : DbContext
    {
        public QuanLyCuaHangEntities()
            : base("name=QuanLyCuaHangEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<HangHoa> HangHoa { get; set; }
        public virtual DbSet<LoaiHangHoa> LoaiHangHoa { get; set; }
        public virtual DbSet<GiaoDich> GiaoDich { get; set; }
    
        public virtual ObjectResult<string> Procedure_LayTatCaTenLoai()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("Procedure_LayTatCaTenLoai");
        }
    
        public virtual int Procedure_Xoa1HangHoa(Nullable<int> param1)
        {
            var param1Parameter = param1.HasValue ?
                new ObjectParameter("param1", param1) :
                new ObjectParameter("param1", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Procedure_Xoa1HangHoa", param1Parameter);
        }
    
        public virtual ObjectResult<Procedure_Lay10HangHoaBanChay_Result> Procedure_Lay10HangHoaBanChay(Nullable<System.DateTime> param1, Nullable<System.DateTime> param2)
        {
            var param1Parameter = param1.HasValue ?
                new ObjectParameter("param1", param1) :
                new ObjectParameter("param1", typeof(System.DateTime));
    
            var param2Parameter = param2.HasValue ?
                new ObjectParameter("param2", param2) :
                new ObjectParameter("param2", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Procedure_Lay10HangHoaBanChay_Result>("Procedure_Lay10HangHoaBanChay", param1Parameter, param2Parameter);
        }
    }
}
