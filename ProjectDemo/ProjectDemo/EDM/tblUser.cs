//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectDemo.EDM
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblUser()
        {
            this.tblcarts = new HashSet<tblcart>();
            this.tblorders = new HashSet<tblorder>();
        }
    
        public int user_id { get; set; }
        public string f_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string l_name { get; set; }
        public string address { get; set; }
        public Nullable<int> state_id { get; set; }
        public Nullable<int> city_id { get; set; }
        public string profile_img { get; set; }
        public Nullable<decimal> mobile { get; set; }
        public string gender { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblcart> tblcarts { get; set; }
        public virtual tblcity tblcity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblorder> tblorders { get; set; }
        public virtual tblstate tblstate { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        public virtual tblUser tblUser2 { get; set; }
    }
}
