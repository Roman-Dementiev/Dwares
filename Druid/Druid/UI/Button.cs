//using System;
//using System.ComponentModel;
//using Dwares.Dwarf;
//using Dwares.Druid.Support;


//namespace Dwares.Druid.UI
//{
//	public class Button : Xamarin.Forms.Button
//	{
//		public Button()
//		{
//			this.PropertyChanged += OnPropertyChanged;

//			//Uid = Xaml.Bind.GetUid(this);
//			//Debug.Print("Button.Button(), Uid={0}", uid);
//		}

//		//string uid;
//		public string Uid {
//			get => Bind.GetUid(this);
//			set => Bind.SetUid(this, value);
//			//set {
//			//	if (value != Bind.GetUid(this)) {
//			//		//uid = value;
//			//		//imageSource.Uid = value;
//			//		//Image = imageSource.Value;
//			//		Command = new RelayCommand(value) { AlwaysEnabled = false };
//			//	}
//			//}
//		}

//		void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
//		{
//			//Debug.Print("Button.OnPropertyChanged(), PropertyName={0}", e.PropertyName);
//			if (e.PropertyName == Bind.UidProperty.PropertyName) {
//				var uid = Bind.GetUid(this);
//				Debug.Print("Button.Button(): PropertyName={0}, Uid={1}", e.PropertyName, uid);
//				Command = new RelayCommand(uid);
//			}
//		}
//	}
//}
