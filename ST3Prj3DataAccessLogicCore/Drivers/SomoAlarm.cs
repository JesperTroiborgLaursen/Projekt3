using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using Interfaces;

namespace DataAccesLogic.Drivers
{
    public class SomoAlarm : IAlarmObserver
    {
        public SerialPort serialPort;
        private readonly byte[] volume;
        private readonly byte[] sdCard;
        private readonly byte[] track1;
        private readonly byte[] track2;
        private readonly byte[] track3;
        private readonly byte[] track4;
        private readonly byte[] repeat;
        private readonly byte[] stop;
        private readonly int numberOfBytes = 1;
        private readonly int start = 0;
        private readonly int cmd = 1;
        private readonly int feedback = 2;
        private readonly int para1 = 3;
        private readonly int para2 = 4;
        private readonly int checksum1 = 5;
        private readonly int checksum2 = 6;
        private readonly int end = 7;
        public SomoAlarm()
        {
            volume = new byte[] { 126, 6, 0, 0, 30, 255, 220, 239 };
            sdCard = new byte[] { 126, 9, 0, 0, 2, 255, 245, 239 };
            track1 = new byte[] { 126, 15, 0, 1, 1, 255, 239, 239 };
            track2 = new byte[] { 126, 15, 0, 1, 2, 255, 238, 239 };
            track3 = new byte[] { 126, 15, 0, 1, 3, 255, 237, 239 };
            track4 = new byte[] { 126, 15, 0, 1, 4, 255, 236, 239 };
            repeat = new byte[] { 126, 25, 0, 0, 0, 255, 231, 239 };
            stop = new byte[] { 126, 22, 0, 0, 0, 255, 234, 239};
            serialPort = new SerialPort("/dev/ttyAMA0", 9600, Parity.None, 8, StopBits.One);
            serialPort.Encoding = new UTF8Encoding();
        }
        public void Setup()
        {
            serialPort.Open();
            Thread.Sleep(50);
            serialPort.Write(volume, start, numberOfBytes);
            serialPort.Write(volume, cmd, numberOfBytes);
            serialPort.Write(volume, feedback, numberOfBytes);
            serialPort.Write(volume, para1, numberOfBytes);
            serialPort.Write(volume, para2, numberOfBytes);
            serialPort.Write(volume, checksum1, numberOfBytes);
            serialPort.Write(volume, checksum2, numberOfBytes);
            serialPort.Write(volume, end, numberOfBytes);
            Thread.Sleep(50);
            serialPort.Write(sdCard, start, numberOfBytes);
            serialPort.Write(sdCard, cmd, numberOfBytes);
            serialPort.Write(sdCard, feedback, numberOfBytes);
            serialPort.Write(sdCard, para1, numberOfBytes);
            serialPort.Write(sdCard, para2, numberOfBytes);
            serialPort.Write(sdCard, checksum1, numberOfBytes);
            serialPort.Write(sdCard, checksum2, numberOfBytes);
            serialPort.Write(sdCard, end, numberOfBytes);
            Thread.Sleep(50);
            serialPort.Close();
        }

        public void PlayAlarmSound(int priority)
        {
            if (priority == 1)
            {
                serialPort.Open();
                Thread.Sleep(50);
                serialPort.Write(track1, start, numberOfBytes);
                serialPort.Write(track1, cmd, numberOfBytes);
                serialPort.Write(track1, feedback, numberOfBytes);
                serialPort.Write(track1, para1, numberOfBytes);
                serialPort.Write(track1, para2, numberOfBytes);
                serialPort.Write(track1, checksum1, numberOfBytes);
                serialPort.Write(track1, checksum2, numberOfBytes);
                serialPort.Write(track1, end, numberOfBytes);
                Thread.Sleep(50);
                Repeat();
                Thread.Sleep(50);
                serialPort.Close();
            }
            else if (priority == 2)
            {
                serialPort.Open();
                Thread.Sleep(50);
                serialPort.Write(track2, start, numberOfBytes);
                serialPort.Write(track2, cmd, numberOfBytes);
                serialPort.Write(track2, feedback, numberOfBytes);
                serialPort.Write(track2, para1, numberOfBytes);
                serialPort.Write(track2, para2, numberOfBytes);
                serialPort.Write(track2, checksum1, numberOfBytes);
                serialPort.Write(track2, checksum2, numberOfBytes);
                serialPort.Write(track2, end, numberOfBytes);
                Thread.Sleep(50);
                Repeat();
                Thread.Sleep(50);
                serialPort.Close();
            }
            else if (priority == 3)
            {
                serialPort.Open();
                Thread.Sleep(50);
                serialPort.Write(track3, start, numberOfBytes);
                serialPort.Write(track3, cmd, numberOfBytes);
                serialPort.Write(track3, feedback, numberOfBytes);
                serialPort.Write(track3, para1, numberOfBytes);
                serialPort.Write(track3, para2, numberOfBytes);
                serialPort.Write(track3, checksum1, numberOfBytes);
                serialPort.Write(track3, checksum2, numberOfBytes);
                serialPort.Write(track3, end, numberOfBytes);
                Thread.Sleep(50);
                Repeat();
                Thread.Sleep(50);
                serialPort.Close();
            }
            else if (priority == 4)
            {
                serialPort.Open();
                Thread.Sleep(50);
                serialPort.Write(track4, start, numberOfBytes);
                serialPort.Write(track4, cmd, numberOfBytes);
                serialPort.Write(track4, feedback, numberOfBytes);
                serialPort.Write(track4, para1, numberOfBytes);
                serialPort.Write(track4, para2, numberOfBytes);
                serialPort.Write(track4, checksum1, numberOfBytes);
                serialPort.Write(track4, checksum2, numberOfBytes);
                serialPort.Write(track4, end, numberOfBytes);
                Thread.Sleep(50);
                Repeat();
                Thread.Sleep(50);
                serialPort.Close();
            }
        }

        public void StopAlarmSound()
        {
            serialPort.Open();
            Thread.Sleep(50);
            serialPort.Write(stop, start, numberOfBytes);
            serialPort.Write(stop, cmd, numberOfBytes);
            serialPort.Write(stop, feedback, numberOfBytes);
            serialPort.Write(stop, para1, numberOfBytes);
            serialPort.Write(stop, para2, numberOfBytes);
            serialPort.Write(stop, checksum1, numberOfBytes);
            serialPort.Write(stop, checksum2, numberOfBytes);
            serialPort.Write(stop, end, numberOfBytes);
            Thread.Sleep(50);
            serialPort.Close();
        }
        private void Repeat()
        {
            serialPort.Write(repeat, start, numberOfBytes);
            serialPort.Write(repeat, cmd, numberOfBytes);
            serialPort.Write(repeat, feedback, numberOfBytes);
            serialPort.Write(repeat, para1, numberOfBytes);
            serialPort.Write(repeat, para2, numberOfBytes);
            serialPort.Write(repeat, checksum1, numberOfBytes);
            serialPort.Write(repeat, checksum2, numberOfBytes);
            serialPort.Write(repeat, end, numberOfBytes);
        }

        public void ShutDown()
        {
            StopAlarmSound();
        }

        public void UpdateBattery(int priority)
        {
            PlayAlarmSound(priority);
        }

        public void UpdateBP(int priority)
        {
            PlayAlarmSound(priority);
        }

        public void UpdatePulse(int priority)
        {
            PlayAlarmSound(priority);
        }
    }
}