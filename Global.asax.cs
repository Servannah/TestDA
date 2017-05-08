using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestDA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(TestDA.Common.ValidateNotValidForProperty.ValidInteger), typeof(TestDA.Common.ValidateNotValidForProperty.ValidIntegerValidator));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(TestDA.Common.ValidateNotValidForProperty.ValidDecimal), typeof(TestDA.Common.ValidateNotValidForProperty.ValidDecimalValidator));
        }
       
    }
}
