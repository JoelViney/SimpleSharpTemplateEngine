using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    public class SwitchModel
    {
        public string MyName { get; set; }
    }

    [TestClass]
    public class SwitchTests
    {
        [TestMethod]
        public void SimpleSwitch()
        {
            // Arrange
            var text = @"{{SWITCH:MyName}}{{CASE:A}}Aye{{ENDCASE}}{{ENDSWITCH}}";
            var model = new SwitchModel() { MyName = "A" };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Aye", result);
        }


        [TestMethod]
        public void Switch()
        {
            // Arrange
            var text = @"{{SWITCH:MyName}}{{CASE:A}}Aye{{ENDCASE}}{{CASE:A}}Bee{{ENDCASE}}{{ENDSWITCH}}";
            var model = new SwitchModel() { MyName = "A" };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Aye", result);
        }


        [TestMethod]
        public void SwitchSecondOption()
        {
            // Arrange
            var text = @"{{SWITCH:MyName}}{{CASE:A}}Aye{{ENDCASE}}{{CASE:B}}Bee{{ENDCASE}}{{ENDSWITCH}}";
            var model = new SwitchModel() { MyName = "B" };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Bee", result);
        }
    }
}
