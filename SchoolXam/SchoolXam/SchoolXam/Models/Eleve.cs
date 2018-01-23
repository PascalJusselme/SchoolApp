using Prism.Mvvm;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace SchoolXam.Models
{
	public class Eleve : BindableBase
	{
		[PrimaryKey, AutoIncrement]
		public int eleveID { get; set; }

		private string _nomEleve;
		public string nomEleve
		{
			get { return _nomEleve; }
			set { SetProperty(ref _nomEleve, value); }
		}
		private string _prenomEleve;
		public string prenomEleve
		{
			get { return _prenomEleve; }
			set { SetProperty(ref _prenomEleve, value); }
		}

		[ForeignKey(typeof(Classe))]
		public int classeID { get; set; }
		[ManyToOne("classeID")]
		public Classe Classe { get; set; }

		//[OneToMany(CascadeOperations = CascadeOperation.All)]
		//public List<Controle> Controles { get; set; }
	}
}
