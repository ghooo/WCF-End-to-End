using GeoLib.Contracts;
using GeoLib.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace GeoLib.Services
{
    // When transaction is turned on the session boundary is equated witht the transaction boundary, which means
    // after the transaction is over the session is over (esp. in PerSession). To turn this off use: 
    // ReleaseServiceInstanceOnTransactionComplete = false
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, ReleaseServiceInstanceOnTransactionComplete = false, InstanceContextMode = InstanceContextMode.PerSession)]
    public class GeoManager : IGeoService
    {
        public GeoManager()
        {
        }

        public GeoManager(IZipCodeRepository zipCodeRepository)
        {
            _ZipCodeRepository = zipCodeRepository;
        }

        public GeoManager(IStateRepository stateRepository)
        {
            _StateRepository = stateRepository;
        }

        public GeoManager(IZipCodeRepository zipCodeRepository, IStateRepository stateRepository)
        {
            _ZipCodeRepository = zipCodeRepository;
            _StateRepository = stateRepository;
        }

        IZipCodeRepository _ZipCodeRepository = null;
        IStateRepository _StateRepository = null;

        private int _Counter = 0;
        public ZipCodeData GetZipInfo(string zip)
        {
            ZipCodeData zipCodeData = null;

            IZipCodeRepository zipCodeRepository = _ZipCodeRepository ?? new ZipCodeRepository();

            ZipCode zipCodeEntity = zipCodeRepository.GetByZip(zip);
            if (zipCodeEntity != null)
            {
                zipCodeData = new ZipCodeData()
                {
                    City = zipCodeEntity.City,
                    State = zipCodeEntity.State.Abbreviation,
                    ZipCode = zipCodeEntity.Zip
                };
            }
            else
            {
                //throw new FaultException(string.Format("Zip code {0} not found.", zip));

                //throw new ApplicationException(string.Format("Zip code {0} not found.", zip));

                //ApplicationException ex = new ApplicationException(string.Format("Zip code {0} not found.", zip));

                //throw new FaultException<ApplicationException>(ex, "Reason for failure.");

                NotFoundData data = new NotFoundData()
                {
                    Message = string.Format("Zip code {0} not found.", zip),
                    When = DateTime.Now.ToString(),
                    User = "ghooo"
                };
                throw new FaultException<NotFoundData>(data, "Just another message.");
            }

            _Counter++;

            return zipCodeData;
        }

        public IEnumerable<string> GetStates(bool primaryOnly)
        {
            List<string> stateData = new List<string>();

            IStateRepository stateRepository = _StateRepository ?? new StateRepository();

            IEnumerable<State> states = stateRepository.Get(primaryOnly);
            if(states == null)
            {
                foreach (State state in states)
                    stateData.Add(state.Abbreviation);
            }
            return stateData;
        }

        public IEnumerable<ZipCodeData> GetZips(string state)
        {
            List<ZipCodeData> zipCodeData = new List<ZipCodeData>();

            IZipCodeRepository zipCodeRepository = _ZipCodeRepository ?? new ZipCodeRepository();
            var zips = zipCodeRepository.GetByState(state);
            if (zips != null)
            {
                foreach (ZipCode zipCode in zips)
                {
                    zipCodeData.Add(new ZipCodeData()
                    {
                        City = zipCode.City,
                        State = zipCode.State.Abbreviation,
                        ZipCode = zipCode.Zip
                    });

                }
            }

            return zipCodeData;
        }

        public IEnumerable<ZipCodeData> GetZips(string zip, int range)
        {
            List<ZipCodeData> zipCodeData = new List<ZipCodeData>();

            IZipCodeRepository zipCodeRepository = _ZipCodeRepository ?? new ZipCodeRepository();
            ZipCode zipEntity = zipCodeRepository.GetByZip(zip);
            var zips = zipCodeRepository.GetZipsForRange(zipEntity, range);
            if (zips != null)
            {
                foreach (ZipCode zipCode in zips)
                {
                    zipCodeData.Add(new ZipCodeData()
                    {
                        City = zipCode.City,
                        State = zipCode.State.Abbreviation,
                        ZipCode = zipCode.Zip
                    });

                }
            }

            return zipCodeData;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateZipCity(string zip, string city)
        {
            IZipCodeRepository zipCodeRepository = _ZipCodeRepository ?? new ZipCodeRepository();

            ZipCode zipEntity = zipCodeRepository.GetByZip(zip);
            if (zipEntity != null)
            {
                zipEntity.City = city;
                zipCodeRepository.Update(zipEntity);
            }
        }

        //[OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = false)]
        //[OperationBehavior(TransactionScopeRequired = false)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateZipCity(IEnumerable<ZipCityData> zipCityData)
        {
            IZipCodeRepository zipCodeRepository = _ZipCodeRepository ?? new ZipCodeRepository();
            int counter = 0;

            foreach (ZipCityData zipCityItem in zipCityData)
            {
                counter++;

                //if (counter == 2)
                //{
                //    throw new FaultException("sorry, no can do.");
                //}

                ZipCode zipCodeEntity = zipCodeRepository.GetByZip(zipCityItem.ZipCode);
                zipCodeEntity.City = zipCityItem.City;
                ZipCode updateItem = zipCodeRepository.Update(zipCodeEntity);
            }

            //using (TransactionScope scope = new TransactionScope())
            //{

            //    int counter = 0;

            //    foreach (ZipCityData zipCityItem in zipCityData)
            //    {
            //        counter++;

            //        if (counter == 2)
            //        {
            //            throw new FaultException("sorry, no can do.");
            //        }

            //        ZipCode zipCodeEntity = zipCodeRepository.GetByZip(zipCityItem.ZipCode);
            //        zipCodeEntity.City = zipCityItem.City;
            //        ZipCode updateItem = zipCodeRepository.Update(zipCodeEntity);
            //    }

            //    scope.Complete();
            //}

            //OperationContext.Current.SetTransactionComplete();

            //IZipCodeRepository zipCodeRepository = _ZipCodeRepository ?? new ZipCodeRepository();

            //Dictionary<string, string> cityBatch = new Dictionary<string, string>();

            //foreach (ZipCityData zipCityItem in zipCityData)
            //{
            //    cityBatch.Add(zipCityItem.ZipCode, zipCityItem.City);
            //}

            //zipCodeRepository.UpdateCityBatch(cityBatch);
        }
    }
}
