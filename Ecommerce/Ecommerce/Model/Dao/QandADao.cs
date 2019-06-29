using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class QandADao
    {
        EcommerceDbContext db = null;
        public QandADao()
        {
            db = new EcommerceDbContext();
        }
       
        public IEnumerable<QandA> ListQ(int page, int pageSize)
        {
            IQueryable<QandA> model = db.QandAs;
            return model.OrderBy(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        public IEnumerable<QandA> ListQA()
        {
            IQueryable<QandA> model = db.QandAs;
            return model.OrderBy(x => x.CreateDate).ToList();
        }
        public long Insert(QandA qandA)
        {
            db.QandAs.Add(qandA);
            db.SaveChanges();
            return qandA.ID;
        }
        public bool Update(QandA entity)
        {
            try
            {
                var qandA = db.QandAs.Find(entity.ID);
                qandA.Question = entity.Question;
                qandA.Answer = entity.Answer;
                qandA.ModifiedBy = entity.ModifiedBy;
                qandA.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }
        public QandA GetByID(long id)
        {
            return db.QandAs.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var qa = db.QandAs.Find(id);
                db.QandAs.Remove(qa);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
