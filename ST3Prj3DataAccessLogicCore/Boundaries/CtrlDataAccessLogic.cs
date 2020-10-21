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
    public class CtrlDataAccessLogic : iDataAccessLogic
    {
        /// <summary>
        /// Indtil videre uden Dependcy Injection
        /// </summary>
        public CtrlDataAccessLogic()
        {
        }
        /// <summary>
        /// "Dataopsamlingen" i getSomeData er KUN et eksempel,
        /// hvor en brugerindtastning udgør dataopsamlingen.
        /// </summary>
        /// <returns></returns>
        public int getSomeData()
        {
            int numVal = -1;
            bool repeat = true;

            while (repeat)
            {
                Console.WriteLine("Enter a number between −2,147,483,648 and +2,147,483,647 (inclusive).");

                string input = Console.ReadLine();

                // ToInt32 can throw FormatException or OverflowException.
                try
                {
                    numVal = Convert.ToInt32(input);
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Input string is not a sequence of digits.");
                }
                catch (OverflowException e)
                {
                    Console.WriteLine("The number cannot fit in an Int32.");
                }
                finally
                {
                    if (numVal < Int32.MaxValue)
                    {
                        Console.WriteLine("The new value is {0}", numVal + 1);
                    }
                    else
                    {
                        Console.WriteLine("numVal cannot be incremented beyond its current value");
                    }
                }
                Console.WriteLine("Go again? Y/N");
                string go = Console.ReadLine();
                if (go == "Y" || go == "y")
                {
                    repeat = true;
                }
                else
                {
                    repeat = false;
                }
            }
            return numVal;
        }
        /// <summary>
        /// En metode som skal eksemplificere at data gemmes i en database eller tilsvarende
        /// </summary>
        /// <param name="val"></param>
        public void saveSomeData(int val)
        {
            Console.WriteLine("DAL saves this value: " + val);
        }
    }

    
    public class CtrlDataAccessLogicWPF : iDataAccessLogic
    {
        /// <summary>
        /// Indtil videre uden Dependcy Injection
        /// Denne udgave er uden brug af Console Window
        /// </summary>
        public CtrlDataAccessLogicWPF()
        {
        }
        /// <summary>
        /// "Dataopsamlingen" i getSomeData er KUN et eksempel,
        /// hvor en brugerindtastning udgør dataopsamlingen.
        /// </summary>
        /// <returns></returns>
        public int getSomeData()
        {
            
            return 342;//Returns a value and only "that value"
        }
        /// <summary>
        /// En metode som skal eksemplificere at data gemmes i en database eller tilsvarende
        /// </summary>
        /// <param name="val"></param>
        public void saveSomeData(int val)
        {
            System.Diagnostics.Debug.WriteLine("DAL saves this value: " + val);
        }
    }
}
