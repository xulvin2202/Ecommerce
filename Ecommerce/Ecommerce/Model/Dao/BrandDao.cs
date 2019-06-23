using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class BrandDao
    {
        EcommerceDbContext db = null;
        public BrandDao()
        {
            db = new EcommerceDbContext();
        }
        public List<Brand> ListBrand()
        {
            return db.Brands.OrderByDescending(x => x.CreateDate).ToList();
        }
        public List<ProductViewModel> ListByCategoryId(long brandID)
        {

            var model = (from a in db.Products
                         join b in db.Brands
                         on a.Brand_ID equals b.ID
                         where a.Brand_ID == brandID
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreateDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PromotionPrice = a.PromotionPrice
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreateDate = x.CreatedDate,
                             ID = x.ID,
                             Image = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PromotionPrice = x.Price
                         });
            model.OrderByDescending(x => x.CreateDate);
            return model.ToList();
        }
        public List<Brand> ListAllBrand()
        {
            return db.Brands.Where(x => x.Status == true).OrderByDescending(x=>x.CreateDate).ToList();
        }
    }
}