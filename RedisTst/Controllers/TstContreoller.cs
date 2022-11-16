using Microsoft.AspNetCore.Mvc;
using RedisTst.Data;
using RedisTst.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RedisTst.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class TstContreoller : ControllerBase {

        private readonly DataContext dataContext;

        public TstContreoller(DataContext dataContext) { 
            this.dataContext = dataContext;
        }
        // GET: api/<TstContreoller>
        [HttpGet]
        public IEnumerable<TstModel> Get() {
            return dataContext.TstModels.ToList();
        }

        // POST api/<TstContreoller>
        [HttpPost]
        public void Post(string value) {
            dataContext.TstModels.Add(new TstModel { Value = value});
            dataContext.SaveChanges();
        }
    }
}
