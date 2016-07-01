using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObject
{
    public class User
    {
        private static User _authenticatedUser;

        public static void Set_authenticated_user(User user)
        {
            _authenticatedUser = user;
        }
        public static User Get_authenticated_user()
        {
            return _authenticatedUser;
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public int Role { get; set; }
        public int Team { get; set; }
        public DateTime Last_login_date { get; set; }
        public bool Is_deleted { get; set; }
    }
}
