using System.ComponentModel.DataAnnotations;
using Reciper.Model.User;

namespace Reciper.Model.Recipe;

public class Recipe
{
    [Key]
    public int RecipeId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required] public string Description { get; set; }
    [Required] public string ImgUrl { get; set; }
    public int UserId { get; set; }
    public virtual ICollection<Ingredient> Ingredients { get; set; }
    public virtual ICollection<Instruction> Instructions { get; set; }
}