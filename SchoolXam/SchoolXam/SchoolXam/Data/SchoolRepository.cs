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
		AnneeScolaire Get_Annee(AnneeScolaire annee);
		List<AnneeScolaire> Get_ListAnnees();
		void SaveAnnee(AnneeScolaire annee);
		#endregion

		#region Classe
		List<Classe> GetClassesByAnnee(AnneeScolaire annee);
		Classe GetClasseWithChildren(Classe classe);
		#endregion

		#region Matiere
		List<Matiere> Get_MatieresByAnnee(AnneeScolaire annee);
		Matiere Get_MatiereWithChildren(Matiere matiere);
		#endregion

		#region Eleve
		List<Eleve> Get_EleveByClasse(Classe classe);
		#endregion

		#region Devoir
		void Delete_Devoir(Devoir devoir);
		Devoir Get_DevoirWithChildren(Devoir devoir);
		List<Classe> Get_ClassesWithChildrenByAnnee(AnneeScolaire annee);
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
		public List<AnneeScolaire> Get_ListAnnees()
		{
			return _conn.Table<AnneeScolaire>().ToList();
		}

		public AnneeScolaire Get_Annee(AnneeScolaire annee)
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

					_conn.InsertOrReplaceAllWithChildren(classe.Eleves);

					_conn.InsertOrReplaceAllWithChildren(classe.Devoirs);
				}
			}
			else
			{
				_conn.UpdateWithChildren(annee);

				foreach (Classe classe in annee.Classes)
				{
					_conn.UpdateWithChildren(classe);

					_conn.InsertOrReplaceAllWithChildren(classe.Eleves);

					_conn.InsertOrReplaceAllWithChildren(classe.Devoirs);
				}
			}
		}
		#endregion

		#region Classe
		public List<Classe> GetClassesByAnnee(AnneeScolaire annee)
		{
			return _conn.Table<Classe>().Where(cl => cl.anneeID == annee.anneeID).ToList();
		}

		public List<Classe> Get_ClassesWithChildrenByAnnee(AnneeScolaire annee)
		{
			return _conn.GetAllWithChildren<Classe>(m => m.anneeID == annee.anneeID);
		}

		public Classe GetClasseWithChildren(Classe classe)
		{
			return _conn.GetWithChildren<Classe>(classe.classeID);
		}
		#endregion

		#region Matiere
		public List<Matiere> Get_MatieresByAnnee(AnneeScolaire annee)
		{
			return _conn.Table<Matiere>().Where(ma => ma.anneeID == annee.anneeID).ToList();
		}

		//public List<Matiere> Get_MatieresWithChildrenByAnnee(AnneeScolaire annee)
		//{
		//	return _conn.GetAllWithChildren<Matiere>(m=>m.anneeID == annee.anneeID);
		//}

		public Matiere Get_MatiereWithChildren(Matiere matiere)
		{
			return _conn.GetWithChildren<Matiere>(matiere.matiereID);
		}

		//public void Update_MatiereWithChildren(Matiere matiere)
		//{
		//	_conn.UpdateWithChildren(matiere);
		//}
		#endregion

		#region Eleve
		public List<Eleve> Get_EleveByClasse(Classe classe)
		{
			return _conn.Table<Eleve>().Where(el => el.classeID == classe.classeID).ToList();
		}
		#endregion

		#region Devoir
		public void Delete_Devoir(Devoir devoir)
		{
			_conn.Delete(devoir);
		}

		public Devoir Get_DevoirWithChildren(Devoir devoir)
		{
			return _conn.GetWithChildren<Devoir>(devoir.devoirID);
		}
		#endregion
	}
}
