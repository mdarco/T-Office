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
    
    public partial class Clients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clients()
        {
            this.ClientRegistrationDocumentData = new HashSet<ClientRegistrationDocumentData>();
        }
    
        public int ID { get; set; }
        public string OwnerPersonalNo { get; set; }
        public string OwnerPIB { get; set; }
        public string OwnerName { get; set; }
        public string OwnerSurnameOrBusinessName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEmail { get; set; }
        public string UserPersonalNo { get; set; }
        public string UserPIB { get; set; }
        public string UserName { get; set; }
        public string UserSurnameOrBusinessName { get; set; }
        public string UserAddress { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string RecommendedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientRegistrationDocumentData> ClientRegistrationDocumentData { get; set; }
    }
}
