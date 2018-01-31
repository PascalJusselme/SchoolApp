using SchoolXam.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;

namespace SchoolXam.Data
{
	public interface ISchoolRepository
	{
		#region AnneeScolaire
		AnneeScolaire GetAnnee(AnneeScolaire annee);
		List<AnneeScolaire> GetAnnees();
		void SaveAnnee(AnneeScolaire annee);
		#endregion

		#region Classe
		List<Classe> GetClassesByAnnee(AnneeScolaire annee);
		Classe GetClasse(Classe classe);
		void UpdateClasse(Classe classe);
		#endregion

		#region Matiere
		List<Matiere> GetMatieresByAnnee(AnneeScolaire annee);
		Matiere GetMatiere(Matiere matiere);
		void UpdateMatiere(Matiere matiere);
		#endregion

		#region Eleve
		List<Eleve> GetEleveByClasse(Classe classe);
		void DeleteDevoir(Devoir devoir);
		#endregion

		#region Devoir

		#endregion
	}

	public class SchoolRepository : ISchoolRepository
	{
		public SQLiteConnection _conn;

		public SchoolRepository(SQLiteConnection conn)
		{
			_conn = conn;

			_conn.CreateTable<AnneeScolaire>();
			_conn.CreateTable<Classe>();
			_conn.CreateTable<Matiere>();
			_conn.CreateTable<ClasseMatiere>();
			_conn.CreateTable<Eleve>();
			_conn.CreateTable<Devoir>();

			//DeleteDB();
		}

		#region AdminDB
		private void DeleteDB()
		{
			_conn.DropTable<AnneeScolaire>();
			_conn.DropTable<Classe>();
			_conn.DropTable<Matiere>();
			_conn.DropTable<ClasseMatiere>();
			_conn.DropTable<Eleve>();
			_conn.DropTable<Devoir>();
		}
		#endregion

		#region AnneeScolaire
		public List<AnneeScolaire> GetAnnees()
		{
			return _conn.Table<AnneeScolaire>().ToList();
		}

		public AnneeScolaire GetAnnee(AnneeScolaire annee)
		{
			return _conn.Table<AnneeScolaire>()
						.First(an => an.anneeLib == annee.anneeLib);
		}

		public void SaveAnnee(AnneeScolaire annee)
		{
			if (annee.anneeID == 0)
			{
				_conn.InsertWithChildren(annee);

				foreach (Classe classe in annee.Classes)
				{
					_conn.UpdateWithChildren(classe);

					foreach (Eleve eleve in classe.Eleves)
					{
						_conn.Insert(eleve);
					}

					foreach (Devoir devoir in classe.Devoirs)
					{
						_conn.Insert(devoir);
					}
				}

				foreach (Matiere matiere in annee.Matieres)
				{
					_conn.UpdateWithChildren(matiere);
				}
			}
			else
			{
				_conn.UpdateWithChildren(annee);

				foreach (Classe classe in annee.Classes)
				{
					if (classe.classeID == 0)
					{
						_conn.Insert(classe);
					}

					foreach (Eleve eleve in classe.Eleves)
					{
						if (eleve.eleveID ==0)
						{
							_conn.Insert(eleve);

							_conn.UpdateWithChildren(classe);
						}
					}

					foreach (Devoir devoir in classe.Devoirs)
					{
						if (devoir.devoirID == 0)
						{
							_conn.Insert(devoir);

							_conn.UpdateWithChildren(classe);
						}
					}
				}

				foreach (Matiere matiere in annee.Matieres)
				{
					if (matiere.matiereID == 0)
					{
						_conn.Insert(matiere);
					}

					_conn.UpdateWithChildren(matiere);
				}
			}
		}
		#endregion

		#region Classe
		public List<Classe> GetClassesByAnnee(AnneeScolaire annee)
		{
			return _conn.Table<Classe>().Where(cl => cl.anneeID == annee.anneeID).ToList();
		}

		public Classe GetClasse(Classe classe)
		{
			return _conn.GetWithChildren<Classe>(classe.classeID);
		}

		public void UpdateClasse(Classe classe)
		{
			_conn.UpdateWithChildren(classe);
		}
		#endregion

		#region Matiere
		public List<Matiere> GetMatieresByAnnee(AnneeScolaire annee)
		{
			return _conn.Table<Matiere>().Where(ma => ma.anneeID == annee.anneeID).ToList();
		}

		public Matiere GetMatiere(Matiere matiere)
		{
			return _conn.GetWithChildren<Matiere>(matiere.matiereID);
		}

		public void UpdateMatiere(Matiere matiere)
		{
			_conn.UpdateWithChildren(matiere);
		}
		#endregion

		#region Eleve
		public List<Eleve> GetEleveByClasse(Classe classe)
		{
			return _conn.Table<Eleve>().Where(el => el.classeID == classe.classeID).ToList();
		}

		public void DeleteDevoir(Devoir devoir)
		{
			_conn.Delete(devoir);
		}
		#endregion

		#region Devoir

		#endregion
	}
}
