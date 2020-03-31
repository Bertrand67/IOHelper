using NUnit.Framework;
using System;
using System.IO;

namespace IOHelper.Test
{
	public class DirectoryInfoExtensionsTest
	{
		private DirectoryInfo GetTestDirectory()
		{
			return new DirectoryInfo(Path.Combine(Path.GetTempPath(), @"DirectoryInfoExtensionsTest"));
		}

		private int _count;

		private string GetDifferentName()
		{
			return $"test{this._count++}";
		}

		private DirectoryInfo CreateDirectoryInside(DirectoryInfo directoryInfo)
		{
			var result = new DirectoryInfo(Path.Combine(directoryInfo.FullName, this.GetDifferentName()));
			result.Create();
			return result;
		}

		private FileInfo CreateFileInside(DirectoryInfo directoryInfo)
		{
			var result = new FileInfo(Path.Combine(directoryInfo.FullName, this.GetDifferentName()));
			using (var fileStream = result.Create())
			{
				fileStream.Close();
			}
			return result;
		}

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			this.GetTestDirectory().Create();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			this.GetTestDirectory().Delete(true);
		}

		[Test]
		public void LastWriteTimeWithin_NoFileSystemInfoWithin_ReturnsDirectoryInfoLastWriteTime()
		{
			DirectoryInfo instance = this.CreateDirectoryInside(this.GetTestDirectory());

			Assert.That(instance.LastWriteTimeWithin(), Is.EqualTo(instance.LastWriteTime));
		}

		[Test]
		public void LastWriteTimeWithin_OneSubdirectoryWithin_ReturnsSubdirectoryLastWriteTime()
		{
			DirectoryInfo instance = this.CreateDirectoryInside(this.GetTestDirectory());
			DirectoryInfo subdirectory = this.CreateDirectoryInside(instance);
			subdirectory.LastWriteTime = DateTime.Now.AddMinutes(1);

			Assert.That(instance.LastWriteTimeWithin(), Is.EqualTo(subdirectory.LastWriteTime));
		}

		[Test]
		public void LastWriteTimeWithin_OneFileInsideSubdirectoryWithin_ReturnsFileLastWriteTime()
		{
			DirectoryInfo instance = this.CreateDirectoryInside(this.GetTestDirectory());
			DirectoryInfo subdirectory = this.CreateDirectoryInside(instance);
			FileInfo file = this.CreateFileInside(subdirectory);
			file.LastWriteTime = DateTime.Now.AddMinutes(2);

			Assert.That(instance.LastWriteTimeWithin(), Is.EqualTo(file.LastWriteTime));
		}
	}
}