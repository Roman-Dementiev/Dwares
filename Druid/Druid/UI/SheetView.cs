using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public enum SheetViewMode
	{
		Default,
		Compact,
		Custom
	}

	public class SheetView : Grid
	{
		static new ClassRef @class = new ClassRef(typeof(SheetView));

		Grid sheetHeader = new Grid();
		Grid sheetGrid = new Grid();
		Grid sideHeader = null;
		View sideView = null;
		SheetFileDefinitions fileDefinitions = null;
		SheetColumns defaultColumns = null;
		SheetColumns compactColumns = null;
		SheetColumns customColumns = null;
		SheetViewMode mode = SheetViewMode.Default;

		public SheetView()
		{
			//Debug.EnableTracing(@class);
			//Debug.TraceProperties(@class, nameof(SheetView), this, nameof(RowDefinitions));
			//Debug.TraceProperties(@class, nameof(SheetView), this, nameof(ColumnDefinitions));s

			RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
			ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) });

			Children.Add(sheetHeader, 0, 0);
			Children.Add(sheetGrid, 0, 1);
		}

		public static readonly BindableProperty ModeProperty =
			BindableProperty.Create(
				nameof(Mode),
				typeof(SheetViewMode),
				typeof(SheetView),
				defaultValue: SheetViewMode.Default,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is SheetView sheetView && newValue is SheetViewMode mode) {
						sheetView.SetMode(mode);
					}
				});

		public SheetViewMode Mode {
			set { SetValue(ModeProperty, value); }
			get { return (SheetViewMode)GetValue(ModeProperty); }
		}

		public static readonly BindableProperty FileDefinitionsProperty =
			BindableProperty.Create(
				nameof(FileDefinitions),
				typeof(SheetFileDefinitions),
				typeof(SheetView),
				defaultValue: new SheetFileDefinitions(),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is SheetView sheetView && newValue is SheetFileDefinitions fileDefinitions) {
						sheetView.SetFileDefinitions(fileDefinitions);
					}
				});

		public SheetFileDefinitions FileDefinitions {
			set => SetValue(FileDefinitionsProperty, value);
			get => (SheetFileDefinitions)GetValue(FileDefinitionsProperty);
		}

		public static readonly BindableProperty DefaultColumnsProperty =
			BindableProperty.Create(
				nameof(DefaultColumns),
				typeof(SheetColumns),
				typeof(SheetView),
				defaultValue: new SheetColumns(),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is SheetView sheetView && newValue is SheetColumns columns) {
						sheetView.SetColumns(SheetViewMode.Default, ref sheetView.defaultColumns, columns);
					}
				});

		public SheetColumns DefaultColumns {
			set { SetValue(DefaultColumnsProperty, value); }
			get { return (SheetColumns)GetValue(DefaultColumnsProperty); }
		}

		public static readonly BindableProperty CompactColumnsProperty =
			BindableProperty.Create(
				nameof(CompactColumns),
				typeof(SheetColumns),
				typeof(SheetView),
				defaultValue: new SheetColumns(),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is SheetView sheetView && newValue is SheetColumns columns) {
						sheetView.SetColumns(SheetViewMode.Compact, ref sheetView.compactColumns, columns);
					}
				});

		public SheetColumns CompactColumns {
			set { SetValue(CompactColumnsProperty, value); }
			get { return (SheetColumns)GetValue(CompactColumnsProperty); }
		}

		public static readonly BindableProperty CustomColumnsProperty =
			BindableProperty.Create(
				nameof(CustomColumns),
				typeof(SheetColumns),
				typeof(SheetView),
				defaultValue: new SheetColumns(),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is SheetView sheetView && newValue is SheetColumns columns) {
						sheetView.SetColumns(SheetViewMode.Custom, ref sheetView.customColumns, columns);
					}
				});

		public SheetColumns CustomColumns {
			set { SetValue(CustomColumnsProperty, value); }
			get { return (SheetColumns)GetValue(CustomColumnsProperty); }
		}

		public static readonly BindableProperty SideViewProperty =
			BindableProperty.Create(
				nameof(SideView),
				typeof(View),
				typeof(SheetView),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is SheetView sheetView && newValue is View sideView) {
						sheetView.SetSideView(sideView);
					}
				});

		public View SideView {
			set { SetValue(SideViewProperty, value); }
			get { return (View)GetValue(SideViewProperty); }
		}


		public SheetColumns CurrentColumns
		{
			get {
				if (mode == SheetViewMode.Compact && compactColumns != null && compactColumns.Count > 0) {
					return compactColumns;
				}
				if (mode == SheetViewMode.Custom && customColumns != null && customColumns.Count > 0) {
					return customColumns;
				}
				if (defaultColumns != null && defaultColumns.Count > 0) {
					return defaultColumns;
				}

				var columns = new SheetColumns();
				foreach (var file in FileDefinitions) {
					columns.Add(file.Tag);
				}
				return columns;
			}
		}

		private void SetMode(SheetViewMode mode)
		{
			if (mode == this.mode)
				return;

			//TODO

			this.mode = mode;

			RebuildGridColumns();
		}

		private void SetFileDefinitions(SheetFileDefinitions fileDefinitions)
		{
			if (fileDefinitions == this.fileDefinitions)
				return;

			this.fileDefinitions = fileDefinitions;

			RebuildGridColumns();
		}

		private void SetColumns(SheetViewMode mode, ref SheetColumns columns, SheetColumns newColumns)
		{
			if (newColumns == columns)
				return;

			// TODO

			columns = newColumns;

			if (mode == this.mode) {
				RebuildGridColumns();
			}
		}

		private void SetSideView(View sideView)
		{
			if (sideView == this.sideView)
				return;

			//TODO

			this.sideView = sideView;
		}

		public void RebuildGridColumns()
		{
			Debug.Print("SheetView.RebuildGridColumns()");

			sheetHeader.ColumnDefinitions.Clear();	
			sheetHeader.Children.Clear();
			sheetGrid.ColumnDefinitions.Clear();

			var columns = CurrentColumns;
			var colIndex = 0;
			foreach (var tag in columns) {
				var file = GetFile(tag);
				if (file == null)
					continue;

				sheetHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = file.Width });
				sheetGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = file.Width });
				
				sheetHeader.Children.Add(new Label { Text = file.Header }, colIndex, 0);
				
				colIndex++;
			}
		}

		private SheetFile GetFile(string tag)
		{
			foreach (var file in FileDefinitions) {
				if (file.Tag == tag)
					return file;
			}
			return null;
		}
	}

	public class SheetFile
	{
		public string Tag { get; set; }
		public string Header { get; set; }
		public GridLength Width { get; set; }
	}

	public class SheetFileDefinitions : List<SheetFile>
	{
		public SheetFileDefinitions() {}
	}

	public class SheetColumns: List<string>
	{
		public SheetColumns() { }
	}
}
