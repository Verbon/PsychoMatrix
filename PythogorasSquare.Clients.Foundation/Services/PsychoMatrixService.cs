using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PythogorasSquare.Clients.Foundation.Interfaces;
using PythogorasSquare.Clients.ServiceClients.DataContracts;
using PythogorasSquare.Clients.ServiceClients.Interfaces;
using PythogorasSquare.Common;
using PythogorasSquare.Foundation.Interfaces;

namespace PythogorasSquare.Clients.Foundation.Services
{
    [UsedImplicitly]
    public class PsychoMatrixService : IPsychoMatrixService
    {
        private readonly IPythogorasSquareService _pythogorasSquareService;
        private readonly IEntityControllerProvider<ServiceQuality, IQualityController> _qualityControllerProvider;


        public PsychoMatrixService(
            IPythogorasSquareService pythogorasSquareService,
            IEntityControllerProvider<ServiceQuality, IQualityController> qualityControllerProvider)
        {
            _pythogorasSquareService = pythogorasSquareService;
            _qualityControllerProvider = qualityControllerProvider;
        }


        public async Task<IReadOnlyCollection<IQualityController>> GetPsychoMatrixForAsync(DateTime birthDate)
        {
            var getPsychoMatrixServiceResponse = await _pythogorasSquareService.GetPsychoMatrixQualitiesAsync(birthDate.Year, birthDate.Month, birthDate.Day);
            if (getPsychoMatrixServiceResponse != null && getPsychoMatrixServiceResponse.IsSuccessful)
            {
                return getPsychoMatrixServiceResponse.Qualities.Select(_qualityControllerProvider.GetControllerFor).ToList();
            }

            return null;
        }
    }
}