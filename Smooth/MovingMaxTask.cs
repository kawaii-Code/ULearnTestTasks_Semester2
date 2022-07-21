using System;
using System.Collections.Generic;

namespace yield
{
	public static class MovingMaxTask
	{
		public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var window = new MaxWindow<double>(windowWidth);

			foreach (DataPoint point in data)
            {
				window.AddElement(point.OriginalY);
				yield return point.WithMaxY(window.Max);
            }
		}
	}

	public class MaxWindow<T> where T : IComparable
    {
		private LinkedList<T> _potentialMax;
		private LinkedList<T> _windowElements;
		private int _bufferLen;

		public MaxWindow(int bufferLen)
        {
			_bufferLen = bufferLen;
			_potentialMax = new LinkedList<T>();
			_windowElements = new LinkedList<T>();
        }

		public T Max => _potentialMax.First.Value;

		public void AddElement(T value)
        {
			_windowElements.AddLast(value);
			RemoveElementsLessThan(value);
			_potentialMax.AddLast(value);

			ApplyBufferConstraint();
        }

		private void ApplyBufferConstraint()
        {
			if (_windowElements.Count > _bufferLen)
			{
				if (_windowElements.First.Value.CompareTo(_potentialMax.First.Value) == 0)
					_potentialMax.RemoveFirst();
				_windowElements.RemoveFirst();
			}
		}

		private void RemoveElementsLessThan(T value)
        {
			while (_potentialMax.Count > 0 &&
					_potentialMax.Last.Value.CompareTo(value) < 0)
				_potentialMax.RemoveLast();
		}
    }
}