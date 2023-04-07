using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SimpleSharpTemplateEngine
{
    public class LoopModel
    {
        public List<LoopChildModel> MyList { get; set; }
    }

    public class LoopChildModel
    {
        public string MyName { get; set; }
    }

    [TestClass]
    public class LoopTests
    {

        [TestMethod]
        public void Loop()
        {
            // Arrange
            var text = ".{{LOOP:mylist}}|{{myname}}|{{ENDLOOP}}.";
            var model = new LoopModel
            {
                MyList = new List<LoopChildModel>(new[] { new LoopChildModel() { MyName = "One" }, new LoopChildModel() { MyName = "Two" } })
            };
            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual(".|One||Two|.", result);
        }


        [TestMethod]
        public void EmptyLoop()
        {
            // Arrange
            var text = ".{{LOOP:mylist}}|{{myname}}|{{ENDLOOP}}.";
            var model = new LoopModel
            {
                MyList = new List<LoopChildModel>()
            };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("..", result);
        }

        [TestMethod]
        public void LoopThenLoop()
        {
            // Arrange
            var text = ".{{LOOP:mylist}}|{{myname}}|{{ENDLOOP}}.{{LOOP:mylist}}|{{myname}}|{{ENDLOOP}}.";
            var model = new LoopModel
            {
                MyList = new List<LoopChildModel>(new[] { new LoopChildModel() { MyName = "One" }, new LoopChildModel() { MyName = "Two" } })
            };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual(".|One||Two|.|One||Two|.", result);
        }
    }
}
