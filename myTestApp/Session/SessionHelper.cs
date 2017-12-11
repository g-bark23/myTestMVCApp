using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myTestApp.Session
{
    public static class SessionHelper
    {
        public static readonly String USERKEY = "_UserID";
        public static readonly String GROUPKEY = "_GroupID";
        public static readonly String PROJECTKEY = "_ProjectID";
        public static readonly String SELECTEDUSER = "_SelectedUserID";
    }
}
