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
        public string Name { get; set; }
    }

    [TestClass]
    public class LoopTests
    {

        [TestMethod]
        public void Loop()
        {
            // Arrange
            var text = ".##STARTLOOP:mylist##|##name##|##ENDLOOP##.";
            var model = new LoopModel
            {
                MyList = new List<LoopChildModel>(new[] { new LoopChildModel() { Name = "One" }, new LoopChildModel() { Name = "Two" } })
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
            var text = ".##STARTLOOP:mylist##|##name##|##ENDLOOP##.";
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
            var text = ".##STARTLOOP:mylist##|##name##|##ENDLOOP##.##STARTLOOP:mylist##|##name##|##ENDLOOP##.";
            var model = new LoopModel
            {
                MyList = new List<LoopChildModel>(new[] { new LoopChildModel() { Name = "One" }, new LoopChildModel() { Name = "Two" } })
            };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual(".|One||Two|.|One||Two|.", result);
        }
    }
}
