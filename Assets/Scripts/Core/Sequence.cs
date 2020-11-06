using System;
using System.Collections.Generic;

namespace NeonBlaze.Core
{
	public class Sequence<T> where T : Delegate
	{
		public event Action Finished;

		private readonly Queue<T> mQueue = new Queue<T>();

		public void Next(params object[] args)
		{
			if (mQueue.Count > 0) mQueue.Dequeue().DynamicInvoke(args);
			else Finish();
		}

		public void Finish()
		{
			mQueue.Clear();
			Finished?.Invoke();
			Finished = null;
		}

		public void Add(T d)
		{
			mQueue.Enqueue(d);
		}

		public void Start(params object[] args)
		{
			Next(args);
		}
	}
}