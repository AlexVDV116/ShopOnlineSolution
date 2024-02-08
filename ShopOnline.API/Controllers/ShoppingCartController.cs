using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Extensions;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly IProductRepository _productRepository;

    // GET
    public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,
        IProductRepository productRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _productRepository = productRepository;
    }

    [HttpGet]
    [Route("{userId}/GetItems")]
    public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
    {
        try
        {
            var cartItems = await _shoppingCartRepository.GetItems(userId);

            if (cartItems == null)
            {
                return NoContent();
            }
            var products = await _productRepository.GetItems();

            if (products == null)
            {
                throw new Exception("No products exists in the system");
            }
            
            var cartItemDto = cartItems.ConvertToDto(products);

            return Ok(cartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CartItemDto>> GetItem(int id)
    {
        try
        {
            var cartItem = await _shoppingCartRepository.GetItem(id);
            
            if(cartItem == null)
            {
                return NotFound();
            }
            
            var product = await _productRepository.GetItem(cartItem.ProductId);
            
            if (product == null)
            {
                return NotFound();
            }
            
            var cartItemDto = cartItem.ConvertToDto(product);
            return Ok(cartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto)
    {
        try
        {
            var newCartItem = await _shoppingCartRepository.AddItem(cartItemToAddDto);
            
            if (newCartItem == null)
            {
                return NoContent();
            }
            
            var product = await _productRepository.GetItem(newCartItem.ProductId);
            
            if (product == null)
            {
                throw new Exception($"Something went wrong when attempting to retrieve product (productId:({cartItemToAddDto.ProductId})");
            }
            
            var newCartItemDto = newCartItem.ConvertToDto(product);
            
            // It is standard practice for a POST action method to return the location of the resource where the 
            // newly added item can be found, this location will be returned in the header of the response
            // We can use the createdAtAction method to make sure we adhere to this standard practice
            // We are including the id of the newly added resource, and including the newly added object
            return CreatedAtAction(nameof(GetItem), new {id=newCartItemDto.Id}, newCartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
    {
        try
        {
            var cartItem = await _shoppingCartRepository.DeleteItem(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            
            var product = await _productRepository.GetItem(cartItem.ProductId);
            if(product == null)
            {
                return NotFound();
            }
            
            var cartItemDto = cartItem.ConvertToDto(product);
            return Ok(cartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

        }
    }
}