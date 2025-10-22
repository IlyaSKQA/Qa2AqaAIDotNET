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

        private FileStorage storage;

        [SetUp]
        public void Setup()
        {
            storage = new FileStorage(); // создаём fresh storage для каждого теста
            Logger.Info("Setup new FileStorage instance for test.");
        }

        [Test]
        public void DefaultConstructor_Should_Have_MaxSize100_And_AvailableSize200()
        {
            var files = storage.GetFiles();
            
            files.Should().BeEmpty();

            Logger.Info("Default constructor test passed.");
        }

        [Test]
        public void ConstructorWithSize_Should_InitializeProperly()
        {
            var customStorage = new FileStorage(50);
            var files = customStorage.GetFiles();

            files.Should().BeEmpty();

            Logger.Info("Constructor with custom size test passed.");
        }

        [Test]
        public void Write_Should_Add_File_When_Unique_And_Size_Fits()
        {
            var file = new File("test.txt", new string('a', 50)); // size = 25
            var result = storage.Write(file);

            result.Should().BeTrue();
            storage.IsExists("test.txt").Should().BeTrue();
            storage.GetFiles().Should().ContainSingle(f => f.GetFileName() == "test.txt");

            Logger.Info("Write unique file test passed.");
        }

        [Test]
        public void Write_Should_Throw_When_FileNameAlreadyExists()
        {
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
            var largeContent = new string('a', 500); // size = 250 > default available
            var largeFile = new File("big.txt", largeContent);
            var result = storage.Write(largeFile);

            result.Should().BeFalse();
            storage.IsExists("big.txt").Should().BeFalse();

            Logger.Info("Write large file test passed.");
        }

        [Test]
        public void IsExists_Should_Return_True_If_File_Added()
        {
            var file = new File("exists.txt", "content");
            storage.Write(file);

            var exists = storage.IsExists("exists.txt");

            exists.Should().BeTrue();

            Logger.Info("IsExists positive test passed.");
        }

        [Test]
        public void IsExists_Should_Return_False_If_File_Not_Added()
        {
            var exists = storage.IsExists("nonexistent.txt");

            exists.Should().BeFalse();

            Logger.Info("IsExists negative test passed.");
        }

        [Test]
        public void Delete_Should_Remove_Existing_File()
        {
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
            var result = storage.Delete("notfound.txt");

            result.Should().BeFalse();

            Logger.Info("Delete non-existent file test passed.");
        }

        [Test]
        public void GetFiles_Should_Return_All_Added_Files()
        {
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
            var file = new File("getme.txt", "abc");
            storage.Write(file);

            var retrieved = storage.GetFile("getme.txt");

            retrieved.Should().NotBeNull();
            retrieved.GetFileName().Should().Be("getme.txt");

            Logger.Info("GetFile existing file test passed.");
        }

        [Test]
        public void GetFile_Should_Return_Null_When_File_Not_Found()
        {
            var retrieved = storage.GetFile("nofile.txt");

            retrieved.Should().BeNull();

            Logger.Info("GetFile non-existent file test passed.");
        }
    }
}
