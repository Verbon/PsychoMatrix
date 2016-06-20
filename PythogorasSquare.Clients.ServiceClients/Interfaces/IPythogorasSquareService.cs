using System.Threading.Tasks;
using PythogorasSquare.Clients.ServiceClients.DataContracts.Responses;

namespace PythogorasSquare.Clients.ServiceClients.Interfaces
{
    public interface IPythogorasSquareService
    {
        Task<PsychoMatrixServiceReponse> GetPsychoMatrixQualitiesAsync(int year, int month, int day);
    }
}