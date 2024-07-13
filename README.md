# How it works
- Hello! I've set <SpaProxyLaunchCommand>npm start</SpaProxyLaunchCommand>, so solution should be launched simply by F5
- If it no, try to cd \promomashinc.client, run npm install, npm build, npm start and run again
# About
- I'm using SqlLite to not waste time for deploy, change connection string, etc (but i had so much problem with it, working with MSSQL or Postgres much easier)
- Usually I work with DevExtreme (or KendoUI) ui components, so I didn't use angular material before (but it's interesting experience) )
- Firstly, I created simple layered solution for simple registration form (tag simple_solution_done in git)
- Then I added Mediatr and some simple cache. I used CQRS only in the win services before, into some worker processes, so maybe I violated some best practice with Mediatr)
- But as for me for most cases simple layered architecture is more usable
- Thanks for your attention!

# Stack
- .NET 8.0 
- Microsoft.EntityFrameworkCore.Sqlite 8.0.6
- Angular 18.0.0
- Angular/material 18.0.0
