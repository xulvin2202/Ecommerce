using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductDao
    {
        EcommerceDbContext db = null;
        public ProductDao()
        {
            db = new EcommerceDbContext();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public IEnumerable<Product> ListAllPaging(string searchString/*, int page, int pageSize*/)
        {
            IQueryable<Product> model = db.Products.OrderByDescending(x=>x.CreateDate);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) );
            }

            return model.OrderByDescending(x => x.CreateDate);
        }
       
        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }
        
        public List<Product> ListSaleProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.PromotionPrice).Take(top).ToList();
        }
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }
        public List<Product> ListRelatedProducts(long productID)
        {
            var product = db.Products.Find(productID);
            return db.Products.Where(x => x.ID != productID && x.Category_ID == product.Category_ID).ToList();
        }
        public List<Content> ListContent(int id)
        {
            return db.Contents.OrderByDescending(x => x.CreateDate).Take(id).ToList();
        }

        public List<Brand> ListBrand(int brand)
        {
            return db.Brands.OrderBy(x => x.CreateDate).Take(brand).ToList();
        }
        public List<Product> ListByCategoryId(long categoryID)
        {

            return db.Products.Where(x => x.Category_ID == categoryID).ToList();
        }
        public List<ProductViewModel> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.Category_ID == categoryID).Count();
            var model = (from a in db.Products
                         join b in db.Categories
                         on a.Category_ID equals b.ID
                         where a.Category_ID == categoryID
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
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PromotionPrice = x.Price
                         });
            model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }
    }
}
