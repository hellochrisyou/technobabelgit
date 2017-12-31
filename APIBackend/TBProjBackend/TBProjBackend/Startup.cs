using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Collections.Generic;
using Swashbuckle.Application;

[assembly: OwinStartup(typeof(TBProjBackend.Startup))]

namespace TBProjBackend
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            
            /*
           GlobalConfiguration.Configure(WebApiConfig.Register);

           
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "WebAPI");
                c.IncludeXmlComments(GetXmlCommentsPath());
            }).EnableSwaggerUi();
            */
            

            appBuilder.UseWindowsAzureActiveDirectoryBearerAuthentication(
               new Microsoft.Owin.Security.ActiveDirectory.WindowsAzureActiveDirectoryBearerAuthenticationOptions
               {
                   TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                   {
                       ValidAudiences = new List<string>() { "a1d7f1e9-b4a8-4b6e-b2c6-0c99e5e2ccea", "d245a54b-2409-4402-b859-dc507defd2ed" }
                   },
                   Tenant = "Technobabel.onmicrosoft.com"
               });

            appBuilder.UseWebApi(config);
        }

        protected static string GetXmlCommentsPath()
{
    return System.String.Format(@"{0}\bin\ApplicationName.XML", 
			System.AppDomain.CurrentDomain.BaseDirectory);
}
    }
}
