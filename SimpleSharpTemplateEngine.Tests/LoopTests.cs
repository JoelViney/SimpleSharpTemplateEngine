using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SimpleSharpTemplateEngine
{
    public class ParentLoopModel
    {
        public LoopModel MyChildLoop { get; set; }
    }

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
            var template = ".{{ loop MyList }}|{{ myname }}|{{ end loop }}.";
            var model = new LoopModel
            {
                MyList = new List<LoopChildModel>(new[] { new LoopChildModel() { MyName = "One" }, new LoopChildModel() { MyName = "Two" } })
            };
            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual(".|One||Two|.", result);
        }

        [TestMethod]
        public void ParentLoop()
        {
            // Arrange
            var template = ".{{ loop MyChildLoop.MyList }}|{{ myname }}|{{ end loop }}.";
            var model = new ParentLoopModel
            {
                MyChildLoop = new LoopModel
                {
                    MyList = new List<LoopChildModel>(new[] { new LoopChildModel() { MyName = "One" }, new LoopChildModel() { MyName = "Two" } })
                }
            };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual(".|One||Two|.", result);
        }


        [TestMethod]
        public void EmptyLoop()
        {
            // Arrange
            var template = ".{{ loop MyList }}|{{ myname }}|{{ end loop }}.";
            var model = new LoopModel
            {
                MyList = new List<LoopChildModel>()
            };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("..", result);
        }

        [TestMethod]
        public void LoopThenLoop()
        {
            // Arrange
            var template = ".{{ loop MyList }}|{{ myname }}|{{ end loop }}.{{ loop MyList }}|{{ myname }}|{{ end loop }}.";
            var model = new LoopModel
            {
                MyList = new List<LoopChildModel>(new[] { new LoopChildModel() { MyName = "One" }, new LoopChildModel() { MyName = "Two" } })
            };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual(".|One||Two|.|One||Two|.", result);
        }
    }
}
