using System;

[Serializable]
public class BrainData
{
    public Genome genome;
    public NeuralNetwork brain;
    public float OutputThreshold = 0.5f;
    public OBSTACLE_TYPE type;
}