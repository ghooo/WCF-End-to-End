using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GeoLib.Contracts;
using GeoLib.Data;

namespace GeoLib.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class StatefulGeoManager : IStatefulGeoService
    {
        public StatefulGeoManager()
        {
            Console.WriteLine("StatefulGeoManager instantiated.");
        }

        private ZipCode _ZipCodeEntity;
        public void PushZip(string zip)
        {
            IZipCodeRepository zipCodeRepository = new ZipCodeRepository();

            _ZipCodeEntity = zipCodeRepository.GetByZip(zip);

            Console.WriteLine("A new zip code pushed value {0}", zip);
        }

        public ZipCodeData GetZipInfo()
        {
            ZipCodeData zipCodeData = null;

            if (_ZipCodeEntity != null)
            {
                zipCodeData = new ZipCodeData()
                {
                    City = _ZipCodeEntity.City,
                    State = _ZipCodeEntity.State.Abbreviation,
                    ZipCode = _ZipCodeEntity.Zip
                };
            }
            //else
            //{
            //    throw new ApplicationException();
            //}

            return zipCodeData;
        }

        public IEnumerable<ZipCodeData> GetZips(int range)
        {
            List<ZipCodeData> zipCodeData = new List<ZipCodeData>();

            if (_ZipCodeEntity != null)
            {
                IZipCodeRepository zipCodeRepository = new ZipCodeRepository();
                var zips = zipCodeRepository.GetZipsForRange(_ZipCodeEntity, range);
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
            }
            //else
            //{
            //    throw new ApplicationException();
            //}

            return zipCodeData;
        }
    }
}
