using Prism.Mvvm;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace SchoolXam.Models
{
	public class Controle : BindableBase
	{
		[PrimaryKey, AutoIncrement]
		public int controleID { get; set; }

		public double controleNote { get; set; }
		public long controlePresence { get; set; }

		[ForeignKey(typeof(Eleve))]
		public int eleveID { get; set; }
		[ManyToOne("eleveID")]
		public Eleve Eleve { get; set; }

		[ForeignKey(typeof(Devoir))]
		public int devoirID { get; set; }
		[ManyToOne("devoirID")]
		public Devoir Devoir { get; set; }

	}
}
