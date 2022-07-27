using Blazorise;
using Microsoft.EntityFrameworkCore;
using ProjectOMatic.Data;
using ProjectOMatic.Helpers;
using ProjectOMatic.Models;
using ProjectOMatic.Services;

namespace ProjectOMatic.Pages
{
    public partial class Index
    {
        private MarkdownService _markdownService = new MarkdownService();
        private int _lastSelectedProjectNum { get; set; }

        public List<Language> Languages { get; set; } = new List<Language>();
        public List<Framework> Frameworks { get; set; } = new List<Framework>();
        public List<SkillLevel> SkillLevels { get; set; } = new List<SkillLevel>();

        public Project? SelectedProject { get; set; } = new Project();
        public Language? SelectedLanguage { get; set; }
        public Framework? SelectedFramework { get; set; }
        public SkillLevel? SelectedSkillLevel { get; set; }
        public Solution? SelectedSolution { get; set; }

        public bool IsSolutionVisible { get; set; }
        public bool IsLoading { get; set; }


        protected override void OnInitialized()
        {
            _markdownService.LoadProjectMarkdownDataToDB();
            _markdownService.LoadSolutionMarkdownDataToDB();

            SkillLevels = LoadSkillLevels();
            Languages = LoadLanguages();

            if (SelectedLanguage != null)
            {
                Frameworks = LoadFrameworks(SelectedLanguage.Id);
            }

            base.OnInitialized();
        }

        //public async void GetSolutions()
        //{
        //    //var client = new GraphQLHttpClient(new GraphQLHttpClientOptions
        //    //{
        //    //    EndPoint = new Uri("https://api.hashnode.com/")
        //    //}, new NewtonsoftJsonSerializer());

        //    //var request = new GraphQLRequest
        //    //{
        //    //    Query = @"{
        //    //            user(username: ""joelpickin"") {
        //    //                publication {
        //    //                    posts(page: 0) {
        //    //                        title
        //    //                        brief
        //    //                        slug
        //    //               }
        //    //           }
        //    //        }
        //    //     }"
        //    //};

        //    //var response = await client.SendQueryAsync<ResponseType>(request);

        //    //var posts = response.Data.User.Publication.Posts;

        //    //TODO - See if Hashnode release an API call where you can get all posts based on a tag.
        //}

        private List<SkillLevel> LoadSkillLevels()
        {
            using ProjectDbContext context = new ProjectDbContext();

            List<SkillLevel> skillLevels = context.SkillLevels.ToList();

            if (skillLevels != null)
            {
                SelectedSkillLevel = skillLevels.FirstOrDefault();

                return skillLevels;
            }

            return new List<SkillLevel>();
        }

        private List<Framework> LoadFrameworks(int languageId)
        {
            using ProjectDbContext context = new ProjectDbContext();

            List<Framework> frameworks = context.Frameworks.Where(f => f.Language.Id == languageId).ToList();

            if (frameworks != null)
            {
                SelectedFramework = frameworks.FirstOrDefault();

                return frameworks;
            }

            return new List<Framework>();
        }

        private List<Language> LoadLanguages()
        {
            using ProjectDbContext context = new ProjectDbContext();

            List<Language> languages = context.Languages.ToList();

            if (languages != null)
            {
                SelectedLanguage = languages.FirstOrDefault();

                return languages;
            }

            return new List<Language>();
        }

        private async Task FetchProject()
        {
            IsSolutionVisible = false;

            if (!IsLoading)
            {
                IsLoading = true;

                if (SelectedSkillLevel != null)
                {
                    using ProjectDbContext context = new ProjectDbContext();

                    var selectedProjects = await context.Projects.Where(p => p.SkillLevel.Name.Contains(SelectedSkillLevel.Name)).ToListAsync();

                    var projectCount = selectedProjects.Count();

                    if (projectCount > 0)
                    {
                        Random rnd = new Random();

                        var randomProjectNum = rnd.Next(0, projectCount);

                        while (randomProjectNum == _lastSelectedProjectNum)
                        {
                            randomProjectNum = rnd.Next(0, projectCount);
                        }

                        SelectedProject = selectedProjects[randomProjectNum];

                        if (SelectedProject != null && SelectedProject.ProjectBrief != null)
                        {
                            SelectedProject.HasSolution = context.Solutions.Any(s => s.Project.Title == SelectedProject.Title);

                            SelectedProject.ProjectBrief = MarkdownHelper.Parse(SelectedProject.ProjectBrief);

                            await ShowProjectModal();

                            _lastSelectedProjectNum = randomProjectNum;
                        }
                    }
                }

                IsLoading = false;
            }
        }

