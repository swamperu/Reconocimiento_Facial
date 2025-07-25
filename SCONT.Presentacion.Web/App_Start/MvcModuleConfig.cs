﻿using System;
using System.Web;

//[assembly: PreApplicationStartMethod(typeof(SCONT.Presentacion.Web.MvcRegisterConfig), "Initialize")]
namespace SCONT.Presentacion.Web
{
    public class MvcModuleConfig : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.LogRequest += new EventHandler(OnLogRequest);
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }

        public void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            // remove the Server Http header
            HttpContext.Current.Response.Headers.Remove("Server");
        }

    }
}
