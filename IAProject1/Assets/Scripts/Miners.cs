using System.Collections.Concurrent;
using System.Threading.Tasks;

using UnityEngine;

public class Miners : MonoBehaviour
{
    #region EXPOSED_FIELDS
    public GameObject minerGo;
    public GameObject mine;
    public GameObject deposit;
    private ConcurrentBag<Miner> miners = new();
    private ParallelOptions parallelOptions;
    private float deltaTime;
    #endregion

    private void Start()
    {
        parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 12 };

        for (int i = 0; i < 100; i++)
        {
            GameObject go = Instantiate(minerGo);
            Miner miner = go.GetComponent<Miner>();
            miner.Init(mine, deposit, GetDeltaTime);

            miners.Add(miner);
        }
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;
        Parallel.ForEach(miners, parallelOptions, minerito => { minerito.UpdateMiner(); });
    }

    private float GetDeltaTime()
    {
        return deltaTime;
    }
}