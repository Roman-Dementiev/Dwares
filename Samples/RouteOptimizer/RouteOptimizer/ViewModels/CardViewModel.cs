using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid;
using Dwares.Druid.ViewModels;


namespace RouteOptimizer.ViewModels
{
	public enum NewCard { _ }

	public class CardViewModel<TSource> : ShadowModel<TSource> where TSource : class, IModel
	{
		//static ClassRef @class = new ClassRef(typeof(CardViewModel));

		public CardViewModel() { }

		//public CardViewModel(NewCard newCard)
		//{
		//	Source = new TSource();
		//	IsNewCard = true;
		//}

		public CardViewModel(TSource source) :
			base(source)
		{
			//Debug.EnableTracing(@class);
		}

		public bool IsPlaceholder {
			get => Source == null;
		}

		public bool IsRegular {
			get => !IsPlaceholder && !IsEditing;
		}

		public bool IsEditing {
			get => isEditing;
			set => SetPropertyEx(ref isEditing, value, nameof(IsEditing), nameof(IsRegular));
		}
		bool isEditing;

		public bool IsNewCard {
			get => isNewCard;
			set => SetProperty(ref isNewCard, value);
		}
		bool isNewCard;


		public string CardFrameFlavor {
			get => cardFrameFlavor;
			set => SetProperty(ref cardFrameFlavor, value, setModified: false);
		}
		string cardFrameFlavor;

		protected override void OnSelectedChanged()
		{
			CardFrameFlavor = IsSelected ? "Card-frame-selected" : "Card-frame-default";
		}
	}
}
