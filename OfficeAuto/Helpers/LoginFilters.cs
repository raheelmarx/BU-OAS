using Microsoft.AspNetCore.Mvc.Filters;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OfficeAuto.Helpers
{
    public class LoginFilters : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //HttpContext.Session.SetString(SessionKeyName, "The Doctor");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
