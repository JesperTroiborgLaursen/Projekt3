using System.Collections.Generic;
using System.Linq;
using Domain.Context;
using Domain.Models;
using Interfaces;

namespace DataAccesLogic.Boundaries
{
    public class LocalDB : IDatabase
    {
        private SamplePackDBContext db;
        public void SaveSamplePack(SamplePack samplePack)
        {
            using (db)
            {
                db.SamplePacks.Add(samplePack);
            }
            db.SaveChanges();
        }

        public void DeleteSamplePack(int samplePackID)
        {
            var samplePack = GetSamplePack(samplePackID);
            db.Attach(samplePack);
            db.Remove(samplePack);
            db.SaveChanges();
        }

        public SamplePack GetSamplePack(int samplePackID)
        {
            SamplePack samplePack = db.SamplePacks.Find(samplePackID);
            return samplePack;
        }

        public List<SamplePack> GetAllSamplePacks()
        {
            List<SamplePack> result = new List<SamplePack>(db.SamplePacks.ToList());
            return result;
        }
    }
}