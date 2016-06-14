using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PythogorasSquare.Web.Foundation.Interfaces
{
    public interface IPsychoMatrixService
    {
        Task<IReadOnlyCollection<IQualityController>> GetQualitiesForAsync(DateTime birthDate);
    }
}