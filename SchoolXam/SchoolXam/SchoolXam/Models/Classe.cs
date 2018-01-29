using Prism.Mvvm;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace SchoolXam.Models
{
	public class Classe : BindableBase
	{
		[PrimaryKey, AutoIncrement]
		public int classeID { get; set; }

		private string _classeLib;
		public string classeLib
		{
			get { return _classeLib; }
			set { SetProperty(ref _classeLib, value); }
		}

		[ForeignKey(typeof(AnneeScolaire))]
		public int anneeID { get; set; }
		[ManyToOne("anneeID")]
		public AnneeScolaire Annee { get; set; }

		[ManyToMany(typeof(ClasseMatiere),CascadeOperations = CascadeOperation.All)]
		public List<Matiere> Matieres { get; set; }

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Eleve> Eleves { get; set; }

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Devoir> Devoirs { get; set; }
	}
}
