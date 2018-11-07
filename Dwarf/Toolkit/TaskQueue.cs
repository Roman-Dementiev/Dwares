using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Dwares.Dwarf.Toolkit
{
	//public interface ITaskSource<TType>
	//{
	//	Action GetTask(TType type);
	//}

	public class TaskQueue<TType>
	{
		ulong lastId = 0;
		LinkedList<TaskNode> queue = new LinkedList<TaskNode>();

		//public TaskQueue()
		//{
		//}

		public bool Running { get; private set; }

		public ulong AddTask(Action action, TType type = default(TType), bool urgent = false)
			=> AddTask(null, action, type, urgent);

		public ulong AddTask(object source, Action action, TType type = default(TType), bool urgent = false)
		{
			var node = new TaskNode {
				Id = ++lastId,
				Type = type,
				Source = source,
				Action = action
			};

			if (urgent) {
				queue.AddFirst(node);
			} else {
				queue.AddLast(node);
			}

			return node.Id;
		}

		//public ulong AddTask(ITaskSource<TType> source, TType type = default(TType), bool urgent = false)
		//{
		//	var task = source?.GetTask(type);
		//	if (task != null) {
		//		return AddTask(source, task, type, urgent);
		//	} else {
		//		return 0;
		//	}
		//}

		public Task GetTask(ulong id)
		{
			foreach (var node in queue) {
				if (node.Id == id)
					return node.Task;
			}
			return null;
		}

		public async Task Run()
		{
			if (Running)
				return;

			Running = true;

			while (queue.Count > 0) {
				var node = queue.First.Value;
				queue.RemoveFirst();

				try {
					node.Task = Task.Run(node.Action);
					await node.Task;
				}
				catch (Exception ex) {
					Debug.ExceptionCaught(ex);
				}
			}

			Running = false;
		}

		struct TaskNode
		{
			public ulong Id { get; set; }
			public TType Type { get; set; }
			public object Source { get; set; }
			public Action Action { get; set; }
			public Task Task { get; set; }
		}
	}

	public class TaskQueue : TaskQueue<Type>
	{
		public TaskQueue() { }
	}
}
