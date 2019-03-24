using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class UserModel
    {
        //private WebChatEntities db;
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientChatID { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public Nullable<System.DateTime> LastOnline { get; set; }
    }        
}