using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Segment.Analytics;

namespace AspNetMvcSample.Controllers
{
    public class AnalyticsController : Controller
    {
        protected readonly Analytics Analytics;

        protected readonly dynamic ViewModel = new ExpandoObject();

        public AnalyticsController(Analytics analytics)
        {
            Analytics = analytics;
            ViewModel.Analytics = Analytics;
        }
    }
}
