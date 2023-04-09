using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal interface ITemplateObject
    {
        StringBuilder Process(object model);
    }
}
