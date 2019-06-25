using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class AboutDao
    {

        EcommerceDbContext db = null;
        public AboutDao()
        {
            db = new EcommerceDbContext();
        }

        public long Insert(About entity)
        {
            db.Abouts.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public List<About> LstAbout(long id)
        {
            return db.Abouts.ToList();
        }
        public bool Update(About entity)
        {
            try
            {
                var content = db.Contents.Find(entity.ID);               
                content.Detail = entity.Detail;              
                content.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }
        public About GetByID(long id)
        {
            return db.Abouts.Find(id);
        }
        public About ViewDetail(long id)
        {
            return db.Abouts.Find(id);
        }

    }
}
