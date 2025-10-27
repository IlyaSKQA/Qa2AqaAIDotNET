using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using NLog;
using System.Collections.Generic;
using Task1.SourceCode.exception;
using Task1.SourceCode;
using File = Task1.SourceCode.File;

namespace TrainOne.Tests
{
    [TestFixture]
    public class FileStorageTests
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Test]
        public void Storage_Should_Be_Successfully_Created_With_Default_Constructor()
        {
            var storage = new FileStorage();
            var files = storage.GetFiles();

            files.Should().BeEmpty();

            Logger.Info("Default constructor test passed.");
        }

        [Test]
        public void ConstructorWithSize_Should_InitializeProperly()
        {
            var storage = new FileStorage(50);
            var files = storage.GetFiles();

            files.Should().BeEmpty();

            Logger.Info("Constructor with custom size test passed.");
        }

        [Test]
        public void Write_Should_Add_File_When_Unique_And_Size_Fits()
        {
            var storage = new FileStorage();
            var file = new File("test.txt", new string('a', 50));
            var result = storage.Write(file);

            result.Should().BeTrue();
            storage.IsExists("test.txt").Should().BeTrue();
            storage.GetFiles().Should().ContainSingle(f => f.GetFileName() == "test.txt");

            Logger.Info("Write unique file test passed.");
        }

        [Test]
        public void Write_Should_Throw_When_FileNameAlreadyExists()
        {
            var storage = new FileStorage();
            var file1 = new File("duplicate.txt", "1234");
            var file2 = new File("duplicate.txt", "5678");
            storage.Write(file1);

            var act = () => storage.Write(file2);

            act.Should().Throw<FileNameAlreadyExistsException>();

            Logger.Info("Write duplicate filename test passed.");
        }

        [Test]
        public void Write_Should_Return_False_When_File_Too_Large()
        {
            var storage = new FileStorage();
            var largeContent = new string('a', 500);
            var largeFile = new File("big.txt", largeContent);
            var result = storage.Write(largeFile);

            result.Should().BeFalse();
            storage.IsExists("big.txt").Should().BeFalse();

            Logger.Info("Write large file test passed.");
        }

        [Test]
        public void IsExists_Should_Return_True_If_File_Added()
        {
            var storage = new FileStorage();
            var file = new File("exists.txt", "content");
            storage.Write(file);

            var exists = storage.IsExists("exists.txt");

            exists.Should().BeTrue();

            Logger.Info("IsExists positive test passed.");
        }

        [Test]
        public void IsExists_Should_Return_False_If_File_Not_Added()
        {
            var storage = new FileStorage();
            var exists = storage.IsExists("nonexistent.txt");

            exists.Should().BeFalse();

            Logger.Info("IsExists negative test passed.");
        }

        [Test]
        public void Delete_Should_Remove_Existing_File()
        {
            var storage = new FileStorage();
            var file = new File("delete.txt", "abc");
            storage.Write(file);

            var result = storage.Delete("delete.txt");

            result.Should().BeTrue();
            storage.IsExists("delete.txt").Should().BeFalse();

            Logger.Info("Delete existing file test passed.");
        }

        [Test]
        public void Delete_Should_Return_False_If_File_Not_Found()
        {
            var storage = new FileStorage();
            var result = storage.Delete("notfound.txt");

            result.Should().BeFalse();

            Logger.Info("Delete non-existent file test passed.");
        }

        [Test]
        public void GetFiles_Should_Return_All_Added_Files()
        {
            var storage = new FileStorage();
            var file1 = new File("f1.txt", "1234");
            var file2 = new File("f2.txt", "5678");
            storage.Write(file1);
            storage.Write(file2);

            var files = storage.GetFiles();

            files.Should().HaveCount(2);
            files.Should().Contain(f => f.GetFileName() == "f1.txt");
            files.Should().Contain(f => f.GetFileName() == "f2.txt");

            Logger.Info("GetFiles test passed.");
        }

        [Test]
        public void GetFile_Should_Return_File_When_Exists()
        {
            var storage = new FileStorage();
            var file = new File("getme.txt", "abc");
            storage.Write(file);

            var retrieved = storage.GetFile("getme.txt");

            retrieved.Should().BeEquivalentTo(file);

            Logger.Info("GetFile existing file test passed.");
        }

        [Test]
        public void GetFile_Should_Return_Null_When_File_Not_Found()
        {
            var storage = new FileStorage();
            var retrieved = storage.GetFile("nofile.txt");

            retrieved.Should().BeNull();

            Logger.Info("GetFile non-existent file test passed.");
        }

        [Test]
        public void File_Constructor_Should_Set_Name_And_Size_Correctly()
        {
            var fileName = "sample.txt";
            var content = "abcd";
            var file = new File(fileName, content);

            file.GetFileName().Should().Be(fileName);
            file.GetSize().Should().Be(content.Length / 2.0);

            Logger.Info("File constructor sets name and size correctly.");
        }

        [Test]
        public void Constructor_Should_Handle_FileName_Without_Extension()
        {
            var fileName = "readme";
            var content = "hello";
            var file = new File(fileName, content);

            file.GetFileName().Should().Be(fileName);

            var expected = (double)(content.Length / 2);
            file.GetSize().Should().Be(expected);

            Logger.Info("File name without extension handled correctly.");
        }
    }
}

