using Blazorise;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
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
            SkillLevels = LoadSkillLevels();
            Languages = LoadLanguages();

            if (SelectedLanguage != null)
            {
                Frameworks = LoadFrameworks(SelectedLanguage.Id);
            }


            base.OnInitialized();
        }

        private List<SkillLevel> LoadSkillLevels()
        {
            _markdownService.LoadProjectMarkdownDataToDB(appEnvironment.WebRootPath);
            _markdownService.LoadSolutionMarkdownDataToDB(appEnvironment.WebRootPath);

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

                if (!projectModal.Visible)
                {
                    ToggleSpinReel();
                }

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

                            if (!projectModal.Visible)
                            {
                                await Task.Delay(2000);
                                ToggleSpinReel();
                            }

                            await ShowProjectModal();

                            _lastSelectedProjectNum = randomProjectNum;
                        }
                    }
                }

                IsLoading = false;
            }
        }

        public async void ShowSolution()
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

                    var solution = await GetSolution(SelectedSolution.Slug, SelectedSolution.HostName);

                    SelectedSolution.SolutionContent = MarkdownHelper.Parse(solution.Content);
                    SelectedSolution.Title = solution.Title;
                    SelectedSolution.Author = solution.Author.Name;

                    SetSocialLink(solution.Author.SocialMedia);
                }
                else
                {
                    SelectedSolution = solutions.FirstOrDefault();

                    var solution = await GetSolution(SelectedSolution.Slug, SelectedSolution.HostName);

                    SelectedSolution.SolutionContent = MarkdownHelper.Parse(solution.Content);
                    SelectedSolution.Title = solution.Title;
                    SelectedSolution.Author = solution.Author.Name;

                    SetSocialLink(solution.Author.SocialMedia);
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

        private void SetSocialLink(SocialMedia socialLinks)
        {
            if (socialLinks != null)
            {
                if (!string.IsNullOrEmpty(socialLinks.Twitter))
                {
                    SelectedSolution.SocialLink = socialLinks.Twitter;
                }
                else if (!string.IsNullOrEmpty(socialLinks.Website))
                {
                    SelectedSolution.SocialLink = socialLinks.Website;
                }
                else if (!string.IsNullOrEmpty(socialLinks.GitHub))
                {
                    SelectedSolution.SocialLink = socialLinks.GitHub;
                }
                else if (!string.IsNullOrEmpty(socialLinks.LinkedIn))
                {
                    SelectedSolution.SocialLink = socialLinks.LinkedIn;
                }
            }
        }

        public async Task<Post> GetSolution(string slug, string hostName)
        {
            var client = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("https://api.hashnode.com/")
            }, new NewtonsoftJsonSerializer());

            var request = new GraphQLRequest
            {
                Query = $@"{{
      post(slug:""{slug}"", hostname: ""{hostName}"") {{
        title
        content
            author {{
      name
      publicationDomain
      socialMedia {{
        twitter
        github
        linkedin
      	website
      }}
    }}
      }}
        }}"
            };

            var response = await client.SendQueryAsync<ResponseType>(request);

            return response.Data.Post;
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
            if (projectModal != null)
            {
                projectModal.Visible = true;
            }

            return projectModal.Show();
        }

        private Task HideProjectModal()
        {
            if (projectModal != null)
            {
                projectModal.Visible = false;
            }

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

        private Task OnModalClosing(ModalClosingEventArgs e)
        {
            projectModal.Visible = false;
            IsSolutionVisible = false;
            return Task.CompletedTask;
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

        private bool spinReels = false;

        private string? SpinReelCssClass => spinReels ? "reel" : null;

        private void ToggleSpinReel()
        {
            spinReels = !spinReels;
        }
    }
}
