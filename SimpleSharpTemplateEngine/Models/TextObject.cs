using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class TextObject : ITemplateObject
    {
        public StringBuilder Text { get; set; }

        public TextObject()
        {
            Text = new StringBuilder();
        }

        public StringBuilder Process(object model)
        {
            return new StringBuilder(this.Text.ToString());
        }
    }
}
