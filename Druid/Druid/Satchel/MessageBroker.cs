using System;
using Xamarin.Forms;


namespace Dwares.Druid.Satchel
{
	public static partial class MessageBroker
	{
		public static string GetMessageId(Type messageType)
		{
			// TODO
			return messageType.FullName;
		}
		
		public static void GetMessageId<Message>(ref string messageId)
		{
			if (string.IsNullOrEmpty(messageId)) {
				messageId = GetMessageId(typeof(Message));
			}
		}

		public static void Send<Message>(Message message, string messageId = null) where Message : class
		{
			GetMessageId<Message>(ref messageId);
			MessagingCenter.Send(message, messageId);
		}

		public static void Subscribe<Message>(object subscriber, Action<Message> callback) where Message : class
			=> Subscribe(subscriber, null, callback);

		public static void Subscribe<Message>(object subscriber, string messageId, Action<Message> callback) where Message : class
		{
			GetMessageId<Message>(ref messageId);
			MessagingCenter.Subscribe<Message>(subscriber, messageId, callback);
		}

		public static void Unsubscribe<Message>(object subscriber, string messageId = null) where Message : class
		{
			GetMessageId<Message>(ref messageId);
			MessagingCenter.Unsubscribe<Message>(subscriber, messageId);
		}
	}
}
