namespace Domain.Models
{
    public class Sample
    {

        private short _value;

        public short Value
        {
            get => _value;
            set => _value = value;
        }

        public int ID { get; set; }
        public int SamplePackID { get; set; }

    }
}