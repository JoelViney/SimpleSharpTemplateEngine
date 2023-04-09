using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    public class TemplateModel
    {
        public string MyProperty { get; set; }
    }

    [TestClass]
    public class TemplateTests
    {

        [DataTestMethod]
        [DataRow("Hello {{ MyProperty }}.", "Hello World.")]
        [DataRow("Hello {{ my-property }}.", "Hello World.")]
        [DataRow("Hello {{myproperty}}.", "Hello World.")]
        public void ReplaceText(string text, string expected)
        {
            // Arrange
            var model = new TemplateModel() { MyProperty = "World" };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void EmptyTemplate()
        {
            // Arrange
            var text = "";
            var model = new TemplateModel();

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void JustText()
        {
            // Arrange
            var text = "Hello World.";
            var model = new TemplateModel();

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Hello World.", result);
        }

        [TestMethod]
        public void NullProperty()
        {
            // Arrange
            var text = "Hello {{ MyProperty }}.";
            var model = new TemplateModel() { MyProperty = null };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Hello .", result);
        }

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
    }
}
