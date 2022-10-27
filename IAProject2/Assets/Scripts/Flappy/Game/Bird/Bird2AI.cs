using System;
using System.Collections.Generic;

using UnityEngine;

public class Bird2AI : BirdBase
{
    protected override void OnThink(float dt, float outputThreshold, BirdBehaviour birdBehaviour, Obstacle obstacle, List<BrainData> brains)
    {
        float[] inputs = new float[5];
        inputs[0] = (obstacle.transform.position - birdBehaviour.transform.position).x;
        inputs[1] = (obstacle.transform.position - birdBehaviour.transform.position).y;
        inputs[2] = (obstacle.transform.position - birdBehaviour.transform.position).y;
        inputs[3] = (obstacle.transform.position - birdBehaviour.transform.position).y;
        inputs[4] = (obstacle.transform.position - birdBehaviour.transform.position).y;

        float[] outputs;
        
        if (obstacle.type == OBSTACLE_TYPE.VERTICAL)
        {
            outputs = brains[0].brain.Synapsis(inputs);
        }
        else
        {
            outputs = brains[1].brain.Synapsis(inputs);
        }
        
        if (outputs[0] > outputThreshold)
        {
            birdBehaviour.Flap();
        }

        if (Vector3.Distance(obstacle.transform.position, birdBehaviour.transform.position) <= 1.0f)
        {
            genome.fitness *= 2;
        }
        
        genome.fitness += (100.0f - Vector3.Distance(obstacle.transform.position, birdBehaviour.transform.position));
        genome.fitness -= Math.Abs(birdBehaviour.transform.position.y) * 2;
    }

    protected override void OnDead()
    {
    }

    protected override void OnReset()
    {
        genome.fitness = 0.0f;
    }
}