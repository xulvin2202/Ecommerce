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

        public IEnumerable<About> ListAllAbout()
        {
            IQueryable<About> model = db.Abouts;

            return model.ToList();
        }
        public bool Update(About entity)
        {
            try
            {
                var about = db.Abouts.Find(entity.ID);
            
                about.Detail = entity.Detail;
             
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
        
    }
}
