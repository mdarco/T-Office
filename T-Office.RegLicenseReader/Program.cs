using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using eVehicleRegistrationCOM;

namespace RegLicenseReader
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
            if (initResponse != 0 && initResponse != 1056)
            {
                Console.Write(HandleError(initResponse));
                return;
            }

            string readerName = string.Empty;
            int getReaderNameResponse = reg.GetReaderName(0, out readerName);
            if (getReaderNameResponse != 0 && (uint)getReaderNameResponse != 0x80100008)
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
                    // ERROR_BAD_FORMAT
                    return "Interna greška (nisu pronađeni obavezni interni podaci na kartici).";

                case 12:
                    // ERROR_INVALID_ACCESS
                    return "Interna greška (nije pozvana funkcija za obradu nove kartice).";

                case 13:
                    // ERROR_INVALID_DATA
                    return "Neispravni podaci na saobraćajnoj dozvoli.";

                case 87:
                    // ERROR_INVALID_PARAMETER
                    return "Interna greška (nije zadat naziv čitača saobraćajne dozvole).";

                case 1056:
                    // ERROR_SERVICE_ALREADY_RUNNING
                    return "Interno upozorenje (servis za čitanje saobraćajne dozvole je već aktivan).";

                case 1062:
                    // ERROR_SERVICE_NOT_ACTIVE
                    return "Interna greška (servis za čitanje saobraćajne dozvole nije aktivan).";

                case 0x80004003:
                    // E_POINTER
                    return "Podaci iz saobraćajne dozvole nisu preuzeti.";

                case 0x80100008:
                    // SCARD_E_INSUFFICIENT_BUFFER
                    return "Interno upozorenje (premala veličina polja za naziv čitača).";

                case 0x80100009:
                    // SCARD_E_UNKNOWN_READER
                    return "Nepoznat čitač saobraćajne dozvole.";

                case 0x8010000C:
                    // SCARD_E_NO_SMARTCARD
                    return "Saobraćajna dozvola nije ubačena u čitač.";

                case 0x80100011:
                    // SCARD_E_INVALID_VALUE
                    return "SCARD_E_INVALID_VALUE";

                case 0x80100017:
                    // SCARD_E_READER_UNAVAILABLE
                    return "Čitač saobraćajne dozvole nije dostupan.";

                case 0x8010001C:
                    // SCARD_E_CARD_UNSUPPORTED
                    return "Nepoznata kartica u čitaču.";

                case 0x8010002E:
                    // SCARD_E_NO_READERS_AVAILABLE
                    return "Čitač saobraćajne dozvole nije instaliran.";

                default:
                    return string.Empty;
            }
        }
    }
}
