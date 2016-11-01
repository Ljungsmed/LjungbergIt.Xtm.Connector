using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjungbergIt.Xtm.Connector.Helpers
{
    public class ScUser
    {
        public User GetUser()
        {
            string stringUser = @"sitecore\xtmsystemadmin";
            User scUser = User.FromName(stringUser, false);
            return (scUser);
        }
    }
}
