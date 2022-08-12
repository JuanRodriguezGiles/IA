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
    #endregion

    private void Start()
    {
        parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 8 };
        
        for (int i = 0; i < 100; i++)
        {
            GameObject go = Instantiate(minerGo);
            Miner miner = go.GetComponent<Miner>();
            miner.Init(mine, deposit);
            
            miners.Add(miner);
        }
    }

    private void Update()
    {
        Parallel.ForEach(miners, parallelOptions, minerito =>
        {
            minerito.UpdateMiner();
        });
    }
}