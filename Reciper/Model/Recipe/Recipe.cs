using System.ComponentModel.DataAnnotations;

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
    public virtual User.User User { get; set; }
    public virtual ICollection<Ingredient> Ingredients { get; set; }
    public virtual ICollection<Instruction> Instructions { get; set; }
}