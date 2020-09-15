using System;
using System.Collections.Generic;
using Dwares.Dwarf;


namespace Dwares.Druid.ViewModels
{
	public enum MultiPageTitleMode
	{
		MainOnly,
		PageOnly,
		Combined
	};


	public class MultiPageViewModel : ViewModel
	{
		public MultiPageViewModel() : this(ApplicationScope) { }

		public MultiPageViewModel(BindingScope parentScope) :
			base(parentScope)
		{
			ViewModelTypes = new List<Type>();
		}

		//TODO: Make it ObservableCollection ?
		public List<Type> ViewModelTypes { get; }
		public bool ContentViewModels { get; protected set; }


		MultiPageTitleMode titleMode;
		public MultiPageTitleMode TitleMode {
			get => titleMode;
			set {
				if (SetProperty(ref titleMode, value)) {
					AdjustTitle();
				}
			}
		}

		public string mainTitle;
		public string MainTitle {
			get => mainTitle;
			set {
				if (SetProperty(ref mainTitle, value)) {
					if (String.IsNullOrEmpty(Title)) {
						AdjustTitle();
					}
				}
			}
		}

		public string pageTitle;
		public string PageTitle {
			get => pageTitle;
			set {
				if (SetProperty(ref pageTitle, value)) {
					AdjustTitle();
				}
			}
		}

		protected virtual void AdjustTitle()
		{
			if (TitleMode == MultiPageTitleMode.MainOnly || String.IsNullOrEmpty(PageTitle)) {
				Title = MainTitle ?? string.Empty;
			}
			else if (TitleMode == MultiPageTitleMode.PageOnly || String.IsNullOrEmpty(MainTitle)) {
				Title = PageTitle ?? string.Empty;
			}
			else {
				Title = MainTitle + ": " + PageTitle;
			}
		}
	}
}
