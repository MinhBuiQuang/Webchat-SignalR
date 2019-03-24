//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChatApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.MessageLogs = new HashSet<MessageLog>();
        }
    
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientChatID { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public Nullable<System.DateTime> LastOnline { get; set; }
        public Nullable<bool> IsOnline { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessageLog> MessageLogs { get; set; }
    }
}
