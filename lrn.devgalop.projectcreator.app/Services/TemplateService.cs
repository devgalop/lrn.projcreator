using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lrn.devgalop.projectcreator.app.Extensions;

namespace lrn.devgalop.projectcreator.app.Services
{
    public class TemplateService : ITemplateService
    {
        private List<string> _architectureTemplates;
        Dictionary<string, List<string>> _projectTemplateFolders;
        public TemplateService()
        {
            _architectureTemplates = new() { "onion", "clean", "hexagonal", "layered" };
            _projectTemplateFolders = new()
            {
                {"onion", new(){ "Core", "Infrastructure", "ProjectType"}},
                {"clean", new(){ "Domain","UseCases", "Infrastructure","ProjectType"}},
                {"hexagonal", new(){ "Core", "Adpters","ProjectType"}},
                {"layered", new(){ "Presentation","Domain","Infrastructure","ProjectType"}}
            };
        }

        public string GetTemplate()
        {
            return _architectureTemplates.SelectMultipleChoice();
        }

        public List<string> GetTemplateFolders(string template)
        {
            _ = _projectTemplateFolders.TryGetValue(template, out List<string>? folders);
            return folders ?? new();
        }
    }

    public interface ITemplateService
    {
        string GetTemplate();
        List<string> GetTemplateFolders(string template);
    }
}