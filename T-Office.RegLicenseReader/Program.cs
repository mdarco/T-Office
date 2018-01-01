using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eVehicleRegistrationCOM;

namespace reg_license_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Registration reg = new Registration();

            int initResponse = reg.Initialize();

            string readerName = string.Empty;
            int getReaderNameResponse = reg.GetReaderName(0, out readerName);

            int getSelectReaderResponse = reg.SelectReader(readerName);

            int processNewCardResponse = reg.ProcessNewCard();

            _DOCUMENT_DATA docData;
            int docDataResponse = reg.ReadDocumentData(out docData);

            _VEHICLE_DATA vehicleData;
            int vehicleDataResponse = reg.ReadVehicleData(out vehicleData);

            _PERSONAL_DATA personalData;
            int personalDataResponse = reg.ReadPersonalData(out personalData);

            int finalizeRsponse = reg.Finalize();
        }
    }
}
