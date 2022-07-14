using System.ComponentModel.DataAnnotations;

namespace ProjectOMatic.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public SkillLevel? SkillLevel { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? ProjectBrief { get; set; }

        public bool HasSolution { get; set; }
    }
}
