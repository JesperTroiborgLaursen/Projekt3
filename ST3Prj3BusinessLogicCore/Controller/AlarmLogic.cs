namespace BusinessLogic.Controller
{
    public class AlarmLogic
    {
        private bool stop=false;

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        public AlarmLogic()
        {
            //Skal have AnalyzeLogicqueue
        }

        public void Run()
        {
            while (!Stop)
            {
                //Take fra en kø med gnms. blodtryk
                    //Hvis den er meget for høj/lav ConcreteSubject.NotifyBP(1)
                    //Hvis den er mellem for høj/lav ConcreteSubject.NotifyBP(2)
                    //Hvis den er lidt for høj/lav ConcreteSubject.NotifyBP(3)

                //Take fra en kø med batterispænding
                    //Hvis den er meget for høj/lav ConcreteSubject.NotifyBattery(1)
                    //Hvis den er mellem for høj/lav ConcreteSubject.NotifyBattery(2)
                    //Hvis den er lidt for høj/lav ConcreteSubject.NotifyBatery(3)

                //Take fra en kø med Puls
                    //Hvis den er meget for høj/lav ConcreteSubject.NotifyBattery(1)
                    //Hvis den er mellem for høj/lav ConcreteSubject.NotifyBattery(2)
                    //Hvis den er lidt for høj/lav ConcreteSubject.NotifyBatery(3)
            }
        }

    }
}