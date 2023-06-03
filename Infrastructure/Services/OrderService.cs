using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : EntityBaseRepository<Order>, IOrderService
    {
        public OrderService(Context context) : base(context)
        {
        }
    }
}
