using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SimpleSharpTemplateEngine
{
    public class TemplateModel
    {
        public string MyProperty { get; set; }
        public int MyNumber { get; set; }
        public DateTime MyDateTime { get; set; }
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

        [DataTestMethod]
        [DataRow("Hello {{ MyNumber }}.", "Hello 1234.")]
        [DataRow("Hello {{ MyNumber: C }}.", "Hello $1,234.00.")]
        public void ReplaceNumber(string template, string expected)
        {
            // Arrange
            var model = new TemplateModel() { MyNumber = 1234 };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("Hello {{ MyDateTime: yyyy-MM-dd }}.", "Hello 2020-03-29.")]
        [DataRow("Hello {{ MyDateTime: yyyy-MM-dd HH:mm:ss }}.", "Hello 2020-03-29 14:34:55.")]
        [DataRow("Hello {{ MyDateTime: dddd }}.", "Hello Sunday.")]
        public void ReplaceDateTime(string template, string expected)
        {
            // Arrange
            var model = new TemplateModel() { MyDateTime = new DateTime(2020, 03, 29, 14, 34, 55)};

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual(expected, result);
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
