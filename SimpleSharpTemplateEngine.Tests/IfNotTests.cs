﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    public class NotIfModel
    {
        public bool MyProperty1 { get; set; }
    }

    [TestClass]
    public class IfNotTests
    {
        [DataTestMethod]
        [DataRow("{{if not MyProperty1}}Hello World.{{end if}}", "Hello World.")]
        [DataRow("{{ if not MyProperty1 }}Hello World.{{ end if }}", "Hello World.")]
        [DataRow("{{ IF NOT MyProperty1 }}Hello World.{{ END IF }}", "Hello World.")]
        public void IfNotIsApplied(string text, string expected)
        {
            // Arrange
            var model = new NotIfModel() { MyProperty1 = false };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void IfNotIsntApplied()
        {
            // Arrange
            var text = "{{ if not MyProperty1 }}This text is inside the If{{ end if }}";
            var model = new NotIfModel() { MyProperty1 = true };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("", result);
        }

    }
}
