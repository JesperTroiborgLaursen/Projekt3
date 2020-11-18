using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Domain.Context;
using Domain.DTOModels;
using Domain.Models;
using Interfaces;

namespace DataAccesLogic.Boundaries
{
    public class LocalDB : IDatabase
    {
        private BlockingCollection<LocalDB_DTO> _dataQueueLocalDb;
        public ManualResetEvent _calibrationEvent { get; set; }
        //public SamplePackDBContext Context { get; set; }

        public LocalDB(BlockingCollection<LocalDB_DTO> dataQueueLocalDb, ManualResetEvent autoResetEvent)
        {
            _dataQueueLocalDb = dataQueueLocalDb;
            _calibrationEvent = autoResetEvent;
        }

        public void SaveSamplePack(SamplePack samplePack)
        {
            using (var db = new SamplePackDBContext())
            {
                db.SamplePacks.Add(samplePack);
                db.SaveChanges();
            }
            
        }

        public void DeleteSamplePack(int samplePackID)
        {
            var db = new SamplePackDBContext();
            var samplePack = GetSamplePack(samplePackID);
            db.Attach(samplePack);
            db.Remove(samplePack);
            db.SaveChanges();
        }

        public SamplePack GetSamplePack(int samplePackID)
        {
            var db = new SamplePackDBContext();
            SamplePack samplePack = db.SamplePacks.Find(samplePackID);
            return samplePack;
        }

        public List<SamplePack> GetAllSamplePacks()
        {
            var db = new SamplePackDBContext();
            List<SamplePack> result = new List<SamplePack>(db.SamplePacks.ToList());
            return result;
        }

        public void Run()
        {
            while (_calibrationEvent.WaitOne())
            {
                var db = new SamplePackDBContext();
                using (db)
                {
                    while (!_dataQueueLocalDb.IsCompleted && _calibrationEvent.WaitOne())
                    {
                        try
                        {
                            var container = _dataQueueLocalDb.Take();
                            SamplePack samplePack = container.SamplePack;

                            //using (var db = new SamplePackDBContext())
                            //{
                            db.Add<SamplePack>(samplePack);
                            db.SaveChanges();
                            //}

                        }
                        catch (InvalidOperationException)
                        {
                            continue;
                        }

                        Thread.Sleep(20);
                    }
                }
            }
        }
    }
}