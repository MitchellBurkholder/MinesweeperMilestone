using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace MinesweeperMilestone.Filters
{
    public class SessionCheckFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Check if user is login
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("User") == null)
            {
                context.Result = new RedirectResult("User/Index");
            }
        }
    }
}
