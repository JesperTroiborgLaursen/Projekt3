using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesCore;

/// <summary>
/// Metoder implemeteret som eksempler der overholder iDataAccessLogic intefacet
/// Ændringer signaturer og til tilføjelse af metoder (nye signature) skal ske
/// først skegennem ændringer i iDataAccessLogic interfacet
/// </summary>
namespace DataAccessLogicCore.Boundaries
{
    public class CtrlDataAccessLogic : IDataAccessLogic
    {
        public int GetLastSamplePackID()
        {
            int x = 0;
            return x;
        }
    }
}
