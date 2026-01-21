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
3. Created new controller with endpoints (for now only returns OK status)
  - POST images/process?encoding={encoding} Form: IFormFile file
  - I removed the Attributes from parameters, because they causes errors with Swagger