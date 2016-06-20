using System;
using System.Threading.Tasks;
using System.Web.Http;
using PythogorasSquare.Common;
using PythogorasSquare.Web.Foundation.Interfaces;
using PythogorasSquare.Web.Foundation.Responses;

namespace PythogorasSquare.Web.API.Controllers
{
    [RoutePrefix("rest/pythogoras"), UsedImplicitly]
    public class PythogorasController : ApiController
    {
        private readonly IPsychoMatrixService _psychoMatrixService;


        public PythogorasController(IPsychoMatrixService psychoMatrixService)
        {
            _psychoMatrixService = psychoMatrixService;
        }


        [ActionName("psychoMatrix")]
        public async Task<IHttpActionResult> GetPsychoMatrixFor(int year, int month, int day)
        {
            try
            {
                var birthDate = new DateTime(year, month, day);
                var qualities = await _psychoMatrixService.GetQualitiesForAsync(birthDate);

                return Json(new PsychoMatrixResponse(PsychoMatrixServiceResponses.Success, qualities));
            }
            catch (ArgumentException)
            {
                return Json(new PsychoMatrixResponse(PsychoMatrixServiceResponses.Failed, null));
            }
        }
    }
}