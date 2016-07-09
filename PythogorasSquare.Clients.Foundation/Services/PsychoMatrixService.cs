using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PythogorasSquare.Clients.Foundation.Interfaces;
using PythogorasSquare.Clients.ServiceClients.DataContracts;
using PythogorasSquare.Clients.ServiceClients.Interfaces;
using PythogorasSquare.Clients.Store.Common.Storage;
using PythogorasSquare.Common;
using PythogorasSquare.Common.Caching;
using PythogorasSquare.Common.Serializers;
using PythogorasSquare.Foundation.Interfaces;

namespace PythogorasSquare.Clients.Foundation.Services
{
    [UsedImplicitly]
    public class PsychoMatrixService : IPsychoMatrixService
    {
        private const string PsychoMatrixContentTypeIndetifier = "PsychoMatrix";

        private readonly IPythogorasSquareService _pythogorasSquareService;
        private readonly IContentCacheService _psychoMatrixCacheService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IStorageManager _storageManager;
        private readonly IEntityControllerProvider<ServiceQuality, IQualityController> _qualityControllerProvider;


        public PsychoMatrixService(
            IPythogorasSquareService pythogorasSquareService,
            IContentCacheService psychoMatrixCacheService,
            IJsonSerializer jsonSerializer,
            IStorageManager storageManager,
            IEntityControllerProvider<ServiceQuality, IQualityController> qualityControllerProvider)
        {
            _pythogorasSquareService = pythogorasSquareService;
            _psychoMatrixCacheService = psychoMatrixCacheService;
            _jsonSerializer = jsonSerializer;
            _storageManager = storageManager;
            _qualityControllerProvider = qualityControllerProvider;
        }


        public async Task InitializeAsync()
        {
            await _psychoMatrixCacheService.InitializeAsync(PsychoMatrixContentTypeIndetifier);
        }

        public async Task<Tuple<DateTime, IReadOnlyCollection<IQualityController>>> GetLastSeenPsychoMatrixOrDefaultAsync()
        {
            var cachedPsychoMatrixes = await _psychoMatrixCacheService.TryGetCachedContentPathesAsync();
            if (cachedPsychoMatrixes.IsSuccessful)
            {
                var lastSeenCachedPsychoMatrixPath = cachedPsychoMatrixes.Result.Single();
                var lastSeenPsychoMatrixFile = await _storageManager.TryGetFileAsync(lastSeenCachedPsychoMatrixPath);
                if (lastSeenPsychoMatrixFile.IsSuccessful)
                {
                    using (var lastSeenPsychoMatrixStream = await lastSeenPsychoMatrixFile.Result.OpenStreamForReadAsync())
                    {
                        var buffer = new byte[lastSeenPsychoMatrixStream.Length];
                        await lastSeenPsychoMatrixStream.ReadAsync(buffer, 0, buffer.Length);
                        var serializedQualityControllers = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                        var psychoMatrix = _jsonSerializer.Deserialize<PsychoMatrix>(serializedQualityControllers);
                        var qualities = psychoMatrix.Qualities.Select(_qualityControllerProvider.GetControllerFor).ToList();

                        return new Tuple<DateTime, IReadOnlyCollection<IQualityController>>(psychoMatrix.BirthDate, qualities);
                    }
                }
            }

            var birthDate = DateTime.UtcNow;
            var qualityControllers = await GetPsychoMatrixForAsync(birthDate);

            return new Tuple<DateTime, IReadOnlyCollection<IQualityController>>(birthDate, qualityControllers.IsSuccessful ? qualityControllers.Result : new List<IQualityController>(0));
        }

        public async Task<OperationResult<IReadOnlyCollection<IQualityController>>> GetPsychoMatrixForAsync(DateTime birthDate)
        {
            var getPsychoMatrixServiceResponse = await _pythogorasSquareService.GetPsychoMatrixQualitiesAsync(birthDate.Year, birthDate.Month, birthDate.Day);
            if (getPsychoMatrixServiceResponse != null && getPsychoMatrixServiceResponse.IsSuccessful)
            {
                var serializedPsychoMatrix = _jsonSerializer.Serialize(new PsychoMatrix(birthDate, getPsychoMatrixServiceResponse.Qualities));
                var streamedPsychoMatrix = new MemoryStream(Encoding.UTF8.GetBytes(serializedPsychoMatrix));
                await _psychoMatrixCacheService.DeleteContentAsync(c => true);
                await _psychoMatrixCacheService.AddContentAsync(birthDate.ToString(), streamedPsychoMatrix);

                return getPsychoMatrixServiceResponse.Qualities.Select(_qualityControllerProvider.GetControllerFor).ToList();
            }

            return OperationResult<IReadOnlyCollection<IQualityController>>.CreateUnsuccessful();
        }



        [DataContract]
        private class PsychoMatrix
        {
            [DataMember(Name = "birthDate")]
            public DateTime BirthDate { get; [UsedImplicitly] set; }

            [DataMember(Name = "qualities")]
            public IEnumerable<ServiceQuality> Qualities { get; [UsedImplicitly] set; }


            public PsychoMatrix(DateTime birthDate, IEnumerable<ServiceQuality> qualities)
            {
                BirthDate = birthDate;
                Qualities = qualities;
            }
        }
    }
}