using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    public class TemplateModel
    {
        public string MyProperty { get; set; }
        public int MyNumber { get; set; }
    }

    [TestClass]
    public class TemplateTests
    {

        [DataTestMethod]
        [DataRow("Hello {{ MyProperty }}.", "Hello World.")]
        [DataRow("Hello {{ my-property }}.", "Hello World.")]
        [DataRow("Hello {{myproperty}}.", "Hello World.")]
        public void ReplaceText(string template, string expected)
        {
            // Arrange
            var model = new TemplateModel() { MyProperty = "World" };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ReplaceNumberText()
        {
            // Arrange
            var template = "Hello {{ MyNumber }}.";
            var model = new TemplateModel() { MyNumber = 123 };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("Hello 123.", result);
        }

        [TestMethod]
        public void EmptyTemplate()
        {
            // Arrange
            var template = "";
            var model = new TemplateModel();

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void JustText()
        {
            // Arrange
            var template = "Hello World.";
            var model = new TemplateModel();

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("Hello World.", result);
        }

        [TestMethod]
        public void NullProperty()
        {
            // Arrange
            var template = "Hello {{ MyProperty }}.";
            var model = new TemplateModel() { MyProperty = null };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("Hello .", result);
        }
    }
}
