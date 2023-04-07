using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    /// <summary>
    /// Represents a block of text.
    /// </summary>
    internal class TextObject : ITemplateObject
    {
        public StringBuilder Text { get; }

        public TextObject()
        {
            this.Text = new StringBuilder();
        }

        public StringBuilder Process(object model)
        {
            return new StringBuilder(this.Text.ToString());
        }
    }
}
