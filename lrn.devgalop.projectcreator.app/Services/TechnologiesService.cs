using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lrn.devgalop.projectcreator.app.Extensions;

namespace lrn.devgalop.projectcreator.app.Services
{
    public class TechnologiesService : ITechnologiesService
    {
        private List<string> _technologies;
        private Dictionary<string, List<string>> _projectTypes;

        public TechnologiesService()
        {
            _technologies = new() { "c#", "python" };
            _projectTypes = new()
            {
                {"c#", new(){ "webapi", "console", "webapp"}},
                {"python", new(){"console","webapp"}}
            };
        }

        public string GetTechnology()
        {
            return _technologies.SelectMultipleChoice();
        }

        public List<string> GetProjectTypes(string technology)
        {
            _ = _projectTypes.TryGetValue(technology, out List<string>? projectTypes);
            return projectTypes ?? new();
        }
    }

    public interface ITechnologiesService
    {
        string GetTechnology();
        List<string> GetProjectTypes(string technology);
    }
}