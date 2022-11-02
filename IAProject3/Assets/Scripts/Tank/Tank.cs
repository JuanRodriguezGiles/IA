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
        
        bool idealMine = (IsGoodMine(nearMine) && good) || (!IsGoodMine(nearMine) && !good);
        inputs[4] = idealMine ? 1.0f : -1.0f; 
        inputs[5] = idealMine ? 1.0f : -1.0f;

        if (good)
        {
            float distance = (transform.position - goodMine.transform.position).sqrMagnitude;
            inputs[6] = distance;
        }
        else
        {
            float distance = (transform.position - badMine.transform.position).sqrMagnitude;
            inputs[6] = distance;
        }

        inputs[7] = (transform.position - nearMine.transform.position).sqrMagnitude;

        var colliders = Physics.OverlapSphere(nearMine.transform.position, 10);
        
        float count = 0.0f;
        foreach (var collider in colliders)
        {
            Tank tank = collider.GetComponent<Tank>();
            if (tank != null && tank.nearMine != null) 
            {
                if (tank.GetDirToMine(tank.nearMine) == dirToMine)
                {
                    count++;
                }
            }
        }

        if (count == 0)
        {
            count = -1.0f;
        }

        inputs[8] = count;

        float[] output = brain.Synapsis(inputs);

        SetForces(output[0], output[1], dt);
    }

    protected override void OnTakeMine(GameObject mine)
    {
        // if (IsGoodMine(mine) && good) 
        // {
        //     fitness += 10;
        //     genome.fitness = fitness;
        // }
        // else if (!IsGoodMine(mine) && !good) 
        // {
        //     fitness += 10;
        //     genome.fitness = fitness;
        // }
        // else
        // {
        //     fitness += 5;
        //     genome.fitness = fitness;
        // }
        Color color = mine.GetComponent<Renderer>().material.color;
        if (color == Color.green && good)
        {
            fitness += 10;
            genome.fitness = fitness;
        }
        else if (color == Color.red && !good)
        {
            fitness += 10;
            genome.fitness = fitness;
        }
        else
        {
            fitness += 5;
            genome.fitness = fitness;
        }
    }
}