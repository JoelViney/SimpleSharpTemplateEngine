using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SimpleSharpTemplateEngine
{
    public class TestModel
    {
        public string Name { get; set; }
        public bool MyProperty1 { get; set; }
        public bool MyProperty2 { get; set; }
        public List<TestModel> List { get; set; }
    }

    [TestClass]
    public class TemplateEngineTests
    {
        [TestMethod]
        public void JustText()
        {
            // Arrange
            var engine = new TemplateEngine();
            var text = "Hello World.";
            var model = new TestModel() { Name = "abc" };

            // Act
            var result = engine.Execute(text, model);

            // Assert
            Assert.AreEqual("Hello World.", result);
        }

        [TestMethod]
        public void ReplaceText()
        {
            // Arrange
            var engine = new TemplateEngine();
            var text = "Hello ##name##.";
            var model = new TestModel() { Name = "World" };

            // Act
            var result = engine.Execute(text, model);

            // Assert
            Assert.AreEqual("Hello World.", result);
        }

        [TestMethod]
        public void IfTrue()
        {
            // Arrange
            var engine = new TemplateEngine();
            var text = ".##IF:MyProperty1##abc##ENDIF##.";
            var model = new TestModel() { MyProperty1 = true };

            // Act
            var result = engine.Execute(text, model);

            // Assert
            Assert.AreEqual(".abc.", result);
        }

        [TestMethod]
        public void IfFalse()
        {
            // Arrange
            var engine = new TemplateEngine();
            var text = ".##IF:MyProperty1##This text is inside the If##ENDIF##.";
            var model = new TestModel() { MyProperty1 = false };

            // Act
            var result = engine.Execute(text, model);

            // Assert
            Assert.AreEqual("..", result);
        }

        [TestMethod]
        public void NestedIf()
        {
            // Arrange
            var engine = new TemplateEngine();
            var text = ".##IF:MyProperty1##.##IF:MyProperty2##|Test|##ENDIF##.##ENDIF##.";
            var model = new TestModel() { MyProperty1 = true, MyProperty2 = true };

            // Act
            var result = engine.Execute(text, model);

            // Assert
            Assert.AreEqual("..|Test|..", result);
        }

        [TestMethod]
        public void NestedIfInnerFalse()
        {
            // Arrange
            var engine = new TemplateEngine();
            var text = ".##IF:MyProperty1##.##IF:MyProperty2##|Test|##ENDIF##.##ENDIF##.";
            var model = new TestModel() { MyProperty1 = true, MyProperty2 = false };

            // Act
            var result = engine.Execute(text, model);

            // Assert
            Assert.AreEqual("....", result);
        }

        [TestMethod]
        public void Loop()
        {
            // Arrange
            var engine = new TemplateEngine();
            var text = ".##STARTLOOP:list##|##name##|##ENDLOOP##.";
            var model = new TestModel
            {
                List = new List<TestModel>(new[] { new TestModel() { Name = "One" }, new TestModel() { Name = "Two" } })
            };
            // Act
            var result = engine.Execute(text, model);

            // Assert
            Assert.AreEqual(".|One||Two|.", result);
        }


        [TestMethod]
        public void EmptyLoop()
        {
            // Arrange
            var engine = new TemplateEngine();
            var text = ".##STARTLOOP:list##|##name##|##ENDLOOP##.";
            var model = new TestModel
            {
                List = new List<TestModel>()
            };

            // Act
            var result = engine.Execute(text, model);

            // Assert
            Assert.AreEqual("..", result);
        }

        [TestMethod]
        public void TwoLoops()
        {
            // Arrange
            var engine = new TemplateEngine();
            var text = ".##STARTLOOP:list##|##name##|##ENDLOOP##.##STARTLOOP:list##|##name##|##ENDLOOP##.";
            var model = new TestModel
            {
                List = new List<TestModel>(new[] { new TestModel() { Name = "One" }, new TestModel() { Name = "Two" } })
            };
            // Act
            var result = engine.Execute(text, model);

            // Assert
            Assert.AreEqual(".|One||Two|.|One||Two|.", result);
        }

        [TestMethod]
        public void IfThenLoop()
        {
            // Arrange
            var engine = new TemplateEngine();
            var text = ".##IF:MyProperty1##.##STARTLOOP:list##|##name##|##ENDLOOP##.##ENDIF##.";
            var model = new TestModel
            {
                MyProperty1 = true,
                List = new List<TestModel>(new[] { new TestModel() { Name = "One" }, new TestModel() { Name = "Two" } })
            };
            // Act
            var result = engine.Execute(text, model);

            // Assert
            Assert.AreEqual("..|One||Two|..", result);
        }
    }
}
