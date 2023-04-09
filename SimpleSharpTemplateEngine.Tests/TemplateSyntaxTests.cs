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
            var text = "Hello {{ MyProperty.";
            var model = new TemplateModel();

            // Act
            TemplateEngine.Execute(text, model);

            // Assert - Expecting an exception
        }


        [TestMethod]
        public void SingleDelimiter()
        {
            // Arrange
            var text = "Hello { World.";
            var model = new TemplateModel();

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Hello { World.", result);
        }
    }
}
