using lrn.devgalop.projectcreator.app.Extensions;
using lrn.devgalop.projectcreator.app.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/**
Input parameters:
1) Folder Path (string) => C:\\temp\\HelloWorld
2) Project name (string) => hello_world
3) Project type (string) => webapi, console, webapp, etc... (See more executing: dotnet new list)
**/
var builder = Host.CreateApplicationBuilder(args);

try
{
    builder.Services.AddCustomServices();

    var serviceProvider = builder.Services.BuildServiceProvider();
    var technologiesService = (ITechnologiesService) serviceProvider.GetRequiredService(typeof(ITechnologiesService));
    var commandService = (ICommandService) serviceProvider.GetRequiredService(typeof(ICommandService));
    var templatesService = (ITemplateService) serviceProvider.GetRequiredService(typeof(ITemplateService));
    var languageSelected = technologiesService.GetTechnology();
    if(string.IsNullOrEmpty(languageSelected)) throw new Exception("A programming language must be selected");
    GeneratorService generator;
    switch(languageSelected)
    {
        case "c#":
            generator = new CSharpGeneratorService(commandService, technologiesService, templatesService);
            generator.RunCommands();
        break;
        case "python":
            Console.WriteLine("Not implemented yet");
        break;
        default:
            Console.WriteLine("Invalid option");
        break;
    }
    
    
}
catch (Exception ex)
{
    Console.WriteLine($"Project cannot be created. Error: {ex}");
}


