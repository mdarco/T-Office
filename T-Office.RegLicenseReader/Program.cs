using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using eVehicleRegistrationCOM;

namespace reg_license_test
{
    class Program
    {
        class ProgramResponse
        {
            public bool IsError { get; set; }
            public string ErrorMessage { get; set; }
            public RegLicenseData Result { get; set; }
        }

        class RegLicenseData
        {
            public _DOCUMENT_DATA DocumentData { get; set; }
            public _VEHICLE_DATA VehicleData { get; set; }
            public _PERSONAL_DATA PersonalData { get; set; }
        }

        static void Main(string[] args)
        {
            Registration reg = new Registration();

            int initResponse = reg.Initialize();
            if (initResponse != 0)
            {
                Console.Write(HandleError(initResponse));
                return;
            }

            string readerName = string.Empty;
            int getReaderNameResponse = reg.GetReaderName(0, out readerName);
            if (getReaderNameResponse != 0)
            {
                Console.Write(HandleError(getReaderNameResponse));
                return;
            }

            int getSelectReaderResponse = reg.SelectReader(readerName);
            if (getSelectReaderResponse != 0)
            {
                Console.Write(HandleError(getSelectReaderResponse));
                return;
            }

            int processNewCardResponse = reg.ProcessNewCard();
            if (processNewCardResponse != 0)
            {
                Console.Write(HandleError(processNewCardResponse));
                return;
            }

            _DOCUMENT_DATA docData;
            int docDataResponse = reg.ReadDocumentData(out docData);
            if (docDataResponse != 0)
            {
                Console.Write(HandleError(docDataResponse));
                return;
            }

            _VEHICLE_DATA vehicleData;
            int vehicleDataResponse = reg.ReadVehicleData(out vehicleData);
            if (vehicleDataResponse != 0)
            {
                Console.Write(HandleError(vehicleDataResponse));
                return;
            }

            _PERSONAL_DATA personalData;
            int personalDataResponse = reg.ReadPersonalData(out personalData);
            if (personalDataResponse != 0)
            {
                Console.Write(HandleError(personalDataResponse));
                return;
            }

            int finalizeResponse = reg.Finalize();
            if (finalizeResponse != 0)
            {
                Console.Write(HandleError(finalizeResponse));
                return;
            }

            RegLicenseData resultData = new RegLicenseData();
            resultData.DocumentData = docData;
            resultData.VehicleData = vehicleData;
            resultData.PersonalData = personalData;

            ProgramResponse programResponse = new ProgramResponse()
            {
                IsError = false,
                ErrorMessage = string.Empty,
                Result = resultData
            };

            Console.Write(JsonConvert.SerializeObject(programResponse));
            return;
        }

        static string HandleError(int errorCode)
        {
            string errorMessage = string.Empty;

            ProgramResponse programResponse = new ProgramResponse();

            if ((errorMessage = GetErrorMessage((uint)errorCode)) != string.Empty)
            {
                programResponse.IsError = true;
                programResponse.ErrorMessage = errorMessage;

                return JsonConvert.SerializeObject(programResponse);
            }

            return string.Empty;
        }

        static string GetErrorMessage(uint responseCode)
        {
            switch(responseCode)
            {
                case 0:
                    return string.Empty;

                case 11:
                    return "11";

                case 12:
                    return "12";

                case 13:
                    return "13";

                case 87:
                    return "87";

                case 1056:
                    return "1056";

                case 1062:
                    return "1062";

                case 0x80004003:
                    return "0x80004003";

                case 0x80100008:
                    return "0x80100008";

                case 0x80100009:
                    return "0x80100009";

                case 0x8010000C:
                    return "0x8010000C";

                case 0x80100011:
                    return "0x80100011";

                case 0x80100017:
                    return "0x80100017";

                case 0x8010001C:
                    return "0x8010001C";

                case 0x8010002E:
                    return "0x8010002E";

                default:
                    return string.Empty;
            }
        }
    }
}
