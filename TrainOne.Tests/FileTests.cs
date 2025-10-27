
using NUnit.Framework;
using FluentAssertions;
using NLog;
using File = Task1.SourceCode.File;

namespace TrainOne.Tests
{
    [TestFixture]
    public class FileTests
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [SetUp]
        public void Setup()
        {
            Logger.Info("Starting test setup...");
        }

        [Test]
        public void Constructor_Should_Set_FileName_And_Content_When_Valid()
        {
            var fileName = "document.txt";
            var content = "Hello world";
            var file = new File(fileName, content);

            file.GetFileName().Should().Be(fileName);

            var expected = (double)(content.Length / 2);
            file.GetSize().Should().Be(expected);
        }

        [Test]
        public void Constructor_Should_Handle_FileName_Without_Extension()
        {
            var fileName = "readme";
            var content = "content";
            var file = new File(fileName, content);

            file.GetFileName().Should().Be(fileName);

            var expected = (double)(content.Length / 2);
            file.GetSize().Should().Be(expected);
        }

        [Test]
        public void GetSize_Should_Return_Zero_When_Content_Is_Empty()
        {
            var file = new File("empty.txt", "");
            var size = file.GetSize();
            size.Should().Be(0);
        }

        [Test]
        public void GetFileName_Should_Return_Correct_Value()
        {
            var file = new File("myfile.log", "abc");
            var result = file.GetFileName();

            result.Should().Be("myfile.log");
        }

        [Test]
        public void Constructor_Should_Throw_When_FileName_Is_Null()
        {
            string? fileName = null;
            string content = "some content";
            var act = () => new File(fileName!, content);

            act.Should().Throw<NullReferenceException>();
        }

        [Test]
        public void Constructor_Should_Throw_When_Content_Is_Null()
        {
            string fileName = "test.txt";
            string? content = null;
            var act = () => new File(fileName, content!);

            act.Should().Throw<NullReferenceException>();
        }

        [Test]
        public void Multiple_Calls_Should_Return_Same_Results()
        {
            var file = new File("data.csv", "abcdef");
            var size1 = file.GetSize();
            var size2 = file.GetSize();
            var name1 = file.GetFileName();
            var name2 = file.GetFileName();

            size1.Should().Be(size2);
            name1.Should().Be(name2);
        }
    }
}
