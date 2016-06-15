using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PythogorasSquare.Common.AsyncLinq;
using PythogorasSquare.Web.Foundation.Interfaces;
using PythogorasSquare.Web.Repositories.Interfaces;

namespace PythogorasSquare.Web.Foundation.PsychoMatrix
{
    public class PsychoMatrixService : IPsychoMatrixService
    {
        private static readonly IEnumerable<char> DecimalDigits = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };


        private readonly IPsychoMatrixUnitOfWorkFactory _psychoMatrixUnitOfWorkFactory;
        private readonly IQualityControllerProvider _qualityControllerProvider;


        public PsychoMatrixService(
            IPsychoMatrixUnitOfWorkFactory psychoMatrixUnitOfWorkFactory,
            IQualityControllerProvider qualityControllerProvider)
        {
            _psychoMatrixUnitOfWorkFactory = psychoMatrixUnitOfWorkFactory;
            _qualityControllerProvider = qualityControllerProvider;
        }


        public async Task<IReadOnlyCollection<IQualityController>> GetQualitiesForAsync(DateTime birthDate)
        {
            var year = birthDate.Date.Year;
            var month = birthDate.Date.Month;
            var day = birthDate.Date.Day;
            var workingNumbers = CalculateWorkingNumbers(year, month, day);
            var resultSequence = $"{year}{month}{day}{String.Join(String.Empty, workingNumbers)}";
            var poweredQualities = DecimalDigits.Select(i => new { QualityAssociatedDigit = i - '0', QualityPower = resultSequence.Count(c => c == i) });

            using (var uow = _psychoMatrixUnitOfWorkFactory.Create())
            {
                var qualityDetailedInfoRepository = uow.QualityDetailedInfoRepository;
                var qualitiesDetailedInfos = await poweredQualities
                    .SelectAsync(pq => qualityDetailedInfoRepository.GetByPoweredQualityAsync(pq.QualityAssociatedDigit, pq.QualityPower));

                return qualitiesDetailedInfos.Select(_qualityControllerProvider.GetControllerFor).ToList();
            }
        }


        private static IReadOnlyList<int> CalculateWorkingNumbers(int year, int month, int day)
        {
            var dateParts = new[] { year, month, day };

            var firstWorkingNumber = dateParts.Sum(i => GetDigitsSum(i));
            var secondWorkingNumber = GetDigitsSum(firstWorkingNumber);
            var thirdWorkingNumber = firstWorkingNumber - 2 * (day < 10 ? day : day / 10);
            var fourthWorkingNumber = GetDigitsSum(thirdWorkingNumber);

            return new[] { firstWorkingNumber, secondWorkingNumber, thirdWorkingNumber, fourthWorkingNumber };
        }

        private static int GetDigitsSum(int number)
            => number.ToString().Sum(d => int.Parse(d.ToString()));
    }
}