using Markdig;
using Markdig.Syntax;
using ProjectOMatic.Data;
using ProjectOMatic.Models;
using System.Text.Json;

namespace ProjectOMatic.Services
{
    public class MarkdownService
    {
        public void LoadSolutionMarkdownDataToDB(string webRootPath)
        {
            string path = Path.Combine(webRootPath, "solution-files");

            var solutionListFile = new DirectoryInfo(path).GetFiles().FirstOrDefault();

            var jsonContent = File.ReadAllText(solutionListFile.FullName);

            if (!string.IsNullOrEmpty(jsonContent))
            {
                var solutionList = JsonSerializer.Deserialize<List<Solution>>(jsonContent);

                var solutionCount = GetSolutionCountFromDB();

                using ProjectDbContext context = new ProjectDbContext();

                if (solutionList.Count() > solutionCount)
                {
                    for(int i = solutionCount; i < solutionList.Count(); i++)
                    {
                        var project = context.Projects.Where(p => p.Title.ToLower() == solutionList[i].ProjectName.ToLower()).FirstOrDefault();
                        var language = context.Languages.Where(l => l.Name.ToLower() == solutionList[i].LanguageName.ToLower()).FirstOrDefault();
                        var framework = context.Frameworks.Where(l => l.Name.ToLower() == solutionList[i].FrameworkName.ToLower()).FirstOrDefault();

                        Solution solution = new Solution
                        {
                            Slug = solutionList[i].Slug,
                            HostName = solutionList[i].HostName,
                            Project = project,
                            Language = language,
                            Framework = framework,
                        };

                        context.Solutions.Add(solution);

                        context.SaveChanges();
                    }
                }
            }
        }

        private int GetSolutionCountFromDB()
        {
            using ProjectDbContext context = new ProjectDbContext();

            return context.Solutions.Count();
        }

        public void LoadProjectMarkdownDataToDB(string webRootPath)
        {
            string path = Path.Combine(webRootPath, "project-files");

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
