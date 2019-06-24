using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class CategoryDao
    {
        EcommerceDbContext db = null;
        public CategoryDao()
        {
            db = new EcommerceDbContext();
        }
        public IEnumerable<Category> ListAllCategory()
        {
            IQueryable<Category> model = db.Categories;

            return model.OrderBy(x => x.CreateDate).ToList();
           
        }
        public IEnumerable<Category> ListCategory()
        {
            IQueryable<Category> model = db.Categories;

            return model.Where(x => x.ParentID == null).OrderBy(x => x.CreateDate).ToList();

        }
        public IEnumerable<Category> ListSubCategory()
        {
            IQueryable<Category> model = db.Categories;

            return model.Where(x => x.ParentID != null).OrderByDescending(x => x.CreateDate).ToList();

        }

        public long Insert(ContentCategory entity)
        {
            db.ContentCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public Category ViewDetail(long id)
        {
            return db.Categories.Find(id);
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
