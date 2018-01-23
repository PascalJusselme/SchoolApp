using Android.Content;
using SchoolXam.Abstractions;
using SchoolXam.Droid;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper_Android))]
namespace SchoolXam.Droid
{
	public class FileHelper_Android : IFileHelper
	{
		public FileHelper_Android()
		{

		}		

		public string path => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SchoolDB.sqlite3");
		
		public SQLiteConnection GetConnection()
		{
			SQLiteConnection conn = new SQLiteConnection(path);

			return conn;
		}
	}
}