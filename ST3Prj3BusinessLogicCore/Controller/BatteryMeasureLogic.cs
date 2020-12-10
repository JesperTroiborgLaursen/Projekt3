using System;
using System.Collections.Concurrent;
using System.Threading;
using Domain.DTOModels;

namespace BusinessLogic.Controller
{
    public class BatteryMeasureLogic
    {

        public BlockingCollection<ADC_DTO> _dataQueueAdc { get; set; }
        public BlockingCollection<Battery_DTO> _dataQueueBattery { get; set; }
        private bool stop = false;

        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        public BatteryMeasureLogic(BlockingCollection<ADC_DTO> dataQueueAdc, BlockingCollection<Battery_DTO> dataQueueBattery)
        {
            _dataQueueAdc = dataQueueAdc;
            _dataQueueBattery = dataQueueBattery;
        }

        public void Run()
        {
            while (!Stop)
            {

                try
                {
                    var adcDTO = _dataQueueAdc.Take();
                    var DTO = new Battery_DTO();

                    CalculateCurrent(adcDTO, DTO);

                    CalculateVoltageInPercent(adcDTO,DTO);

                    _dataQueueBattery.Add(DTO);

                    Thread.Sleep(0);
                }
                catch (InvalidOperationException)
                {
                    continue;
                }
                Thread.Sleep(0);
            }
        }

        private void CalculateVoltageInPercent(ADC_DTO adcDto, Battery_DTO dto)
        {
            var min = 8;
            var maxDiff = 11.2-min;
            float adcVoltage = (adcDto.voltage*(float)4.096)/2048;
            //adcVoltage = adcVoltage * (float)4.096;
            adcVoltage = adcVoltage * (float)4.70;
            var voltageDiff = adcVoltage - min;
            var voltageLeftInPercent = (voltageDiff/maxDiff) * 100;
            dto.VoltageLeftInPercent = voltageLeftInPercent;
        }

        void CalculateCurrent(ADC_DTO adcDto, Battery_DTO dto)
        {
            var voltage = (adcDto.voltageOverResistor/2048) * 4.096;
            var current = voltage * 0.005;

            dto.Current = current;
        }

        
    }
}