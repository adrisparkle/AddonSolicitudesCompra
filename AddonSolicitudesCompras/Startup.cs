using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AddonSolicitudesCompras.Models;
using AddonSolicitudesCompras.Models.Auth;
using AddonSolicitudesCompras.Logic;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AddonSolicitudesCompras.Startup))]

namespace AddonSolicitudesCompras
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            bool debugmode = false;
            app.Use(async (environment, next) =>
            {
                AccessLogs log = null;
                ApplicationDbContext _context = new ApplicationDbContext();
                var req = environment.Request;
                string endpoint = environment.Request.Path.ToString();
                Uri uri = req.Uri;
                var seg = uri.Segments;

                string verb = environment.Request.Method;
                int userid = 0;
                Int32.TryParse(environment.Request.Headers.Get("id"), out userid);
                string token = environment.Request.Headers.Get("token");

                ValidateAuth validator = new ValidateAuth();
                int resourceid = 0;
                //tiene resourseid
                if (Int32.TryParse(seg[seg.Length - 1], out resourceid))
                {
                    endpoint = "";
                    for (int i = 0; i < seg.Length - 1; i++)
                    {
                        endpoint += seg[i];
                    }
                }

                bool sup = validator.shallYouPass(userid, token, endpoint, verb, out log);

                if (!debugmode && !sup)
                {
                    environment.Response.StatusCode = 401;
                    environment.Response.Body = new MemoryStream();

                    var newBody = new MemoryStream();
                    newBody.Seek(0, SeekOrigin.Begin);
                    var newContent = new StreamReader(newBody).ReadToEnd();

                    newContent += "You shall no pass.";

                    environment.Response.Body = newBody;
                    environment.Response.Write(newContent);
                    //log = _context.AccessLogses.FirstOrDefault(x => x.Id == logId);
                   
                }
                else
                {
                    await next();
                    //log = _context.AccessLogses.FirstOrDefault(x => x.Id == logId);
                }

            }
            );
            //app.UseStaticFiles();
            ConfigureAuth(app);
        }
    }
}
