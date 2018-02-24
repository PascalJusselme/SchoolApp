using Prism.Mvvm;
using Prism.Navigation;
using SchoolXam.Data;
using SchoolXam.Models;
using System.Collections.Generic;

namespace SchoolXam.ViewModels
{
	public class BaseViewModel : BindableBase, INavigationAware, IDestructible
	{
		protected readonly ISchoolRepository _rep;

		public BaseViewModel(SchoolRepository db)
		{
			_rep = db;
		}

		public virtual void OnNavigatedFrom(NavigationParameters parameters) { }

		public virtual void OnNavigatedTo(NavigationParameters parameters) { }

		public virtual void OnNavigatingTo(NavigationParameters parameters) { }

		public virtual void Destroy() { }

		public AnneeScolaire Load_Annee(AnneeScolaire annee)
		{
			if (annee.anneeID != 0)
			{
				annee = _rep.Get_Annee(annee);

				annee.Classes = _rep.GetClassesByAnnee(annee);
				annee.Matieres = _rep.Get_MatieresByAnnee(annee);

				foreach (Classe classe in annee.Classes)
				{
					Classe cl = _rep.GetClasseWithChildren(classe);

					classe.Annee = annee;
					classe.Matieres = new List<Matiere>(cl.Matieres);
					classe.Eleves = new List<Eleve>(cl.Eleves);
					classe.Devoirs = new List<Devoir>(cl.Devoirs);

					Load_Devoir(classe);
				}

				foreach (Matiere matiere in annee.Matieres)
				{
					Matiere ma = new Matiere();
					ma = _rep.Get_MatiereWithChildren(matiere);

					matiere.Annee = annee;
					matiere.Classes = new List<Classe>(ma.Classes);
					matiere.Devoirs = new List<Devoir>(ma.Devoirs);

					Load_Devoir(matiere);
				}
			}

			return annee;
		}

		public void Load_Devoir(Classe classe)
		{
			foreach (Devoir devoir in classe.Devoirs)
			{
				Devoir dev = _rep.Get_DevoirWithChildren(devoir);

				devoir.Classe = dev.Classe;
				devoir.Matiere = dev.Matiere;
			}
		}

		public void Load_Devoir(Matiere matiere)
		{
			foreach (Devoir devoir in matiere.Devoirs)
			{
				Devoir dev = _rep.Get_DevoirWithChildren(devoir);

				devoir.Classe = dev.Classe;
				devoir.Matiere = dev.Matiere;
			}
		}

		public void Delete_Devoir_OnDesattribClasseMatiere(Classe classe, Matiere matiere)
		{
			Devoir dev = matiere.Devoirs.Find(d => d.Matiere.matiereLib == matiere.matiereLib);
			Devoir devoir = classe.Devoirs.Find(d => d.Classe.classeLib == classe.classeLib);
			if (dev != null && devoir != null)
			{
				classe.Devoirs.Remove(devoir);
				matiere.Devoirs.Remove(devoir);
				// PENSER A METTRE PAGE DE DIALOG DE VALIDATION
				if (devoir.devoirID != 0)
				{
					_rep.Delete_Devoir(dev);
				}
			}
		}
	}
}
