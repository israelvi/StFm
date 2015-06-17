using System;
using System.Linq;
using System.Web.Mvc;
using DataLayer.Entitties;
using DataLayer.Model.Management;
using DataLayer.Repository.Management;
using DataLayer.Repository.Shared;
using InfrastructureLayer.Component.Grid;
using Newtonsoft.Json;

namespace Portal.Web.Areas.Management.Controllers
{
    [Authorize]
    public class CatUserController : Controller
    {
        // GET: Management/CatUser
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid()
        {
            using (var db = new PuEntities())
            {

                var result = db.AspNetUsers
                    .Where(e => e.IsObsolete == false).Select(user => new
                    {
                        user.Id,
                        user.UserName,
                        FullName = user.FirstName + " " + user.LastName,
                        user.Email,
                        Role = user.AspNetRoles.Select(i => i.Description).FirstOrDefault() ?? "ND"
                    }).ToGrid();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Upsert(int? id)
        {
            UserModel model = null;
            try
            {
                if (id.HasValue)
                {
                    using (var repository = new UserRepository())
                    {
                        model = repository.FindModelById(id.Value);
                    }
                }
                else
                {
                    model = new UserModel
                    {
                        Id = EntityConstants.NULL_VALUE
                    };
                }

                ViewBag.Model = JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
            }
            return View(model);
        }
    }
}