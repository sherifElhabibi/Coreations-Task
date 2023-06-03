using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProductService : EntityBaseRepository<Product>,IProductService
    {
        public ProductService(Context context) : base(context)
        {
        }
    }
}
