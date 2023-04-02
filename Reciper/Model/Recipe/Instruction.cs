using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Reciper.Model.Recipe;

public class Instruction
{
    [Key]
    public int InstructionId { get; set; }

    [Required]
    public string Step { get; set; }

    public int RecipeId { get; set; }
    [JsonIgnore]
    public virtual Recipe Recipe { get; set; }
}