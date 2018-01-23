using SchoolXam.Models;
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
		void SaveEleve(Eleve eleve);
		#endregion

		#region Devoir
		List<Devoir> GetDevoirByMatiere(Matiere matiere);
		#endregion
	}
}
