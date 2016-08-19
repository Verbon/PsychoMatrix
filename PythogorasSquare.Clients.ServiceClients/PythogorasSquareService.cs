using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using PythogorasSquare.Clients.ServiceClients.DataContracts.Responses;
using PythogorasSquare.Clients.ServiceClients.Interfaces;
using PythogorasSquare.Common;
using PythogorasSquare.Common.Configuration;
using PythogorasSquare.Common.Serializers;

namespace PythogorasSquare.Clients.ServiceClients
{
    [UsedImplicitly]
    public class PythogorasSquareService : IPythogorasSquareService
    {
        private const string ServiceUrlKey = "ServiceUrl";
        private const double ServiceResponseTimeoutInSeconds = 30;

        private const string GetPsychoMatrixMethod = "api/pythogoras/psychoMatrix/{0}/{1}/{2}";

        private readonly Uri _pythogorasSquareBaseAddress;

        private readonly IJsonSerializer _jsonSerializer;


        public PythogorasSquareService(IAppConfigService appConfig, IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
            _pythogorasSquareBaseAddress = new Uri(appConfig.GetString(ServiceUrlKey));
        }


        public async Task<PsychoMatrixServiceReponse> GetPsychoMatrixQualitiesAsync(int year, int month, int day)
        {
            var getPsychoMatrixUrl = String.Format(GetPsychoMatrixMethod, year, month, day);

            return await GetAsync<PsychoMatrixServiceReponse>(getPsychoMatrixUrl);
        }


        private async Task<TServiceResponse> GetAsync<TServiceResponse>(string requestUri) where TServiceResponse : ServiceResponse
        {
            using (var client = CreateClient())
            {
                var response = await client.GetAsync(requestUri);

                return await GetResponseAsync<TServiceResponse>(response);
            }
        }

        private async Task<TServiceResponse> GetResponseAsync<TServiceResponse>(HttpResponseMessage response) where TServiceResponse : ServiceResponse
        {
            try
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                return _jsonSerializer.Deserialize<TServiceResponse>(jsonResponse);
            }
            catch (Exception)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                return null;
            }
        }

        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = _pythogorasSquareBaseAddress,
                Timeout = TimeSpan.FromSeconds(ServiceResponseTimeoutInSeconds)
            };

            return httpClient;
        }
    }
}