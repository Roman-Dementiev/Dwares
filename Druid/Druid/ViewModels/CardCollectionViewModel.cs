using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;

namespace Dwares.Druid.ViewModels
{
	public class CardCollectionViewModel<TSource, TCard> : CollectionViewModel<TCard>
		where TSource : class, IModel
		where TCard : ShadowModel<TSource>, new()
	{
		//static ClassRef @class = new ClassRef(typeof(CardCollectionViewModel));

		public CardCollectionViewModel() { }


		public CardCollectionViewModel(ObservableCollection<TSource> source, Func<TSource, TCard> cardFactory = null)
		{
			//Debug.EnableTracing(@class);
			Items = new ShadowCollection<TCard, TSource>(source, cardFactory ?? DefaultFactory);
		}

		public new ShadowCollection<TCard, TSource> Items
		{
			get => base.Items as ShadowCollection<TCard, TSource>; 
			
			protected set {
				if (value != base.Items) {
					base.Items = value;
					if (HasPlaceholder) {
						value.Placeholder = Placeholder;
					} else {
						value.Placeholder = null;
					}
				}
			} 
		}

		public bool HasPlaceholder {
			get => hasPlaceholder;
			set {
				if (SetProperty(ref hasPlaceholder, value) && Items != null) {
					if (value) {
						Items.Placeholder = Placeholder;
					} else {
						Items.Placeholder = null;
					}
				}
			}
		}
		bool hasPlaceholder;

		TCard Placeholder {
			get => placeholder ??= new TCard();
			//set {
			//	if (SetProperty(ref placeholder, value) && HasPlaceholder && Items != null) {
			//		Items.Placeholder = null;
			//		Items.Placeholder = value;
			//	}
			//}
		}
		TCard placeholder;

		protected void AddItem(TSource item)
		{
			if (HasPlaceholder) {
				HasPlaceholder = false;
				Items.Source.Add(item);
				HasPlaceholder = true;
			} else {
				Items.Source.Add(item);
			}
		}

		public static TCard DefaultFactory(TSource source)
		{
			var card = new TCard();
			card.Source = source;
			return card;
		}
	}
}
