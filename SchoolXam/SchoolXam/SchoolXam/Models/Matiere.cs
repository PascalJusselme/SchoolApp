using Prism.Mvvm;
using SchoolXam.Validation;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolXam.Models
{
	public class Matiere : ValidatableBase
	{
		public Matiere()
		{
			matiereLib = string.Empty;
			Classes = new List<Classe>();
			Devoirs = new List<Devoir>();
		}

		public Matiere(AnneeScolaire annee)
		{
			matiereLib = string.Empty;
			Classes = new List<Classe>();
			Devoirs = new List<Devoir>();
			Annee = annee;
		}

		[PrimaryKey, AutoIncrement]
		public int matiereID { get; set; }

		private string _matiereLib;

		[Required(ErrorMessage = "This field is required.")]
		//[RegularExpression(NAMESREGEXPATTERN, ErrorMessage = "This field contains invalid characters.")]
		[StringLength(10, MinimumLength = 2, ErrorMessage = "This field requires a minimum of 2 characters and a maximum of 10.")]
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
