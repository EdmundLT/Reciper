using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reciper.Authentication;
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
        return Ok(await _context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Recipe>> GetRecipeById(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .FirstOrDefaultAsync(r => r.RecipeId == id);
        if (recipe != null)
        {
            return Ok(recipe);
        }

        return BadRequest("recipe not found");
    }

    [HttpPost("create")]
    [ServiceFilter(typeof(JwtAuthFilter))]
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

    [HttpPost("{recipeId}/ingredients")]
    [ServiceFilter(typeof(JwtAuthFilter))]
    public async Task<ActionResult<Recipe>> AddIngredients(int recipeId, [FromBody] List<IngredientCreateDto> request)
    {
        var recipe = await _context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.RecipeId == recipeId);

        if (recipe == null) return BadRequest("Recipe not found");
        foreach (IngredientCreateDto ingredientCreateDto in request)
        {
        var ingredient = new Ingredient
            {
                Name = ingredientCreateDto.Name,
                Amount = ingredientCreateDto.Quantity,
                Recipe = recipe,
                RecipeId = recipeId
            };    
            recipe.Ingredients.Add(ingredient);
        }
            await _context.SaveChangesAsync();
        return Ok(recipe);
    }
    [HttpPost("{recipeId}/instructions")]
    [ServiceFilter(typeof(JwtAuthFilter))]
    public async Task<ActionResult<Recipe>> AddInstructions(int recipeId, [FromBody] List<InstructionCreateDto> instructions)
    {
        var recipe = await _context.Recipes.Include(r => r.Instructions).FirstOrDefaultAsync(r => r.RecipeId == recipeId);
        if (recipe == null) return BadRequest("Recipe not found");

        foreach (var instruction in instructions)
        {
            var newInstruction = new Instruction
            {
                Step = instruction.Description,
                Recipe = recipe,
                RecipeId = recipeId
            };
            recipe.Instructions.Add(newInstruction);
        }
    
        await _context.SaveChangesAsync();
        return Ok(recipe);
    }

    
    

}