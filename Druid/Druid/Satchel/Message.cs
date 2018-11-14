//using System;
//using Xamarin.Forms;


//namespace Dwares.Druid.Satchel
//{
//	public class Message<TSender> where TSender : class
//	{
//		protected Message() { }

//		public Message(string id)
//		{
//			Id = id;
//		}

//		public string Id { get; protected set; }

//		public static void Send<TMessage>(TSender sender, TMessage message) where TMessage: Message<TSender>
//		{
//			MessagingCenter.Send(sender, message.Id, message);
//		}

//		public static void Subscribe<TMessage>(object subscriber, string messageId,  Action<TSender, TMessage> callback, TSender source = null)
//		{
//			MessagingCenter.Subscribe(subscriber, messageId, callback, source);
//		}

//		public static void Subscribe(object subscriber, string messageId, Action<TSender> callback, TSender source = null)
//		{
//			MessagingCenter.Subscribe(subscriber, messageId, callback, source);
//		}

//		//public static void Unsubscribe<TMessage>(object subscriber, string messageId)
//		//{
//		//	MessagingCenter.Unsubscribe<TSender, TMessage>(subscriber, messageId);
//		//}

//		public static void Unsubscribe(object subscriber, string messageId)
//		{
//			MessagingCenter.Unsubscribe<TSender>(subscriber, messageId);
//		}
//	}
//}
