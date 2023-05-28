using System.Web.Mvc;

namespace Web.Areas.Team
{
    public class TeamAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Team";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Team_default",
                "Team/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
