using System;
using System.Threading.Tasks;
using System.Web.Http;
using PythogorasSquare.Web.Foundation.Factories;
using PythogorasSquare.Web.Foundation.Interfaces;
using PythogorasSquare.Web.Foundation.Providers;
using PythogorasSquare.Web.Foundation.PsychoMatrix;
using PythogorasSquare.Web.Foundation.Qualities;
using PythogorasSquare.Web.Foundation.Responses;
using PythogorasSquare.Web.Repositories;

namespace PythogorasSquare.Web.API.Controllers
{
    [RoutePrefix("rest/pythogoras")]
    public class PythogorasController : ApiController
    {
        private readonly IPsychoMatrixService _psychoMatrixService;


        public PythogorasController()
        {
            var psychoMatrixUnitOfWorkFactory = new PsychoMatrixUnitOfWorkFactory();

            var qualityControllerFactory = new QualityControllerFactory();
            var qualityDetailedInfoEqualityComparer = new QualityDetailedInfoEqualityComparer();
            var qualityControllerProvider = new CachingQualityControllerProvider(qualityControllerFactory, qualityDetailedInfoEqualityComparer);

            _psychoMatrixService = new PsychoMatrixService(psychoMatrixUnitOfWorkFactory, qualityControllerProvider);
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
            catch (ArgumentException ex)
            {
                return Json(new PsychoMatrixResponse(PsychoMatrixServiceResponses.Failed, null));
            }
        }
    }
}