using System;
using Dwares.Dwarf;
using Beylen.Models;
using Xamarin.Forms;
using System.Collections.Generic;
using Dwares.Druid.Satchel;
using Dwares.Druid.UI;

namespace Beylen.ViewModels
{
	public class ArticleCardModel : CardViewModel<Article>
	{
		//static ClassRef @class = new ClassRef(typeof(ArticleCardModel));

		public ArticleCardModel(Article source) :
			base(source)
		{
			//Debug.EnableTracing(@class);
			ExpandSelected = false;

			UpdateFromSource();
		}

		public bool IsArticle => Source != null;
		public bool IsPlaceholder => Source == null;

		public bool IsCollapsed {
			get => isCollapsed;
			set => SetProperty(ref isCollapsed, value, setModified: false);
		}
		bool isCollapsed;

		public string IsChecked {
			get => isChecked;
			set => SetProperty(ref isChecked, value);
		}
		string isChecked;

		public string ProduceName {
			get => produceName;
			set => SetProperty(ref produceName, value);
		}
		string produceName;

		public FormattedString FormattedQty {
			get => formattedQty;
			set => SetProperty(ref formattedQty, value);
		}
		FormattedString formattedQty;

		public string PackingName {
			get => packingName;
			set {
				if (SetProperty(ref packingName, value)) {
					if (!string.IsNullOrEmpty(value))
						IsCounts = false;
					UpdateFormattedCty();
				}
			}
		}
		string packingName;

		public decimal Quantity {
			get => quantity;
			set {
				if (SetProperty(ref quantity, value)) {
					if (quantity <= 9) {
						quantityString = string.Empty;
					} else  if (Math.Truncate(quantity) == quantity) {
						quantityString = ((int)quantity).ToString();
					} else {
						quantityString = quantity.ToString();
					}
					FirePropertyChanged(nameof(QuantityString));
					UpdateFormattedCty();
				}
			}
		}
		decimal quantity;

		public string QuantityString {
			get => quantityString;
			set {
				if (SetProperty(ref quantityString, value)) {
					decimal val;
					if (decimal.TryParse(quantityString, out val) && val > 0) {
						quantity = val;
					} else {
						quantity = 0;
					}
					FirePropertyChanged(nameof(Quantity));
					UpdateFormattedCty();
				}
			}
		}
		string quantityString;

		public string Note {
			get => note;
			set => SetProperty(ref note, value);
		}
		string note;

		public bool IsCounts {
			get => isCounts;
			set {
				if (SetPropertyEx(ref isCounts, value, nameof(IsCounts), nameof(IsPackage))) {
					if (value) {
						PackingName = string.Empty;
					}
					UpdateFormattedCty();
				}
			}
		}
		bool isCounts = false;

		public bool IsPackage {
			get => !IsCounts;
			set => IsCounts = !value;
		}

		public Command AddArticleCommand => InvoiceFormModel.AddArticleCommand;

		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();

			if (Source != null) {
				IsExpanded = IsSelected;
				IsCollapsed = !IsSelected;
			}
		}

		protected override void UpdateFromSource()
		{
			if (InvoiceFormModel.UpdatingSource)
				return;

			if (Source != null) {
				ProduceName = Source.Produce?.Name ?? string.Empty;
				Quantity = Source.Quantity;
				if (Source.Unit == "ct") {
					PackingName = string.Empty;
					IsCounts = true;
				} else {
					//Packing = Packing.Get(Source.Unit);
					PackingName = Source.Unit;
					IsCounts = false;
				}
				Note = Source.Note;
				IsCollapsed = !IsExpanded;
			} else {
				ProduceName = PackingName = Note = string.Empty;
				Quantity = 0;
				IsCounts = false;
				IsExpanded = false;
				IsCollapsed = false;
			}

			IsModified = false;
		}

		public static bool Equal(decimal v1, decimal v2, decimal epsilon = 0.03m)
		{
			return Math.Abs(v1-v2) <= epsilon;
		}

		void UpdateFormattedCty()
		{
			var fqty = new FormattedString();

			if (Quantity > 0)
			{
				string quant;
				bool isFraction = false;
				if (IsCounts || Equal(Math.Truncate(Quantity), Quantity)) {
					quant = ((int)Quantity).ToString();
				} else if (Equal(quantity, 0.5m)) {
					quant = StdGlyph.FractionOneHalf;
					isFraction = true;
				} else if (Equal(quantity, 0.33m)) {
					quant = StdGlyph.FractionOneThird;
					isFraction = true;
				} else if (Equal(quantity, 0.63m)) {
					quant = StdGlyph.FractionTwoThirds;
					isFraction = true;
				} else if (Equal(quantity, 0.25m)) {
					quant = StdGlyph.FractionOneQuarter;
					isFraction = true;
				} else if (Equal(quantity, 0.75m)) {
					quant = StdGlyph.FractionThreeQuarters;
					isFraction = true;
				} else {
					quant = Quantity.ToString("F1");
				}

				string units = null;
				if (IsCounts) {
					units = " ct";
				} else if (!string.IsNullOrEmpty(PackingName)) {
					units = PackingName?.ToLower();

					var packing = Packing.Get(units);
					if (packing != null && quant != "1" && !isFraction) {
						units = packing.Plural;
					}
				}

				if (isFraction) {
					fqty.Spans.Add(new SpanEx { Text = quant, Flavor = "Invoice-fraction-span" });
				} else {
					fqty.Spans.Add(new Span { Text = quant });
				}
				if (units != null) {
					fqty.Spans.Add(new Span { Text = " " + units });
				}
			}

			FormattedQty = fqty;
		}

		public List<object> ProduceSuggestions => Suggestions.Produce;

		public object ChoosenProduceSuggestion {
			get => choosenProduceSuggestion;
			set {
				choosenProduceSuggestion = value;
				if (value is ProduceSuggestion suggestion) {
					ProduceName = suggestion.Produce.Name;
				} else {
					ProduceName = String.Empty;
				}
				IsModified = true;
			}
		}
		object choosenProduceSuggestion;

		//public List<object> PackingsSuggestions => Suggestions.Packings;

		//public object ChoosenPakingSuggestion {
		//	get => choosenPackingSuggestion;
		//	set {
		//		choosenPackingSuggestion = value;
		//		if (value is PackingSuggestion suggestion) {
		//			//Packing = suggestion.Packing;
		//		} else {
		//			//Packing = null;
		//		}
		//		IsModified = true;
		//	}
		//}
		//object choosenPackingSuggestion;
	}
}
