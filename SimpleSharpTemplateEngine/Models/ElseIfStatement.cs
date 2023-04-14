using SimpleSharpTemplateEngine.Helpers;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class ElseIfStatement : IIfStatementObject, ITemplateObject, IIfStatementChainingObject
    {
        public string PropertyName { get; }
        public ContainerObject Contents { get; }

        public ElseIfStatement(string propertyName, ContainerObject contents)
        {
            this.PropertyName = propertyName;
            this.Contents = contents;
        }

        public bool MatchesExpression(object model)
        {
            var property = PropertyHelper.GetReferencedProperty(model, this.PropertyName);

            if (property is bool value)
            {
                return value;
            }
            
            throw new TemplateEngineException($"The if variable '{this.PropertyName}' isn't a boolean.");
        }

        public StringBuilder Process(object model)
        {
            return this.Contents.Process(model);
        }
    }
}
