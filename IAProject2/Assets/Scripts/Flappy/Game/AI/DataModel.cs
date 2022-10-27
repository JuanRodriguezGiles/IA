using System;
using System.Collections.Generic;

[Serializable]
public class DataModel
{
    public Genome population;

    public int InputsCount;
    public int HiddenLayers;
    public int OutputsCount;
    public int NeuronsCountPerHL;
    public float Bias;
    public float Sigmoid;

    public float outputThreshold;

    public OBSTACLE_TYPE type;
}