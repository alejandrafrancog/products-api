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

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id){

        var index = Products.FindIndex(p => p.Id == id);

        if(index != -1){
            Products.RemoveAt(index);
        }else{
            return NotFound();
        }
        return NoContent();
    }
    

	[HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, Product actualizado){

        if(actualizado.Id != 0 && actualizado.Id != id){
            Console.Write(actualizado.Id);
            return BadRequest("El Id no coincide");
        }
        
        var index = Products.FindIndex(p => p.Id == id);
        
        if(index == -1){
            return NotFound();
        }

        Products[index] = actualizado with { Id = id };

        return NoContent();
    }
    
	[HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Patch(int id, Product actualizado){
        
        var index = Products.FindIndex(p => p.Id == id);

        if (index == -1){
            return NotFound();
        }
        
        var actual = Products[index];

        Products[index] = actual with
        {
            Name = actualizado.Name ?? actual.Name,
            Description = actualizado.Description ?? actual.Description,
            Price = actualizado.Price != 0 ? actualizado.Price : actual.Price,
            Stock = actualizado.Stock != 0 ? actualizado.Stock : actual.Stock
        };

        return NoContent();
    }

    public sealed record Product(
        int Id,
        string Name,
        string? Description,
        decimal Price,
        int Stock);
    
   
}
