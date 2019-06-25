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
        public long Insert(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return category.ID;
        }
        public Category GetByID(long id)
        {
            return db.Categories.Find(id);
        }
        public Category ViewDetail(long id)
        {
            return db.Categories.Find(id);
        }
        public bool Update(Category entity)
        {
            try
            {
                var category = db.Categories.Find(entity.ID);
                category.Name = entity.Name;
                category.MetaTitle = entity.MetaTitle;
                category.Image = entity.Image;
                category.CreateDate = DateTime.Now;
                category.ParentID = entity.ParentID;
                category.CreateDate = entity.CreateDate;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }
        
        public bool Delete(int id)
        {
            try
            {
                var category = db.Categories.Find(id);
                db.Categories.Remove(category);
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
