1. Creating a blank solution then adding a new Web API project
  - ASPNET Core Web API
2. Add Swagger to the project
  - Install the Swashbuckle.AspNetCore NuGet package
  - Configure Swagger in Program.cs
	- builder.Services.AddSwaggerGen();
	- app.UseSwagger();
	- app.UseSwaggerUI();
  - Configure launchSettings.json
	- "launchBrowser": true,
	- "launchUrl": "swagger"