using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    public class TemplateModel
    {
        public string MyName { get; set; }
    }

    [TestClass]
    public class TemplateTests
    {

        [DataTestMethod]
        [DataRow("Hello {{myname}}.", "Hello World.")]
        [DataRow("Hello {{my-name}}.", "Hello World.")]
        [DataRow("Hello {{ myname }}.", "Hello World.")]
        [DataRow("Hello {{ my-name }}.", "Hello World.")]
        public void ReplaceText(string text, string expected)
        {
            // Arrange
            var model = new TemplateModel() { MyName = "World" };

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
    }
}
