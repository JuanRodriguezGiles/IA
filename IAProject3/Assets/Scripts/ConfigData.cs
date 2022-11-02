using System;

[Serializable]
public class ConfigData
{
    public int populationCount;
    public int minesCount;
    public float generationDuration;
    public int iterationCount;
    public int eliteCount;
    public float mutationChance;
    public float mutationRate;
    public int inputsCount;
    public int hiddenLayers;
    public int outputsCount;
    public int neuronsCountPerHL;
    public float bias;
    public float p;
}