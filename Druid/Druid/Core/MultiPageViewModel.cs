using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid
{
	public enum MultiPageTitleMode
	{
		MainOnly,
		PageOnly,
		Combined
	};


	public class MultiPageViewModel : ViewModel
	{
		public MultiPageViewModel(BindingScope parentScope) :
			base(parentScope)
		{
			PageViewModelTypes = new List<Type>();
		}

		//TODO: Make it ObservableCollection ?
		public List<Type> PageViewModelTypes { get; }
		

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
