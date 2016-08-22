using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PythogorasSquare.Web.Foundation.Interfaces;
using PythogorasSquare.WebApp.ViewModels.Home;
using PythogorasSquare.WebApp.ViewModels.Qualities;

namespace PythogorasSquare.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPsychoMatrixService _psychoMatrixService;


        public HomeController(IPsychoMatrixService psychoMatrixService)
        {
            _psychoMatrixService = psychoMatrixService;
        }


        [HttpGet]
        public Task<ActionResult> PsychoMatrix()
        {
            return PsychoMatrix(DateTime.UtcNow);
        }

        [HttpPost]
        public async Task<ActionResult> PsychoMatrix(DateTime dateOfBirth)
        {
            var qualities = await _psychoMatrixService.GetQualitiesForAsync(dateOfBirth);
            var qualitiesViewModels = qualities.Select(CreateForm).ToList();
            var psychoMatrixViewModel = new PsychoMatrixViewModel(dateOfBirth, qualitiesViewModels);

            return View(psychoMatrixViewModel);
        }


        private static QualityViewModel CreateForm(IQualityController qualityController)
        {
            var qualityViewModel = new QualityViewModel(qualityController.Name, qualityController.Power, qualityController.Description);

            return qualityViewModel;
        }
    }
}