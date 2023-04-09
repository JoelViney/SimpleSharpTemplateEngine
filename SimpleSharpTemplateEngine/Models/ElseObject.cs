using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class ElseObject : IIfStatementObject, ITemplateObject, IIfStatementChainingObject
    {
        public ContainerObject Contents { get; }

        public ElseObject(ContainerObject contents)
        {
            this.Contents = contents;
        }

        public bool MatchesExpression(object model)
        {
            return true; // If the else gets called, we always want to use it.
        }

        public StringBuilder Process(object model)
        {
            return this.Contents.Process(model);
        }
    }
}
