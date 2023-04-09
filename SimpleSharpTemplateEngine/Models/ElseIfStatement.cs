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
            Type modelType = model.GetType();
            PropertyInfo[] properties = modelType.GetProperties();
            var property = properties.FirstOrDefault(x => x.Name.ToLower() == this.PropertyName.ToLower());

            if (property == null)
                throw new TemplateEngineException($"Unable to locate the property ##{this.PropertyName}##");

            if (!typeof(bool).IsAssignableFrom(property.PropertyType))
                throw new TemplateEngineException($"The if variable ##{this.PropertyName}## isn't a boolean.");

            var value = property.GetValue(model) as bool?;

            return (value == true);
        }

        public StringBuilder Process(object model)
        {
            return this.Contents.Process(model);
        }
    }
}
