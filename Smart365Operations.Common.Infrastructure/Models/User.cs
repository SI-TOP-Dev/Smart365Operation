﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Models
{
    public class User
    {
        public User(string id,string username, string displayName, string email, string[] roles)
        {
            Id = id;
            Username = username;
            DisplayName = displayName;
            Email = email;
            Roles = roles;
        }
        public string Username
        {
            get;
            set;
        }

        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Email
        {
            get;
            set;
        }

        public string[] Roles
        {
            get;
            set;
        }
    }
}
