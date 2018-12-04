using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Passket.ViewModels;


namespace Passket.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecordEditPage : ContentPageEx
	{
		public RecordEditPage()
		{
			InitializeComponent();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (BindingContext is RecordEditModel viewModel) {
				PopulateView(viewModel);
			}
		}

		void PopulateView(RecordEditModel viewModel)
		{
			var record = viewModel.Source;
			var factory = Factory.Instance;
			var children = layout.Children;
			children.Clear();

			var label = new Label {
				Text = "Name",
				FontAttributes = FontAttributes.Bold
			};
			View view = new Entry {
				Text = record.Name
			};
			children.Add(label);
			children.Add(view);

			foreach (var entry in record.Entries) {
				bool needLabel = true;
				view = factory.CreateEdit(entry, ref needLabel);
				if (view == null)
					continue;

				if (needLabel) {
					label = new Label {
						Text = entry.Name,
						FontAttributes = FontAttributes.Bold
					};
					children.Add(label);
				}
				children.Add(view);
			}
		}
	}
}