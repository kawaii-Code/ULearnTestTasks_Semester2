using System.Collections.Generic;
using System.Linq;

namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var mean = new MovingMean(windowWidth);

			foreach (var next in data) 
				yield return mean.Measure(next);
		}
	}

	public class MovingMean
    {
		private double _sum;
		private int _bufferLen;
		private Queue<double> _data;

		public MovingMean(int bufferLen)
        {
			_data = new Queue<double>();
			_bufferLen = bufferLen;
        }

		public DataPoint Measure(DataPoint point)
        {
			_data.Enqueue(point.OriginalY);
			_sum += point.OriginalY;

			if (_data.Count > _bufferLen)
				_sum -= _data.Dequeue();

			return point.WithAvgSmoothedY(_sum / _data.Count);
        }
    }		
}