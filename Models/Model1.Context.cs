﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebQuanLySpare.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WebSparePartEntities : DbContext
    {
        public WebSparePartEntities()
            : base("name=WebSparePartEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<danhsachlinhkien> danhsachlinhkiens { get; set; }
        public virtual DbSet<dathang> dathangs { get; set; }
        public virtual DbSet<model> models { get; set; }
        public virtual DbSet<tbl_admin> tbl_admin { get; set; }
    }
}
