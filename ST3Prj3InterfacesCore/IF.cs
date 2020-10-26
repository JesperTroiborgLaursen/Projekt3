/// <summary>
/// Navngivningen i nedenstående intefaces er alene eksempelgivende
/// Ændringer af signaturer og tilføjelse af metoder (nye signature) skal ske
/// først skegennem ændringer i de givne interfaces
/// En yderliger opdeling af med namespaces under namespaces ST3Prj3InterfacesCore
/// eks "ST3Prj3InterfacesCore.BusinessLogic.SpecialFunctions" kan overvejes.
/// </summary>

using System.Security.Cryptography;
using DomaineCore.Models;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;

namespace InterfacesCore
{
    public interface IDataAccessLogic
    {
        int GetLastSamplePackID();//Signatur
    }

    public interface IDataBroadcastLogic
    {
        void BroadcastSamplePack(SamplePack samplePack);//Signatur
    }
    public interface IBusinessLogic
    {
        void doAnAlogrithm();

        int DoAnAlogrithm();

    }

    public interface IPresentationLogic
    {
         void startUpGUI();
    }
}
