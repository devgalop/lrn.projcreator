using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lrn.devgalop.projectcreator.app.Services;
using Microsoft.Extensions.DependencyInjection;

namespace lrn.devgalop.projectcreator.app.Extensions
{
    public static class GeneralExtensions
    {
        public static string SelectMultipleChoice(this List<string> options)
        {
            string result = string.Empty;
            bool isCanceled;
            do
            {
                Console.WriteLine("Please select one of the following:");
                for (int idx = 0; idx < options.Count; idx++)
                {
                    Console.WriteLine($"[{idx}]: {options[idx]}");
                }
                var validOptionSelected = int.TryParse(Console.ReadLine(), out int currentSelection);
                if(!validOptionSelected || currentSelection < 0 || currentSelection > options.Count)
                {
                    Console.WriteLine("Invalid option selected. Type 'Y' to retry or 'N' to cancel.");
                    var retrySelected = Console.ReadLine()??"N";
                    isCanceled = retrySelected.ToLower() == "n";
                    continue;
                }
                result = options[currentSelection];
                Console.WriteLine($"You have selected the option: {result}");
                isCanceled = true;
            }while(!isCanceled);
            return result;
        }
        
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<ICommandService, CommandService>();
            services.AddTransient<ITechnologiesService, TechnologiesService>();
            services.AddTransient<ITemplateService, TemplateService>();
        }
    }
}