using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VST111.Controllers
{
    internal class HomeController : BaseController
    {
        public override object Index()
        {
            return View();
        }
        public object Search(string search)
        {
            return View(Provider.Select("Facility", $"location like '%{search}%'"));
        }
    }
}
