using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using PythogorasSquare.Common;
using PythogorasSquare.Web.Foundation.Interfaces;
using PythogorasSquare.Web.Foundation.Responses;

namespace PythogorasSquare.WebApp.Controllers
{
    [RoutePrefix("api/pythogoras"), UsedImplicitly]
    public class PythogorasController : Controller
    {
        private readonly IPsychoMatrixService _psychoMatrixService;


        public PythogorasController(IPsychoMatrixService psychoMatrixService)
        {
            _psychoMatrixService = psychoMatrixService;
        }


        [ActionName("psychoMatrix")]
        public async Task<ActionResult> GetPsychoMatrixFor(int year, int month, int day)
        {
            try
            {
                var birthDate = new DateTime(year, month, day);
                var qualities = await _psychoMatrixService.GetQualitiesForAsync(birthDate);

                return Json(new PsychoMatrixResponse(PsychoMatrixServiceResponses.Success, qualities), JsonRequestBehavior.AllowGet);
            }
            catch (ArgumentException)
            {
                return Json(new PsychoMatrixResponse(PsychoMatrixServiceResponses.Failed, null), JsonRequestBehavior.AllowGet);
            }
        }
    }
}