using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    public class ParentTemplateModel
    {
        public ChildTemplateModel MyChild { get; set; }
    }

    public class ChildTemplateModel
    {
        public string MyProperty { get; set; }
        public GrandchildTemplateModel MyChild { get; set; }
    }

    public class GrandchildTemplateModel
    {
        public string MyProperty { get; set; }
    }

    [TestClass]
    public class TemplateChainingTests
    {
        [TestMethod]
        public void ChildProperty()
        {
            // Arrange
            var template = "Hello {{ MyChild.MyProperty }}.";
            var model = new ParentTemplateModel() { MyChild = new ChildTemplateModel() { MyProperty = "World"} };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("Hello World.", result);
        }

        [TestMethod]
        public void GrandchildProperty()
        {
            // Arrange
            var template = "Hello {{ MyChild.MyChild.MyProperty }}.";
            var model = new ParentTemplateModel() 
            { 
                MyChild = new ChildTemplateModel() 
                { 
                    MyProperty = "",
                    MyChild = new GrandchildTemplateModel()
                    {
                        MyProperty = "World"
                    }
                } 
            };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("Hello World.", result);
        }
    }
}
