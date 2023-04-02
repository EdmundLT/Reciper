using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public virtual Recipe Recipe { get; set; } 
}