/// <summary>
/// Navngivningen i nedenstående intefaces er alene eksempelgivende
/// Ændringer af signaturer og tilføjelse af metoder (nye signature) skal ske
/// først skegennem ændringer i de givne interfaces
/// En yderliger opdeling af med namespaces under namespaces ST3Prj3InterfacesCore
/// eks "ST3Prj3InterfacesCore.BusinessLogic.SpecialFunctions" kan overvejes.
/// </summary>
namespace InterfacesCore
{
    public interface iDataAccessLogic
    {
        int getSomeData();//Signatur
        void saveSomeData(int val);
    }
    public interface iBusinessLogic
    {
        void doAnAlogrithm();

        int DoAnAlogrithm();
    }

    public interface iPresentationLogic
    {
         void startUpGUI();
    }
}
