using UnityEngine;

public class Tank : TankBase
{
    float fitness = 0;
    protected override void OnReset()
    {
        fitness = 1;
    }

    protected override void OnThink(float dt) 
	{
        Vector3 dirToMine = GetDirToMine(nearMine);
        Vector3 dir = this.transform.forward;

        inputs[4] = -1;
        inputs[5] = -1;
        
        var colliders = Physics.OverlapSphere(nearMine.transform.position, 10);
        if (colliders != null)
        {
            inputs[4] = colliders.Length;
            inputs[5] = colliders.Length;
        }
        
        inputs[0] = dirToMine.x;
        inputs[1] = dirToMine.z;
        inputs[2] = dir.x;
        inputs[3] = dir.z;

        float[] output = brain.Synapsis(inputs);

        SetForces(output[0], output[1], output[4], dt);
    }
    
    protected override void OnTakeMine(GameObject mine)
    {
        fitness *= 2;
        genome.fitness = fitness;
    }
}
