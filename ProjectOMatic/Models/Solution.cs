using System.ComponentModel.DataAnnotations;

namespace ProjectOMatic.Models
{
    public class Solution
    {
        public int Id { get; set; }

        [Required]
        public string? SolutionContent { get; set; }
        [Required]
        public Project? Project { get; set; }
        [Required]
        public Language? Language { get; set; }
        [Required]
        public Framework? Framework { get; set; }
    }
}
