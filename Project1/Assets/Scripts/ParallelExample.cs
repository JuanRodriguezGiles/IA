using System.Collections.Concurrent;
using System.Threading.Tasks;

using UnityEngine;

public class ParallelExample : MonoBehaviour
{
    void Start()
    {
        ConcurrentBag<int> falopa = new ConcurrentBag<int>
        {
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        };

        ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 8 };

        Parallel.For(0, falopa.Count, parallelOptions, index =>
        {
            if (falopa.TryTake(out int result))
            {
                result++;
                falopa.Add(result);
            }
        });

        for (int i = 0; i < falopa.Count; i++)
        {
            if (falopa.TryTake(out int result))
            {
                Debug.Log(result);
                falopa.Add(result);
            }
        }
    }
}