using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using System.Collections.Generic;

namespace ProductsApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : Controller
{
    private static readonly List<Product> Products =
        [new(1, "Notebook", "Notebook para desarrollo", 1200.00m, 10),
        new(2, "Teclado mecánico", "Teclado mecánico 123", 200.00m, 5),
        new (3, "Mouse", "Mouse inalámbrico", 25.50m, 30)];
        
    private static int _nextId = 2;

    [HttpGet]
    [ProducesResponseType<IEnumerable<Product>>(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        return Ok(Products);
    }
    
    
	[HttpGet("{id:int}")]
   	[ProducesResponseType<IEnumerable<Product>>(StatusCodes.Status200OK)]
	[ProducesResponseType<IEnumerable<Product>>(StatusCodes.Status404NotFound)]
    public ActionResult<Product> GetById(int id){
	   if(id < 0 || id > Products.Count)
	   {
		   return NotFound();
	   }	
       return Ok(Products[id-1]);
    }

	[HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
	public ActionResult<Product> Create(Product product)
	{
		Products.Add(product);
		return CreatedAtAction(
			nameof(GetById),
			new {id = product.Id},
			product);
	}
	
    
    public sealed record Product(
        int Id,
        string Name,
        string? Description,
        decimal Price,
        int Stock);
    
   
}
