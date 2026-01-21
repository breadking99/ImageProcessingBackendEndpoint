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
4. Added C++ DLL project to the solution
  - added dependencies to to the Api project
5. Testing Dll import in the controller
  - added a multiple by two test function in the DLL
  - imported the function with DllImport in the controller with help DllNative static class
  - edited the Api project file, so if it rebuilds, it copies the Dll to the output folder
6. Limit what file types can be uploaded in the controller