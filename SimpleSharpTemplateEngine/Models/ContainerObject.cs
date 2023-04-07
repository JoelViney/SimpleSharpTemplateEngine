using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class ContainerObject : ITemplateObject
    {
        public List<ITemplateObject> Items { get; set; }

        public ContainerObject()
        {
            this.Items = new List<ITemplateObject>();
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
