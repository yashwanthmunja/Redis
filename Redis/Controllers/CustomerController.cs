using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redis.Data;
using Redis.IRepository;
using Redis.Models;


namespace Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly StudentdbContext _studentdbContext;
        private readonly ICacheService _cacheService;

        public CustomerController()
        {
            _studentdbContext = new StudentdbContext();
            _cacheService = new CacheService();
        }

        [HttpGet("Customer")]
        public IEnumerable<Customer> Getcustomer()
        {
            var cacheData = _cacheService.GetData<IEnumerable<Customer>>("Customer");
            if (cacheData == null)
            {
                var expirationTime = DateTimeOffset.Now.AddMinutes(60.0);
                cacheData = _studentdbContext.Customers.ToList();
                _cacheService.SetData<IEnumerable<Customer>>("Customer", cacheData, expirationTime);
                return cacheData;

            }
            else
            {
                return cacheData;
            }
        }

        [HttpGet("oneCustomer")]
        public Customer Getone(int id)
        {
            Customer filteredData;
            var cacheData = _cacheService.GetData<IEnumerable<Customer>>("Customer");
            if (cacheData != null)
            {
                filteredData = cacheData.Where(x => x.Customerid == id).FirstOrDefault();
                return filteredData;
            }
            filteredData = _studentdbContext.Customers.Where(x => x.Customerid == id).FirstOrDefault();
            return filteredData;
        }

        [HttpPost("addcustomer")]
        public async Task<Customer> Post([FromBody] Customer customer)
        {
            var obj = await _studentdbContext.Customers.AddAsync(customer);
            _cacheService.RemoveData("customer");
            _studentdbContext.SaveChanges();
            return obj.Entity;
        }
        [HttpPut("updateproduct")]
        public IActionResult Put(Customer customer)
        {
            _studentdbContext.Customers.Update(customer);
            _cacheService.RemoveData("customer");
            _studentdbContext.SaveChanges();
            //update the
            var expirationTime = DateTimeOffset.Now.AddMinutes(60.0);
           var  cacheData = _studentdbContext.Customers.ToList();
            _cacheService.SetData<IEnumerable<Customer>>("Customer", cacheData, expirationTime);
            return Ok(cacheData);
          
        }
        [HttpDelete("deleteproduct")]
        public IActionResult Delete(int Id)
        {
            var filteredData = _studentdbContext.Customers.Where(x => x.Customerid == Id).FirstOrDefault();
            _studentdbContext.Remove(filteredData);
            _cacheService.RemoveData("Customer");
            _studentdbContext.SaveChanges();
            return Ok(filteredData);
        }
    }
}
  
   
    
