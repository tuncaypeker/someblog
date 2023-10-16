﻿namespace SomeBlog.Model
{
    public class InstagramAccount : Core.ModelBase
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Pk { get; set; }
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Change Password **password** with real password
        /// </summary>
        public string StateData { get; set; }
        public System.DateTime LoginDate { get; set; }
        public string Password { get; set; }
        public int Followers { get; set; }
    }
}
