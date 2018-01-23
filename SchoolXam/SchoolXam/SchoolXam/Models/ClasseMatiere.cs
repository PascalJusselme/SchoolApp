using SQLiteNetExtensions.Attributes;

namespace SchoolXam.Models
{
	public class ClasseMatiere
	{
		[ForeignKey(typeof(Classe))]
		public int classeID { get; set; }

		[ForeignKey(typeof(Matiere))]
		public int matiereID { get; set; }
	}
}