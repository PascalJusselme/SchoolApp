using Prism.Mvvm;
using SchoolXam.Validation;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SchoolXam.Models
{
	public class AnneeScolaire : ValidatableBase
	{
		//const string NAMESREGEXPATTERN = @"\A\p{L}+([\p{Zs}\-][\p{L}]+)*\z";

		[PrimaryKey, AutoIncrement]
		public int anneeID { get; set; }

		private string _anneeLib;

		[Required(ErrorMessage = "This field is required.")]
		//[RegularExpression(NAMESREGEXPATTERN, ErrorMessage = "This field contains invalid characters.")]
		[StringLength(10, MinimumLength = 2, ErrorMessage = "This field requires a minimum of 2 characters and a maximum of 10.")]
		public string anneeLib
		{
			get { return _anneeLib; }
			set { SetProperty(ref _anneeLib, value); }
		}

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Classe> Classes { get; set; }

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Matiere> Matieres { get; set; }
	}
}
