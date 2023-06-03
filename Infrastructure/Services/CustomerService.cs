using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CustomerService : EntityBaseRepository<Customer>,ICustomerService
    {
        public CustomerService(Context context) : base(context)
        {
        }
    }
}
