using System;


namespace Dwares.Dwarf.Toolkit
{
	public enum FitMode
	{
		None,
		Shrink,
		Expand,
		Fill
	};

	public abstract class Fitter<TValue>
	{
		public enum ProbeResult
		{
			Ok,
			CanExpand,
			NeedShrink
		}

		public FitMode Mode { get; set; }
		//public TValue Value { get; protected set; }

		public Fitter(FitMode mode = FitMode.Fill)
		{
			Mode = mode;
		}

		protected abstract bool Shrink(ref TValue value);
		protected abstract bool TryExpand(TValue value, out TValue newValue);

		public bool Fit(ref TValue value, Func<TValue, ProbeResult> probe)
		{
			//Value = value;
			var result = probe(value);

			if (Mode == FitMode.None) {
				return result != ProbeResult.NeedShrink;
			}

			if (result == ProbeResult.NeedShrink) {
				if (Mode == FitMode.Expand)
					return false;

				do {
					if (!Shrink(ref value))
						return false;

					result = probe(value);
				}
				while (result == ProbeResult.NeedShrink);
			}
			else if (result == ProbeResult.CanExpand && Mode != FitMode.Shrink) {
				do {
					TValue newValue;
					if (!TryExpand(value, out newValue))
						return true;

					result = probe(newValue);
					if (result == ProbeResult.NeedShrink)
						break;
					value = newValue;
				}
				while (result == ProbeResult.CanExpand);
			}

			return true;
		}

		public static ProbeResult ProbeValue(float value, float constrain)
		{
			if (value > constrain)
				return ProbeResult.NeedShrink;
			if (value < constrain)
				return ProbeResult.CanExpand;
			return ProbeResult.Ok;
		}
	}
}
