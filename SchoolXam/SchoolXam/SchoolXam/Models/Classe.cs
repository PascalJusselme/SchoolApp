using Prism.Mvvm;
using SchoolXam.Validation;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolXam.Models
{
	public class Classe : ValidatableBase
	{
		public Classe()
		{
			classeLib = string.Empty;
			Matieres = new List<Matiere>();
			Devoirs = new List<Devoir>();
			Eleves = new List<Eleve>();
		}

		public Classe(AnneeScolaire annee)
		{
			classeLib = string.Empty;
			Matieres = new List<Matiere>();
			Devoirs = new List<Devoir>();
			Eleves = new List<Eleve>();
			Annee = annee;
		}

		[PrimaryKey, AutoIncrement]
		public int classeID { get; set; }

		private string _classeLib;

		[Required(ErrorMessage = "This field is required.")]
		//[RegularExpression(NAMESREGEXPATTERN, ErrorMessage = "This field contains invalid characters.")]
		[StringLength(10, MinimumLength = 2, ErrorMessage = "This field requires a minimum of 2 characters and a maximum of 10.")]
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
