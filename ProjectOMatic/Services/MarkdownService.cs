using Markdig;
using Markdig.Syntax;
using ProjectOMatic.Data;
using ProjectOMatic.Models;

namespace ProjectOMatic.Services
{
    public class MarkdownService
    {
        public void LoadSolutionMarkdownDataToDB()
        {
            string path = Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), @"ProjectOMatic\wwwroot\solution-files");

            var files = new DirectoryInfo(path).GetFiles().OrderBy(f => f.LastWriteTime).ToList();

            var solutionCount = GetSolutionCountFromDB();

            if (files.Count() > solutionCount)
            {
                for (int i = solutionCount; i < files.Count(); i++)
                {
                    var fileText = File.ReadAllText(files[i].FullName);

                    var markdownDocument = Markdown.Parse(fileText);

                    var docInfo = markdownDocument.Select(b => b as HeadingBlock)
                    .Where(b => b != null)
                    .Select(hb => hb.Inline.FirstChild.ToString()).ToList();

                    var title = docInfo[0];
                    var languageName = docInfo[1];
                    var frameworkName = docInfo[2];

                    using ProjectDbContext context = new ProjectDbContext();

                    var project = context.Projects.Where(p => p.Title.ToLower() == title.ToLower()).FirstOrDefault();
                    var language = context.Languages.Where(l => l.Name.ToLower() == languageName.ToLower()).FirstOrDefault();
                    var framework = context.Frameworks.Where(l => l.Name.ToLower() == frameworkName.ToLower()).FirstOrDefault();

                    Solution solution = new Solution
                    {
                        SolutionContent = fileText,
                        Project = project,
                        Language = language,
                        Framework = framework,
                    };

                    context.Solutions.Add(solution);

                    context.SaveChanges();
                }
            }
        }

        private int GetSolutionCountFromDB()
        {
            using ProjectDbContext context = new ProjectDbContext();

            return context.Solutions.Count();
        }

        public void LoadProjectMarkdownDataToDB()
        {
            string path = Path.Combine(Path.GetDirectoryName(Environment.CurrentDirectory), @"ProjectOMatic\wwwroot\project-files");

            string[] folders = Directory.GetDirectories(path);

            foreach (var folder in folders)
            {
                var files = new DirectoryInfo(folder).GetFiles().OrderBy(f => f.LastWriteTime).ToList();

                var folderName = new DirectoryInfo(folder).Name;

                var projectCount = GetProjectCountFromDB(folderName);

                if (files.Count() > projectCount)
                {
                    for (int i = projectCount; i < files.Count(); i++)
                    {
                        var fileText = File.ReadAllText(files[i].FullName);

                        var markdownDocument = Markdown.Parse(fileText);

                        var title = markdownDocument.Select(b => b as HeadingBlock)
                        .Where(b => b != null)
                        .Select(hb => hb.Inline.FirstChild).First().ToString();

                        using ProjectDbContext context = new ProjectDbContext();

                        Project project = new Project
                        {
                            Title = title,
                            ProjectBrief = fileText,
                            SkillLevel = context.SkillLevels.First(s => s.Name.ToLower() == folderName.ToLower())
                        };

                        context.Projects.Add(project);

                        context.SaveChanges();
                    }
                }
            }
        }

        private int GetProjectCountFromDB(string skillLevel)
        {
            using ProjectDbContext context = new ProjectDbContext();

            return context.Projects.Where(p => p.SkillLevel.Name == skillLevel).Count();
        }
    }
}
