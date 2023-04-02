using System.ComponentModel.DataAnnotations;

namespace Reciper.Model.Recipe;

public class Instruction
{
    [Key]
    public int InstructionId { get; set; }

    [Required]
    public string Step { get; set; }

    public int RecipeId { get; set; }

    public virtual Recipe Recipe { get; set; }
}