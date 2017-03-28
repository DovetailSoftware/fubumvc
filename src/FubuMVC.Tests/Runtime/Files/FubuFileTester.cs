using System;
using System.IO;
using FubuMVC.Core.Runtime.Files;
using NUnit.Framework;
using FubuTestingSupport;
using Rhino.Mocks;
using FubuCore;

namespace FubuMVC.Tests.Runtime.Files
{
    [TestFixture]
    public class FubuFileTester
    {
        [SetUp]
        public void SetUp()
        {
            var dir = Path.GetDirectoryName(typeof(FubuFileTester).Assembly.CodeBase);
            dir = new System.Uri(dir).LocalPath;
            Directory.SetCurrentDirectory(dir);
        }

        [Test]
        public void read_contents()
        {
            var file = new FubuFile(Path.Combine("Runtime", "Files", "Data", "a.txt"), ContentFolder.Application);
            file.ReadContents().Trim().ShouldEqual("some text from a.txt");
        }

        [Test]
        public void read_lines()
        {
            var file = new FubuFile(Path.Combine("Runtime", "Files", "Data", "a.txt"), ContentFolder.Application);
            var action = MockRepository.GenerateMock<Action<string>>();

            file.ReadLines(action);

            action.AssertWasCalled(x => x.Invoke("some text from a.txt"));
        }

        [Test]
        public void read_contents_by_stream()
        {
            var wasCalled = false;
            var file = new FubuFile(Path.Combine("Runtime", "Files", "Data", "a.txt"), ContentFolder.Application);
            file.ReadContents(stream =>
            {
                wasCalled = true;
                stream.ReadAllText().ShouldEqual("some text from a.txt");
            });

            wasCalled.ShouldBeTrue();
        }

    }
}