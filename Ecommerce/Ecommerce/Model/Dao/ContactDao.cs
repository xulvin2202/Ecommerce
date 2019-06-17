using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Model.Dao
{
    public class ContactDao
    {
        EcommerceDbContext db = null;
        public ContactDao()
        {
            db = new EcommerceDbContext();
        }

        public Contact GetActiveContact()
        {
            return db.Contacts.Single(x => x.Status == true);
        }

        public int InsertReview(Feedback rv)
        {
            db.Feedbacks.Add(rv);
            db.SaveChanges();
            return rv.ID;
        }
    }
}
