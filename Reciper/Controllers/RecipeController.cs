using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reciper.Data;
using Reciper.Model.Recipe;
using Reciper.Model.User;

namespace Reciper.Controllers;
[Route("api/[controller]")]
[ApiController]

public class RecipeController : Controller
{
    private readonly DataContext _context;

    public RecipeController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<Recipe>>> GetAllRecipes()
    {
        return Ok(await _context.Recipes.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Recipe>> GetRecipeById(int id)
    {
        var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == id);
        if (recipe != null)
        {
            return Ok(recipe);
        }

        return BadRequest("recipe not found");
    }

    [HttpPost("create")]
    public async Task<ActionResult<Recipe>> CreateRecipe(RecipeDto request)
    {
        Recipe recipe = new Recipe();
        recipe.Name = request.name;
        recipe.Description = request.description;
        recipe.ImgUrl = request.imgUrl;
        recipe.UserId = request.userId;
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        return Ok(recipe);
    }

}