using UnityEngine;
using System.Collections;

public class Tank : TankBase
{
    public bool good = false;
    public float fitness = 0;
    
    public override void OnReset()
    {
        fitness = 1;
    }

    protected override void OnThink(float dt) 
	{
        Vector3 dirToMine = GetDirToMine(nearMine);
        Vector3 dir = this.transform.forward;

        inputs[0] = dirToMine.x;
        inputs[1] = dirToMine.z;
        inputs[2] = dir.x;
        inputs[3] = dir.z;
        inputs[4] = IsGoodMine(nearMine) ? 1 : 0;
        inputs[5] = IsGoodMine(nearMine) ? 1 : 0;

        float[] output = brain.Synapsis(inputs);

        SetForces(output[0], output[1], dt);
	}
    
    protected override void OnTakeMine(GameObject mine)
    {
        if (IsGoodMine(mine) && !good) 
        {
            fitness *= 2;
            genome.fitness = fitness;
        }
        else if (!IsGoodMine(mine) && good)
        {
            fitness *= 2;
            genome.fitness = fitness;
        }
        else
        {
            Debug.Log("No points given");
        }
    }
}
