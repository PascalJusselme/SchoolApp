using Prism.Mvvm;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SchoolXam.Models
{
	public class Matiere : BindableBase
	{
		[PrimaryKey,AutoIncrement]
		public int matiereID { get; set; }

		private string _matiereLib;
		public string matiereLib
		{
			get { return _matiereLib; }
			set { SetProperty(ref _matiereLib, value); }
		}

		[ForeignKey(typeof(AnneeScolaire))]
		public int anneeID { get; set; }
		[ManyToOne("anneeID")]
		public AnneeScolaire Annee { get; set; }

		[ManyToMany(typeof(ClasseMatiere), CascadeOperations = CascadeOperation.All)]
		public List<Classe> Classes { get; set; }

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Devoir> Devoirs { get; set; }
	}
}
