using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.UI;
using Drive.Models;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Drive.ViewModels
{
    public class ListViewItem : PropertyNotifier, ISelectable
    {
		protected ListViewItem()
		{
			ItemFrameDefault = UITheme.Current.GetStyleByName("ListView-item-frame-default");
			ItemFrameSelected = UITheme.Current.GetStyleByName("ListView-item-frame-selected");
		}

		public string[] PropertiesChangedOnSelected { get; protected set; }


		bool isSelected;
        public bool IsSelected {
            get => isSelected;
            set => SetPropertyEx(ref isSelected, value, PropertiesChangedOnSelected);
        }

        public bool ShowDetails {
            // TODO: why UneventRows doesn't work on UWP??
            //get => IsSelected;
            get {
                return Device.RuntimePlatform == Device.UWP || isSelected;
            }
        }

        //static float cornerRadius = 20;
        //public float CornerRadius {
        //    get => IsSelected ? 0 : cornerRadius;
        //}

        ////static Color backgroundColor = new Color(232.0/255.0, 233.0/255.0, 236.0/255.0);
        //static Color backgroundColor = Color.LightCyan;
        //static Color backgroundSelected = Color.LightSkyBlue;
        //public Color BackgroundColor {
        //    get => IsSelected ? backgroundSelected : backgroundColor;
        //}

        //static Color borderColor = Color.Accent;
        //public Color BorderColor {
        //    get => IsSelected ? borderColor : Color.Transparent;
        //}

		Style ItemFrameDefault { get; }
		Style ItemFrameSelected { get; }

		public Style ItemFrameStyle {
			get => IsSelected ? ItemFrameSelected : ItemFrameDefault;
		}

		public double TextSize {
            get => 20;
        }
        public double SmallTextSize {
            get => 18;
        }

    }
}
