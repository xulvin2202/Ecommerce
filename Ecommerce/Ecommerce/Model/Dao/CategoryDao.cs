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
        public List<Category> ListAllCategory()
        {
            return db.Categories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }
        /// <summary>
        /// get list product by category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<Brand> ListAllBrand()
        {
            return db.Brands.Where(x => x.Status == true).ToList();
        }
        public Category ViewDetail(long id)
        {
            return db.Categories.Find(id);
        }
       
    }
}
