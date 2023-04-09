using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class SwitchStatement : ITemplateObject
    {
        public string PropertyName { get; }
        public List<ISwitchCaseObject> Cases { get; set; }

        public SwitchStatement(string propertyName, ContainerObject container)
        {
            this.PropertyName = propertyName;

            this.Cases = new List<ISwitchCaseObject>();
            foreach (var item in container.Items) 
            {
                if (!(item is SwitchCaseStatement switchCase))
                {
                    throw new TemplateEngineException("Invalid item in case statement.");
                }
                this.Cases.Add(switchCase);
            }
        }

        public StringBuilder Process(object model)
        {
            Type modelType = model.GetType();
            PropertyInfo[] properties = modelType.GetProperties();
            var property = properties.FirstOrDefault(x => x.Name.ToLower() == this.PropertyName.ToLower());

            if (property == null)          
                throw new TemplateEngineException($"Unable to locate the property ##{this.PropertyName}##");

            var value = modelType.GetProperty(property.Name).GetValue(model);

            foreach(var switchCase in this.Cases)
            { 
                if (switchCase.MatchExpression(value))
                {
                    return switchCase.Process(model);
                }
            }

            return new StringBuilder();
        }
    }
}
