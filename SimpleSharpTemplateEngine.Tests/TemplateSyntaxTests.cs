using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    [TestClass]
    public class TemplateSyntaxTests
    {
        [TestMethod]
        [ExpectedException(typeof(TemplateEngineException))]
        public void CommandBlockWithoutClosingStatement()
        {
            // Arrange
            var template = "Hello {{ MyProperty.";
            var model = new TemplateModel();

            // Act
            TemplateEngine.Execute(template, model);

            // Assert - Expecting an exception
        }


        [TestMethod]
        public void SingleDelimiter()
        {
            // Arrange
            var template = "Hello { World.";
            var model = new TemplateModel();

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("Hello { World.", result);
        }
    }
}
