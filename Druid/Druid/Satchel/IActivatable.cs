using System;


namespace Dwares.Druid.Satchel
{
	public interface IActivatable
	{
		bool IsActive { get; }

		void Activate();
		void Deactivate();
	}
}
