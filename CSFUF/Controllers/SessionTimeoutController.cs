using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CSFUF.Controllers
{
    public class SessionTimeoutController : Controller
    {
        // GET: SessionTimeout
        private static Timer sessionTimeoutTimer;

        public void StartSessionTimeoutMonitoring()
        {
            if (sessionTimeoutTimer == null)
            {
                sessionTimeoutTimer = new Timer(SessionTimeoutCheck, null, 0, Session.Timeout * 1000);
            }
        }

        private void SessionTimeoutCheck(object state)
        {
            var user = User.Identity;
            if (user != null && user.IsAuthenticated)
            {
                var session = HttpContext.Session;
                var lastActivityTime = (DateTime?)session["LastActivityTime"];

                if (lastActivityTime != null && DateTime.Now - lastActivityTime > TimeSpan.FromMinutes(Session.Timeout))
                {
                    session.Abandon();
                    Response.Redirect(Url.Action("Login", "Account"));
                }
                else
                {
                    session["LastActivityTime"] = DateTime.Now;
                }
            }
        }
    }
}