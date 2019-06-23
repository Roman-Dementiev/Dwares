using System;


namespace Drive.ViewModels
{
	interface ITabContentViewModel
	{
		Type ContentViewType();
		Type ControlsViewType(bool landscape);
	}
}
