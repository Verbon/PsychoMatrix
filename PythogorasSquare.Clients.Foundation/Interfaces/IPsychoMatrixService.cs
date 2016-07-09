using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PythogorasSquare.Common;

namespace PythogorasSquare.Clients.Foundation.Interfaces
{
    public interface IPsychoMatrixService
    {
        Task InitializeAsync();

        Task<Tuple<DateTime, IReadOnlyCollection<IQualityController>>> GetLastSeenPsychoMatrixOrDefaultAsync();

        Task<OperationResult<IReadOnlyCollection<IQualityController>>> GetPsychoMatrixForAsync(DateTime birthDate);
    }
}