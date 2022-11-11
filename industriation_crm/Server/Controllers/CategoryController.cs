using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _ICategory;
        public CategoryController(ICategory ICategory)
        {
            _ICategory = ICategory;
        }
        [HttpGet]
        public async Task<List<category>> Get()
        {
            return await Task.FromResult(_ICategory.GetCategoryDetails());
        }
    }
}
