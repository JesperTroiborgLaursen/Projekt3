using System;
using System.Collections.Concurrent;
using System.Threading;
using Domain.Context;
using Domain.DTOModels;
using Domain.Models;

namespace DataAccesLogic.Boundaries
{
    public class LocalDB
    {
        public BlockingCollection<LocalDB_DTO> _dataQueueLocalDb;
        private bool stop = false;

        //Cant delete this ctor, though it is never used
             //Getting:
                //System.MissingMethodException: Method not found
        public LocalDB(BlockingCollection<LocalDB_DTO> dataQueueLocalDb, ManualResetEvent manualResetEvent)
        {
            
        }

        public LocalDB(BlockingCollection<LocalDB_DTO> dataQueueLocalDb)
        {
            _dataQueueLocalDb = dataQueueLocalDb;
        }


        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        public void Run()
        {
            while (!Stop)
            {
                var db = new SamplePackDBContext();
                using (db)
                {
                    while (!_dataQueueLocalDb.IsCompleted)
                    {
                        try
                        {
                            var container = _dataQueueLocalDb.Take();
                            SamplePack samplePack = container.SamplePack;

                            db.Add<SamplePack>(samplePack);
                            db.SaveChanges();

                        }
                        catch (InvalidOperationException)
                        {
                            continue;
                        }

                        Thread.Sleep(1000);
                    }
                }
            }
        }




        //Implemtation of IDatabase to be able to work further with the data collected in the local db and make it easy to impl. HL7 FHIR or other db's.
        //public void SaveSamplePack(SamplePack samplePack)
        //{
        //    using (var db = new SamplePackDBContext())
        //    {
        //        db.SamplePacks.Add(samplePack);
        //        db.SaveChanges();
        //    }
            
        //}

        //public void DeleteSamplePack(int samplePackID)
        //{
        //    var db = new SamplePackDBContext();
        //    var samplePack = GetSamplePack(samplePackID);
        //    db.Attach(samplePack);
        //    db.Remove(samplePack);
        //    db.SaveChanges();
        //}

        //public SamplePack GetSamplePack(int samplePackID)
        //{
        //    var db = new SamplePackDBContext();
        //    SamplePack samplePack = db.SamplePacks.Find(samplePackID);
        //    return samplePack;
        //}

        //public List<SamplePack> GetAllSamplePacks()
        //{
        //    var db = new SamplePackDBContext();
        //    List<SamplePack> result = new List<SamplePack>(db.SamplePacks.ToList());
        //    return result;
        //}
    }
}