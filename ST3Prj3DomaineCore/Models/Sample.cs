namespace Domain.Models
{
    public class Sample
    {

        private ushort _value;

        public ushort Value
        {
            get => _value;
            set => _value = value;
        }

        public int ID { get; set; }
        public int SamplePackID { get; set; }

    }
}