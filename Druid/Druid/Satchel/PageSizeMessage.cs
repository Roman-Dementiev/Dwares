using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dwares.Druid.Satchel
{
	public class PageSizeMessage
	{
		public double PageWidth { get; set; }
		public double PageHeight { get; set; }
	}

	public static partial class MessageBroker
	{
		public static void SendPageSizeMessage(double pageWidth, double pageHeight)
		{
			var message = new PageSizeMessage() {
				PageWidth = pageWidth,
				PageHeight = pageHeight
			};

			Send(message);
		}

		public static void SubscribePageSizeMessage(object subscriber, Action<PageSizeMessage> action)
		{
			Subscribe<PageSizeMessage>(subscriber, (message) => action(message));
		}

		public static void UnsubscribePageSizeMessage(object subscriber)
		{
			Unsubscribe<PageSizeMessage>(subscriber);
		}

		public static void StartSendingPageSizeMessage(this Page page)
		{
			page.SizeChanged += Page_SizeChanged;
		}
		public static void StopSendingPageSizeMessage(this Page page)
		{
			page.SizeChanged -= Page_SizeChanged;
		}

		private static void Page_SizeChanged(object sender, EventArgs e)
		{
			if (sender is Page page) {
				SendPageSizeMessage(page.Width, page.Height);
			}
		}
	}
}
