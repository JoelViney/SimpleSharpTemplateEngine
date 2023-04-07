using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    public class NotIfModel
    {
        public bool MyProperty1 { get; set; }
    }

    [TestClass]
    public class IfNotTests
    {
        [TestMethod]
        public void IfNotIsTrue()
        {
            // Arrange
            var text = ".##IFNOT:MyProperty1##abc##ENDIF##.";
            var model = new NotIfModel() { MyProperty1 = false };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual(".abc.", result);
        }

        [TestMethod]
        public void IfNotIsFalse()
        {
            // Arrange
            var text = ".##IFNOT:MyProperty1##This text is inside the If##ENDIF##.";
            var model = new NotIfModel() { MyProperty1 = true };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("..", result);
        }

    }
}
