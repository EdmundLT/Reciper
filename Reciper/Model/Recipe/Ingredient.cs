using System.ComponentModel.DataAnnotations;

namespace Reciper.Model.Recipe;

public class Ingredient
{
    [Key]
    public int IngredientId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Amount { get; set; }

    public int RecipeId { get; set; }

    public virtual Recipe Recipe { get; set; } 
}