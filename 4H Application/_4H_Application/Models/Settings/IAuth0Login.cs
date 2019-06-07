//
// _4H_Application.Models.Settings.IAuth0Login.cs: Interface that serves as
//   a platform-independent method logging into the app using Auth0
//
// Author(s):
//   Joel Krause (jkjk8080@gmail.com)
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4H_Application.Models.Settings
{
    public interface IAuth0Login
    {
        void AppLogin();
        void AppLogout();
    }
}
