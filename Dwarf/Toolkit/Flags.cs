using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public struct Flags
	{
		public Flags(uint value)
		{
			this.value = value;
		}

		uint value;
		public uint Value {
			get => value;
			set => this.value = value;
		}

		public bool IsOn(uint flag) => AnyIsOn(value, flag);
		public bool AreOn(uint flags) => AllAreOn(value, flags);
		public bool AreOff(uint flags) => AllAreOff(value, flags);
		public void SetOn(uint flags) => TurnOn(ref value, flags);
		public void SetOff(uint flags) => TurnOff(ref value, flags);
		public void Set(uint flags, bool on) => Turn(ref value, flags, on);

		public static bool AnyIsOn(uint value, uint flags) => (value & flags) != 0;
		public static bool AllAreOn(uint value, uint flags) => (value & flags) == flags;
		public static bool AllAreOff(uint value, uint flags) => (value & flags) == 0;
		public static void TurnOn(ref uint value, uint flags) => value |= flags;
		public static void TurnOff(ref uint value, uint flags) => value &= ~flags;
		public static void Turn(ref uint value, uint flags, bool on) {
			if (on) {
				TurnOn(ref value, flags);
			} else {
				TurnOff(ref value, flags);
			}
		}
	}
}
