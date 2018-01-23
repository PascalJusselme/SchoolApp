using Prism.Mvvm;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SchoolXam.Models
{
	public class AnneeScolaire : BindableBase
	{
		[PrimaryKey, AutoIncrement]
		public int anneeID { get; set; }

		private string _anneeLib;
		public string anneeLib
		{
			get { return _anneeLib; }
			set
			{
				SetProperty(ref _anneeLib, value);
			}
		}

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Classe> Classes { get; set; }

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Matiere> Matieres { get; set; }
	}
}
