using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    /// <summary>
    /// Represents an item in a container object that can be processed for output.
    /// </summary>
    internal interface ITemplateObject
    {
        StringBuilder Process(object model);
    }
}
