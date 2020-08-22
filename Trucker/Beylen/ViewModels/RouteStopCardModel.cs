using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Druid;
using Xamarin.Forms;
using Beylen.Models;
using Dwares.Druid.Satchel;

namespace Beylen.ViewModels
{
	public class RouteStopCardModel : CardViewModel<RouteStop>
	{
		//static ClassRef @class = new ClassRef(typeof(RouteStopCardModel));

		public RouteStopCardModel(RouteStop source) :
			base(source)
		{
			//Debug.EnableTracing(@class);

			EditCommand = new Command(OnEdit, () => CanEdit);
			DeleteCommand = new Command(OnDelete, () => CanDelete);
			StatusCommand = new Command(OnStatus, () => HasStatusCommand);
			DirectionsCommand = new Command(OnDirections, () => HasDirections);

			UpdateFromSource();
			UpdateSeqString();
			UpdateStatusCommand();
			UpdateDeleteCommand();
			UpdateDirectionCommand();

			source.PropertyChanged += (s, e) => UpdateFromSource();
		}

		public Command EditCommand { get; }
		public Command DeleteCommand { get; }
		public Command StatusCommand { get; }
		public Command DirectionsCommand { get; }


		public int Seq {
			get => seq;
			set {
				if (SetProperty(ref seq, value)) {
					Source.Seq = value;
					UpdateSeqString();
				}
			}
		}
		int seq;

		public string SeqString {
			get => seqString;
			set => SetProperty(ref seqString, value);
		}
		string seqString;


		public string CodeName {
			get => name;
			set => SetProperty(ref name, value); // only used in UpdateFromSource
			//set {
			//	if (SetProperty(ref name, value)) {
			//		Source.CodeName = value;
			//	}
			//}
		}
		string name;

		public string Address {
			get => address;
			set {
				if (SetProperty(ref address, value)) {
					//Source.FullName = value; // only used in UpdateFromSource
					UpdateDirectionCommand();
				}
			}
		}
		string address;

		public RouteStatus Status {
			get => status;
			set {
				if (SetProperty(ref status, value)) {
					Source.Status = value;
					UpdateSeqString();
					UpdateStatusCommand();
					UpdateDeleteCommand();
					UpdateDirectionCommand();
				}
			}
		}
		RouteStatus status;

		public bool IsCompleted {
			get => Status == (Source.Kind == RouteStopKind.EndPoint ? RouteStatus.Arrived : RouteStatus.Departed);
		}

		public bool CanEdit => false;

		//public bool CanDelete => Source.Kind == RouteStopKind.Customer;
		public bool CanDelete {
			get => canDelete;
			private set => SetProperty(ref canDelete, value);
		}
		bool canDelete;


		public bool HasDirections {
			get => hasDirections;
			private set => SetProperty(ref hasDirections, value);
		}
		bool hasDirections;


		public async void OnEdit()
		{
			var uri = $"routestop?order={Seq}";
			await Shell.Current.GoToAsync(uri);
		}

		public async void OnDelete()
		{
			if (!IsCompleted) {
				bool proceed = await Alerts.ConfirmAlert($"Delete stop #{Seq} ?");
				if (!proceed)
					return;
			}

			var exc = await AppScope.Instance.Route.DeleteStop(Source);
			if (exc != null) {
				await Alerts.ErrorAlert(exc.Message);
			}
		}

		public async void OnDirections()
		{
			if (HasDirections) {
				var exc = await AppScope.Instance.Route.ShowDirections(Source);
				if (exc != null) {
					await Alerts.ErrorAlert(exc.Message);
				}
			}
		}


		public string StatusCommandName {
			get => statusCommandName;
			set => SetProperty(ref statusCommandName, value);
		}
		string statusCommandName;

		public bool HasStatusCommand {
			get => hasStatusCommand;
			set => SetProperty(ref hasStatusCommand, value);
		}
		bool hasStatusCommand;

		public async void OnStatus()
		{
			var route = AppScope.Instance.Route;
			Exception exc = null;

			switch (Status)
			{
			case RouteStatus.Enroute:
				exc = await route.Arrive(Source);
				break;
			case RouteStatus.Arrived:
				exc = await route.Depart(Source);
				break;
			}

			if (exc != null) {
				await Alerts.ErrorAlert(exc.Message);
			}
		}

		protected override void UpdateFromSource()
		{
			Seq = Source.Seq;
			CodeName = Source.CodeName;
			Address = Source.Address;
			Status = Source.Status;
		}

		void UpdateSeqString()
		{
			switch (Status)
			{
			case RouteStatus.Enroute:
				SeqString = StdGlyph.ChequeredFlag;
				break;
			case RouteStatus.Arrived:
				SeqString = StdGlyph.HeavyCheckMark;
				break;
			case RouteStatus.Departed:
				SeqString = StdGlyph.NotCheckMark;
				break;
			default:
				SeqString = $"{Seq}.";
				break;
			}
		}

		void UpdateStatusCommand()
		{
			switch (Status)
			{
			case RouteStatus.Enroute:
				StatusCommandName = "Arrive";
				HasStatusCommand = true;
				return;

			case RouteStatus.Arrived:
				if (Source.Kind != RouteStopKind.EndPoint) {
					StatusCommandName = "Depart";
					HasStatusCommand = true;
					return;
				}
				break;
			}

			StatusCommandName = string.Empty;
			HasStatusCommand = false;
		}

		void UpdateDeleteCommand()
		{
			switch (Status)
			{
			case RouteStatus.Enroute:
				CanDelete = false;
				break;
			case RouteStatus.Arrived:
				CanDelete = Source.Kind == RouteStopKind.EndPoint;
				break;
			case RouteStatus.Departed:
				CanDelete = true;
				break;
			default:
				CanDelete = Source.Kind == RouteStopKind.Customer;
				break;
			}
		}

		void UpdateDirectionCommand()
		{
			if (Status < RouteStatus.Arrived) {
				HasDirections = AppScope.Instance.Route.CanShowDirections(Source);
			} else {
				HasDirections = false;
			}
		}
	}
}
