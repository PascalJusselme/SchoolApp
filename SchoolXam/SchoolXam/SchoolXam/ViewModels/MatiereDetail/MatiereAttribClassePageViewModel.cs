using Prism;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SchoolXam.ViewModels
{
	public class MatiereAttribClassePageViewModel : BaseViewModel, IActiveAware
	{
		#region Matiere Properties and Commands

		#region Matiere Properties
		private Matiere _matiere;
		public Matiere Matiere
		{
			get { return _matiere; }
			set { SetProperty(ref _matiere, value); }
		}

		private List<SelectableData<Classe>> _classeAttribuableToMatiere;
		public List<SelectableData<Classe>> ClasseAttribuableToMatiere
		{
			get { return _classeAttribuableToMatiere; }
			set { SetProperty(ref _classeAttribuableToMatiere, value); }
		}

		private bool _isVisible_CountAnneeClasse;
		public bool IsVisible_CountAnneeClasse
		{
			get { return _isVisible_CountAnneeClasse; }
			set { SetProperty(ref _isVisible_CountAnneeClasse, value); }
		}

		private string _lbl_IsVisible_lstClasseAttribuable;
		public string Lbl_IsVisible_lstClasseAttribuable
		{
			get { return _lbl_IsVisible_lstClasseAttribuable; }
			set { SetProperty(ref _lbl_IsVisible_lstClasseAttribuable, value); }
		}
		#endregion

		#region Matiere Commands
		public DelegateCommand AttribClasseToMatiereCommand => new DelegateCommand(AttribClasseToMatiere);
		#endregion

		#endregion

		public MatiereAttribClassePageViewModel(SchoolRepository db) : base(db)
		{

		}

		#region Matiere Methods
		private void Refresh_UIMatiere_ClasseAttribPart(Matiere matiere)
		{
			if (matiere.Annee.Classes.Count != 0)
			{
				Lbl_IsVisible_lstClasseAttribuable = $"Libellé des Classes Attribuables à la Matière";
				IsVisible_CountAnneeClasse = true;
			}
			else
			{
				Lbl_IsVisible_lstClasseAttribuable = $"Il n'y a pas de Classes dans l'Année en cours.";
				IsVisible_CountAnneeClasse = false;
			}


		}

		private void AttribClasseToMatiere()
		{
			Matiere.Classes.Clear();

			foreach (var data in ClasseAttribuableToMatiere)
			{
				Classe classe = data.Data;

				if (data.Selected)
				{
					Matiere.Classes.Add(data.Data);

					if (!classe.Matieres.Exists(ma => ma.matiereLib == Matiere.matiereLib))
					{
						classe.Matieres.Add(Matiere);
					}
				}
				else
				{
					// Implémenter PageDialog pour SIGNALER la SUPPRESSION
					// des DEVOIRS liés à la DÉSATTRIBUTION en cours

					Matiere.Classes.Remove(data.Data);

					if (classe.Matieres.Exists(ma => ma.matiereLib == Matiere.matiereLib))
					{
						classe.Matieres.Remove(Matiere);

						Delete_Devoir_OnDesattribClasseMatiere(classe, Matiere);
					}
				}
			}
		}

		private void LoadLstClasseAttribuable(Matiere matiere)
		{
			//Load ListClasseAttribuable with Attribuate Classe pre-load
			ClasseAttribuableToMatiere = new List<SelectableData<Classe>>();

			foreach (Classe classe in matiere.Annee.Classes)
			{
				ClasseAttribuableToMatiere.Add(new SelectableData<Classe>() { Data = classe });
			}

			RefreshLstClasseAttribuable(matiere);
		}

		private void RefreshLstClasseAttribuable(Matiere matiere)
		{
			foreach (Classe classe in matiere.Annee.Classes)
			{
				if (classe.Matieres
						  .Exists(m => m.matiereLib == matiere.matiereLib)
					&& matiere.Classes
							  .Exists(c => c.classeLib == classe.classeLib))
				{
					ClasseAttribuableToMatiere.Find(d => d.Data.classeLib == classe.classeLib)
											  .Selected = true;
				}
				else
				{
					ClasseAttribuableToMatiere.Find(d => d.Data.classeLib == classe.classeLib)
												  .Selected = false;
				}
			}
		}

		private void Delete_Devoir_OnDesattribClasseMatiere(Classe classe, Matiere matiere)
		{
			Devoir dev = matiere.Devoirs.Find(d => d.Matiere.matiereLib == matiere.matiereLib);
			Devoir devoir = classe.Devoirs.Find(d => d.Classe.classeLib == classe.classeLib);
			if (dev != null && devoir != null)
			{
				classe.Devoirs.Remove(devoir);
				matiere.Devoirs.Remove(devoir);
				// Vérifier si vraiment besoin
				if (devoir.devoirID != 0)
				{
					_rep.Delete_Devoir(dev);
				}
			}
		}
		#endregion

		#region IActiveAware Implementation
		public event EventHandler IsActiveChanged;

		protected static bool HasInitialized { get; set; }

		private bool isActive;
		public bool IsActive
		{
			get => isActive;
			set => SetProperty(ref isActive, value, RaiseIsActiveChanged);
		}

		private void RaiseIsActiveChanged()
		{
			Refresh_UIMatiere_ClasseAttribPart(Matiere);

			LoadLstClasseAttribuable(Matiere);
		}

		#endregion

		#region INavigationService Implementation
		public override void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public override void OnNavigatedTo(NavigationParameters parameters)
		{

		}

		public override void OnNavigatingTo(NavigationParameters parameters)
		{
			if (parameters != null && parameters.ContainsKey("Matiere"))
			{
				Matiere = parameters["Matiere"] as Matiere;
			}
		}

		public override void Destroy()
		{

		}
		#endregion

	}
}
