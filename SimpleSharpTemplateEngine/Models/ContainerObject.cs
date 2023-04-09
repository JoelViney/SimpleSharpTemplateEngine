using System.Collections.Generic;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    /// <summary>
    /// An item that can be processed for output and contains other objects that can be processed.
    /// </summary>
    internal class ContainerObject : ITemplateObject
    {
        public List<ITemplateObject> Items { get; set; }

        public ContainerObject()
        {
            this.Items = new List<ITemplateObject>();
        }

        public ContainerObject(ITemplateObject item)
        {
            this.Items = new List<ITemplateObject>() { item };
        }

        public StringBuilder Process(object model)
        {
            var builder = new StringBuilder();

            foreach (var item in this.Items)
            {
                builder.Append(item.Process(model));
            }

            return builder;
        }
    }
}
