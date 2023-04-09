using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    /// <summary>
    /// Represents a block of text.
    /// </summary>
    internal class TextContainer : ITemplateObject
    {
        public StringBuilder Text { get; }

        public TextContainer()
        {
            this.Text = new StringBuilder();
        }

        public StringBuilder Process(object model)
        {
            return new StringBuilder(this.Text.ToString());
        }
    }
}
