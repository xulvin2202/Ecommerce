using Model.EF;
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
        public IEnumerable<QandA> ListAllQandA()
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
    }
}
