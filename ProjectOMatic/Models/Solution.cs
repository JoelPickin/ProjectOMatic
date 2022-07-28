using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectOMatic.Models
{
    public class Solution
    {
        public int Id { get; set; }

        [Required]
        public string Slug { get; set; }
        [Required]
        public string HostName { get; set; }
        [Required]
        public Project? Project { get; set; }
        [NotMapped]
        public string ProjectName { get; set; }
        [Required]
        public Language? Language { get; set; }
        [NotMapped]
        public string LanguageName { get; set; }
        [Required]
        public Framework? Framework { get; set; }
        [NotMapped]
        public string FrameworkName { get; set; }
        [NotMapped]
        public string SolutionContent { get; set; }
    }
}
