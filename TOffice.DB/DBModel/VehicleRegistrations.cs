//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TOffice.DB.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class VehicleRegistrations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VehicleRegistrations()
        {
            this.VehicleRegistrationInstallments = new HashSet<VehicleRegistrationInstallments>();
        }
    
        public int ID { get; set; }
        public int ClientRegistrationDocumentDataID { get; set; }
        public decimal TotalAmount { get; set; }
        public Nullable<int> NumberOfInstallments { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public Nullable<System.DateTime> NextRegistrationDate { get; set; }
    
        public virtual ClientRegistrationDocumentData ClientRegistrationDocumentData { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehicleRegistrationInstallments> VehicleRegistrationInstallments { get; set; }
    }
}
