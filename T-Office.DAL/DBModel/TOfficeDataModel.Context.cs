﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace T_Office.DAL.DBModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TOfficeEntities : DbContext
    {
        public TOfficeEntities()
            : base("name=TOfficeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ClientRegistrationDocumentData> ClientRegistrationDocumentData { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<RegistrationDocumentData> RegistrationDocumentData { get; set; }
        public virtual DbSet<RegistrationVehicleData> RegistrationVehicleData { get; set; }
        public virtual DbSet<VehicleRegistrationInstallments> VehicleRegistrationInstallments { get; set; }
        public virtual DbSet<VehicleRegistrations> VehicleRegistrations { get; set; }
    }
}