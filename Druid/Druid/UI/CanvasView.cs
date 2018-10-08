using System;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Druid.UI
{
	public class CanvasView : SKCanvasView
	{
		public static ClassRef @class => new ClassRef(typeof(CanvasView));

		public CanvasView()
		{
			Debug.EnableTracing(@class);
		}

		public virtual IRatio<int> AspectRatio { get => Rational.None; }

		protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
		{
			SizeRequest request;
			var ratio = AspectRatio;
			if (ratio.IsValid) {
				var one = Math.Min(widthConstraint / ratio.Antecedent, heightConstraint / ratio.Consequent);
				request = new SizeRequest(new Size(one * ratio.Antecedent, one * ratio.Consequent));
			} else {
				request = base.OnMeasure(widthConstraint, heightConstraint);
			}

			Debug.TraceNamedValues(@class, nameof(OnMeasure),
				nameof(widthConstraint), widthConstraint,
				nameof(heightConstraint), heightConstraint,
				nameof(request.Request), request.Request,
				nameof(request.Minimum), request.Minimum
				);
			return request;
		}

		protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			base.OnPaintSurface(e);

			Debug.TraceNamedValues(@class, nameof(OnPaintSurface),
				nameof(CanvasSize), CanvasSize);

			var canvas = e.Surface.Canvas;
			canvas.Save();
			try {
				using (var objects = new Disposables()) {
					Draw(canvas, objects);
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			} 
			finally {
				canvas.Restore();
			}
		}

		protected virtual void Draw(SKCanvas canvas, Disposables objects) { }

		//protected override void OnTouch(SKTouchEventArgs e)
		//{
		//	//switch (e.ActionType) {
		//	//case SKTouchAction.Pressed:
		//	//	// start of a stroke
		//	//	var p = new SKPath();
		//	//	p.MoveTo(e.Location);
		//	//	temporaryPaths[e.Id] = p;
		//	//	break;
		//	//case SKTouchAction.Moved:
		//	//	// the stroke, while pressed
		//	//	if (e.InContact)
		//	//		temporaryPaths[e.Id].LineTo(e.Location);
		//	//	break;
		//	//case SKTouchAction.Released:
		//	//	// end of a stroke
		//	//	paths.Add(temporaryPaths[e.Id]);
		//	//	temporaryPaths.Remove(e.Id);
		//	//	break;
		//	//case SKTouchAction.Cancelled:
		//	//	// we don't want that stroke
		//	//	temporaryPaths.Remove(e.Id);
		//	//	break;
		//	//}

		//	//// we have handled these events
		//	//e.Handled = true;

		//	//// update the UI
		//	//((SKCanvasView)sender).InvalidateSurface();
		//	base.OnTouch(e);
		//}
	}
}
