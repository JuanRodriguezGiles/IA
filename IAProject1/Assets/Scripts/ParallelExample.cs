using System.Collections.Concurrent;
using System.Threading.Tasks;

using UnityEngine;

public class ParallelExample : MonoBehaviour
{
    private void Start()
    {
        var falopa = new ConcurrentBag<int>
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

        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 8 };

        Parallel.For(0, falopa.Count, parallelOptions, index =>
        {
            if (falopa.TryTake(out var result))
            {
                result++;
                falopa.Add(result);
            }
        });

        for (var i = 0; i < falopa.Count; i++)
            if (falopa.TryTake(out var result))
            {
                Debug.Log(result);
                falopa.Add(result);
            }
    }
}