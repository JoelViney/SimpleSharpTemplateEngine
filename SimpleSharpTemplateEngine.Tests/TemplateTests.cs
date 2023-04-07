using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SimpleSharpTemplateEngine
{
    public class TemplateModel
    {
        public string Name { get; set; }
    }

    [TestClass]
    public class TemplateTests
    {
        [TestMethod]
        public void JustText()
        {
            // Arrange
            var text = "Hello World.";
            var model = new TemplateModel() { Name = "abc" };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Hello World.", result);
        }

        [TestMethod]
        public void ReplaceText()
        {
            // Arrange
            var text = "Hello ##name##.";
            var model = new TemplateModel() { Name = "World" };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Hello World.", result);
        }
    }
}
