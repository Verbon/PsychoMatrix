using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PythogorasSquare.Clients.Foundation.Interfaces
{
    public interface IPsychoMatrixService
    {
        Task<IReadOnlyCollection<IQualityController>> GetPsychoMatrixForAsync(DateTime birthDate);
    }
}