using Prism.Mvvm;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SchoolXam.Models
{
	public class Devoir : BindableBase
	{
		[PrimaryKey, AutoIncrement]
		public int devoirID { get; set; }

		private string _devoirLib;
		public string devoirLib
		{
			get { return _devoirLib; }
			set { SetProperty(ref _devoirLib, value); }
		}

		private double _devoirCoeff;
		public double devoirCoeff
		{
			get { return _devoirCoeff; }
			set { SetProperty(ref _devoirCoeff, value); }
		}

		private double _devoirNoteMax;
		public double devoirNoteMax
		{
			get { return _devoirNoteMax; }
			set { SetProperty(ref _devoirNoteMax, value); }
		}

		public long devoirTrimestre { get; set; }

		[ForeignKey(typeof(Matiere))]
		public int matiereID { get; set; }
		[ManyToOne("matiereID")]
		public Matiere Matiere { get; set; }

		[ForeignKey(typeof(Classe))]
		public int classeID { get; set; }
		[ManyToOne("classeID")]
		public Classe Classe { get; set; }

		//[OneToMany(CascadeOperations = CascadeOperation.All)]
		//public List<Controle> Controles { get; set; }
	}
}
