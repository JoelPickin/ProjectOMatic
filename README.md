<div style="position: relative; width: 100%; height: 0; padding-top: 25.0000%;
 padding-bottom: 0; box-shadow: 0 2px 8px 0 rgba(63,69,81,0.16); margin-top: 1.6em; margin-bottom: 0.9em; overflow: hidden;
 border-radius: 8px; will-change: transform;">
  <iframe loading="lazy" style="position: absolute; width: 100%; height: 100%; top: 0; left: 0; border: none; padding: 0;margin: 0;"
    src="https:&#x2F;&#x2F;www.canva.com&#x2F;design&#x2F;DAFOJ6HT5so&#x2F;watch?embed" allowfullscreen="allowfullscreen" allow="fullscreen">
  </iframe>
</div>

## 3 Goals of Project-O-Matic
- An introduction to contributing to open-source
- Learn/improve your technical writing
- Find inspiration for your next project

## An Accessible Introduction to Contributing to Open-Source
Having difficulties starting your first contribution to Open-Source Project? Or maybe you have contributed and just looking for the next issue to get your teeth into!
This is where The Project-O-Matic comes in!
The projects are loaded via markdown files. I have seeded the database with a few to get started. The next step involves the help of developers like you.
We need you to provide project ideas and solutions for your language/framework of choice.
Follow the steps below to get an idea of how you can contribute to the project and simultaneously improve your technical writing skills.
## Learn and Improve Your Technical Writing
Two things that can improve your coding knowledge are building projects and teaching others. The Project-O-Matic open-source collaboration effort helps with both!
You build projects as you usually would, then you share the solutions of how you created them by creating a blog post on Hashnode.
Writing the steps you have taken to create a project and the concepts you have learned cement the knowledge for future use and forces you to explain it in a way understandable to others.
Contribution is entirely optional.... for now *cocks gun*. But seriously, it's your choice to improve your coding and writing skills while contributing to open-source.
This is just here to guide you in these areas so you can go off and create/contribute to amazing things.
## Find Project Inspiration
Head to the website https://project-o-matic.herokuapp.com/ and choose your skill level, language, and framework. Please sit back and let the project-o-matic work its magic.
You will be given a project brief that will tell you what the project will achieve and what features are expected.
Some project briefs may have a solution if you get stuck or want to see how other developers have approached the problem.
Many of them won't; that’s where the following two points come in.
## How to Contribute
[Check out this guide to learn how to contribute to open-source](https://opensource.guide/how-to-contribute/)
### How to Add a Project
- Create a markdown file using this template. You can create it in any text editor or an online markdown editor. Save it locally with the name of the project. E.g., Calculator. md
- Consider each step someone would need to take to create a project.
- If possible, make three different variations. One for beginners, one with more steps for intermediate, and one fully fledged app to cover more profound concepts for advanced learners.

```
# <Project Title Name> 
# REMOVE - Example(Calculator)

*<Project Description>*
REMOVE - *Example(Create a calculator)*

## Features
<Add a list of features>
- Create a grid UI
    - Add numbers 0 - 9
    - Add a plus button
    - Add a minus button
    - Add an equals button
    - Add a multiply button
    - Add a modulo/remainder operator
    - Add a decimal button
    - Add a reset button
    - Input section to show entered numbers

### Created By: [YOUR NAME](YOUR CHOSEN SOCIAL MEDIA OR WEBSITE) 
```

- Download the code
- Open in your chosen IDE
- Upload the file to the www root < project-files < beginner/intermediate/advanced depending on the markdown file. Ensure each file for each skill level has the same name.
- Commit your code to source control on GitHub.
- Create a pull request.
- Wait for it to be reviewed.
- Merge it into the main branch.
- The system does the rest of the work to add the project to the database.

## How to Add a Solution

[Hashnode Getting Started Docs](https://support.hashnode.com/docs/)

[Check out this guide on getting started with technical writing](https://alexandriastech.hashnode.dev/what-you-should-know-about-technical-writing-and-get-paid)

- Head over to [Hashnode](https://hashnode.com/).
- Create an account if you don't have one
- Create a new blog post.
- Using markdown, write up the steps you took to create a project. I was using links, libraries, images, etc.
- Publish your post.
- Then copy the slug of your post. The slug is the line following your hashnode blog domain.
- Head over to the GitHub repo.
- Download the code.
- Open your chosen IDE.
- Open Solutions.json, which is under wwwroot inside a folder called solution-files.
- Copy and paste the last entry in this file and fill in the required details.
- Slug is the part of the blog post URL you copied earlier.
- HostName is your Hashnode account name.
- ProjectName is the name of the project file your solution relates to. If you need to check this it will be under wwwroot < project-files. If one doesn't exist for your created solution, add one using the guide above in "How to add a project.”
- LanguageName is the language it relates to. This is always in PascalCase.
- FrameworkName is the framework it relates to. If the project doesn't use a framework, it will always be "Vanilla.” If it does, add the correct name. e.g., React, Vue, etc.
- Save the file.
- Commit your code to source control on GitHub.
- Create a pull request.
- Wait for the code to be reviewed.
- Merge the code into the main branch.

## Tech Stack
- [Blazor Server-Side](https://github.com/dotnet/blazor)
- HTML & CSS
- Heroku

## Libraries Used
- [Blazorise](https://github.com/Megabit/Blazorise)
- Entity Framework Core
- [Markdig](https://github.com/xoofx/markdig)

## License
[MIT License.txt](https://github.com/JoelPickin/ProjectOMatic/files/9226451/MIT.License.txt)

## Contact

If your chosen tech stack or framework isn't available, you can create an issue or contact me on Twitter [@TechPickleJoel](https://twitter.com/TechPickleJoel)

## Hackathon

This project was created as part of the [Planetscale](https://planetscale.com/) X [Hashnode Hackathon](https://townhall.hashnode.com/planetscale-hackathon)