        public void ShowSolution()
        {
            using ProjectDbContext context = new ProjectDbContext();

            var solutions = context.Solutions.Where(s => s.Project.Title == SelectedProject.Title).ToList();

            var solutionCount = solutions.Count();

            if (solutionCount != 0)
            {
                if (solutionCount > 1)
                {
                    Random rnd = new Random();

                    var randomSolutionNum = rnd.Next(0, solutionCount);

                    SelectedSolution = solutions[randomSolutionNum];

                    int start = SelectedSolution.SolutionContent.IndexOf("#");
                    int end = SelectedSolution.SolutionContent.IndexOf("##", start);

                    SelectedSolution.SolutionContent = SelectedSolution.SolutionContent.Remove(start, end);

                    SelectedSolution.SolutionContent = MarkdownHelper.Parse(SelectedSolution.SolutionContent);
                }
                else
                {
                    SelectedSolution = solutions.FirstOrDefault();

                    int start = SelectedSolution.SolutionContent.IndexOf("#");
                    int end = SelectedSolution.SolutionContent.IndexOf("##", start);

                    SelectedSolution.SolutionContent = SelectedSolution.SolutionContent.Remove(start, end);

                    SelectedSolution.SolutionContent = MarkdownHelper.Parse(SelectedSolution.SolutionContent);
                }

                IsSolutionVisible = true;

                HideProjectModal();
                ShowSolutionModal();

            }
            else
            {
                SelectedSolution = new Solution
                {
                    SolutionContent = "Apologies, no solution found. Why not <a href='https://github.com/JoelPickin/Project-o-Matic' target='_blank'>add your own</a>."
                };

                IsSolutionVisible = true;
            }
        }

        public List<Project> LoadProjects()
        {
            using ProjectDbContext context = new ProjectDbContext();

            List<Project> projects = context.Projects.Include(p => p.SkillLevel).AsNoTracking().ToList();

            return projects;
        }


        private Modal projectModal;

        private Task ShowProjectModal()
        {
            return projectModal.Show();
        }

        private Task HideProjectModal()
        {

            return projectModal.Hide();
        }

        private Modal solutionModal;

        private Task ShowSolutionModal()
        {
            return solutionModal.Show();
        }

        private Task HideSolutionModal()
        {
            IsSolutionVisible = false;
            solutionModal.Hide();
            return projectModal.Show();
        }

        public void UpSkillLevelButton()
        {
            var skillLevel = SkillLevels.FirstOrDefault(s => s.Name == SelectedSkillLevel.Name);

            if (skillLevel != null)
            {
                var index = SkillLevels.IndexOf(skillLevel);

                if (index == SkillLevels.Count - 1)
                {
                    SelectedSkillLevel = SkillLevels.FirstOrDefault();
                }
                else
                {
                    SelectedSkillLevel = SkillLevels[index + 1];
                }
            }
        }

        public void DownSkillLevelButton()
        {
            var skillLevel = SkillLevels.FirstOrDefault(s => s.Name == SelectedSkillLevel.Name);

            if (skillLevel != null)
            {
                var index = SkillLevels.IndexOf(skillLevel);

                if (index == 0)
                {
                    SelectedSkillLevel = SkillLevels.LastOrDefault();
                }
                else
                {
                    SelectedSkillLevel = SkillLevels[index - 1];
                }
            }
        }

        public void UpLanguageButton()
        {
            var language = Languages.FirstOrDefault(s => s.Name == SelectedLanguage.Name);

            if (language != null)
            {
                var index = Languages.IndexOf(language);

                if (index == Languages.Count - 1)
                {
                    SelectedLanguage = Languages.FirstOrDefault();
                }
                else
                {
                    SelectedLanguage = Languages[index + 1];
                }

                if (SelectedLanguage != null)
                {
                    Frameworks = LoadFrameworks(SelectedLanguage.Id);
                }
            }
        }

        public void DownLanguageButton()
        {
            var language = Languages.FirstOrDefault(s => s.Name == SelectedLanguage.Name);

            if (language != null)
            {
                var index = Languages.IndexOf(language);

                if (index == 0)
                {
                    SelectedLanguage = Languages.LastOrDefault();
                }
                else
                {
                    SelectedLanguage = Languages[index - 1];
                }

                if (SelectedLanguage != null)
                {
                    Frameworks = LoadFrameworks(SelectedLanguage.Id);
                }
            }
        }

        public void UpFrameworkButton()
        {
            var framework = Frameworks.FirstOrDefault(s => s.Name == SelectedFramework.Name);

            if (framework != null)
            {
                var index = Frameworks.IndexOf(framework);

                if (index == Frameworks.Count - 1)
                {
                    SelectedFramework = Frameworks.FirstOrDefault();
                }
                else
                {
                    SelectedFramework = Frameworks[index + 1];
                }
            }
        }

        public void DownFrameworkButton()
        {
            var framework = Frameworks.FirstOrDefault(s => s.Name == SelectedFramework.Name);

            if (framework != null)
            {
                var index = Frameworks.IndexOf(framework);

                if (index == 0)
                {
                    SelectedFramework = Frameworks.LastOrDefault();
                }
                else
                {
                    SelectedFramework = Frameworks[index - 1];
                }
            }
        }
    }
}
