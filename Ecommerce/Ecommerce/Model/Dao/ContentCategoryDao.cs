using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Dao;

namespace Model.Dao
{
    public class ContentCategoryDao
    {
        EcommerceDbContext db = null;

        public ContentCategoryDao()
        {
            db = new EcommerceDbContext();
        }
        public long Insert(ContentCategory entity)
        {
            db.ContentCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public IEnumerable<ContentCategory> ListAllContentCategory(string searchString)
        {
            IQueryable<ContentCategory> model = db.ContentCategories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.MetaTitle.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToList();
        }
        public IEnumerable<ContentCategory> ListAllContentCategory()
        {
            IQueryable<ContentCategory> model = db.ContentCategories;

            return model.OrderByDescending(x => x.CreateDate).ToList();
        }
        public bool Delete(int id)
        {
            try
            {
                var contentCategory = db.ContentCategories.Find(id);
                db.ContentCategories.Remove(contentCategory);
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
