# Project-o-Matic

**An open-source collaboration effort to give you project inspiration whilst improving your technical writing skills.**

## This project is here to achieve three things

- Find inspiration for your next project
- An introduction to contributing to open-source
- Learn/improve your technical writing

## Find project inspiration

Just head on over to the website https://project-o-matic.herokuapp.com/ choose your skill level, language and framework. Sit back and let the project-o-matic work it's magic.

You will be given a project brief which will tell you what the project will achieve and what features are expected.

Some project briefs may have a solution for if you get stuck, or if you just want to see how other developers have approached the problem.

A lot of them won't, that's where the next two points come in.

## An easy introduction to contributing to open-source

Have you always wanted to contribute to open-source and didn't know how?

Maybe you have contributed and are just looking for the next issue to get your teeth into!

This is where The Project-O-Matic comes in!

The projects are loaded via markdown files. I have seeded the database with a few to get started. The next step involves the help of developers like you.

We need you to provide project ideas and solutions for your language/framework of choice.

Follow the steps outlined below to get an idea of how you can contribute to the project and improve your technical writing skills at the same time.

## Learn/improve your technical writing

Two things that can improve your coding knowledge the most... building projects and teaching others. The Project-O-Matic open-source collaboration effort helps with both!

You build projects as you usually would, then you share the solutions of how you created them by creating a blog post on Hashnode.

Writing the steps you have taken to create a project and the concepts you have learnt cements the knowledge for future use and forces you to explain it in a way that is understandable to other people.

Contribution is entirely optional.... for now *cocks gun*. But seriously, it's your choice to improve your coding and writing skills, whilst contributing to open-source.

This is just here to guide you in these areas so you can go off and create/contribute to amazing things.

## How to contribute

[Check out this guide to learn how to contribute to open-source](https://opensource.guide/how-to-contribute/)

### How to add a project

- Create a markdown file using this template. You can create it in any text editor or an online markdown editor. Save it locally with the name of the project. E.g. Calculator.md
- Consider each individual step someone would need to take to create a project.
- If possible make three different variations. One for beginners, one with more steps for intermediate and one fully fledged app to cover deeper concepts for advanced learners.


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
- Upload the file to the wwwroot < project-files < beginner/intermediate/advanced depending on the markdown file. Ensure each file for each skill level has the same name.
- Commit your code to source control on GitHub.
- Create a pull request.
- Wait for it to be reviewed.
- Merge it into the master branch.
- The system does the rest of the work to add the project to the database.

## How to add a solution

[Hashnode Getting Started Docs](https://support.hashnode.com/docs/)

[Check out this guide on getting started with technical writing](https://alexandriastech.hashnode.dev/what-you-should-know-about-technical-writing-and-get-paid)

- Head over to [Hashnode](https://hashnode.com/).
- Create an account if you don't have one
- Create a new blog post.
- Using markdown write up the steps you took to create a project. Using links, libraries, images etc.
- Publish your post.
- Then copy the slug of your post. The slug is the line following your hashnode blog domain.
- Head over to the GitHub repo.
- Download the code.
- Open your chosen IDE.
- Open Solutions.json, which is under wwwroot inside a folder called solution-files.
- Copy and paste the last entry in this file and fill in the required details.
- Slug is the part of the blog post URL you copied earlier.
- HostName is your Hashnode account name.
- ProjectName is the name of the project file your solution relates to. If you need to check this it will be under wwwroot < project-files. If one doesn't exist for your created solution, add one using the guide above in "How to add a project".
- LanguageName is the language it relates to. This is always in PascalCase.
- FrameworkName is the framework it relates to. If the project doesn't use a framework it will always be "Vanilla". If it does, add the correct name. e.g. React, Vue etc.
- Save the file.
- Commit your code to source control on GitHub.
- Create a pull request.
- Wait for the code to be reviewed.
- Merge the code into the master branch.


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

This project was originally created as part of the [Planetscale](https://planetscale.com/) X [Hashnode Hackathon](https://townhall.hashnode.com/planetscale-hackathon)
