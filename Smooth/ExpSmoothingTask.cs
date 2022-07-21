using System.Collections.Generic;
using System.Linq;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			var enumerator = data.GetEnumerator();

			enumerator.MoveNext();
			DataPoint current = enumerator.Current;
			if (current == null)
				yield break;
			yield return current = current.WithExpSmoothedY(current.OriginalY);

			while(enumerator.MoveNext())
            {
				var next = enumerator.Current;
				double smoothed = alpha * next.OriginalY + (1 - alpha) * current.ExpSmoothedY;
				yield return current = next.WithExpSmoothedY(smoothed);
			}
		}
	}
}