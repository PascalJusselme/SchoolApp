using SchoolXam.Abstractions;
using SchoolXam.UWP;
using SQLite;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper_UWP))]
namespace SchoolXam.UWP
{
	public class FileHelper_UWP : IFileHelper
	{
		public FileHelper_UWP()
		{

		}

		public string path => Path.Combine(ApplicationData.Current.LocalFolder.Path, "SchoolDB.sqlite3");
		
		public SQLiteConnection GetConnection()
		{
			string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "SchoolDB.sqlite3");

			SQLiteConnection conn = new SQLiteConnection(path);
			return conn;
		}
	}
}
