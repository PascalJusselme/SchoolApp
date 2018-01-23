using SQLite;

namespace SchoolXam.Abstractions
{
	public interface IFileHelper
    {
		SQLiteConnection GetConnection();
		string path { get; }
    }
}
