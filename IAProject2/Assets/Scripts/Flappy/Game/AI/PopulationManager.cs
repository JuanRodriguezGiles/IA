using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class PopulationManager : MonoBehaviour
{
    public GameObject BirdPrefab;
    public int PopulationCount = 40;
    public int IterationCount = 1;

    public int EliteCount = 4;
    public float MutationChance = 0.10f;
    public float MutationRate = 0.01f;

    public int InputsCount = 4;
    public int HiddenLayers = 1;
    public int OutputsCount = 2;
    public int NeuronsCountPerHL = 7;
    public float Bias = 1f;
    public float Sigmoid = 0.5f;

    public float OutputThreshold = 0.5f;

    public List<string> brainsDataName;
    public string currentDataName;
    public OBSTACLE_TYPE currentObstacleType;
    
    GeneticAlgorithm genAlg;

    List<BirdBase> populationGOs = new List<BirdBase>();
    List<Genome> population = new List<Genome>();
    List<NeuralNetwork> brains = new List<NeuralNetwork>();
    List<BrainData> savedBrains = new List<BrainData>();

    bool isRunning = false;

    public int generation { get; private set; }

    public float bestFitness { get; private set; }

    public float avgFitness { get; private set; }

    public float worstFitness { get; private set; }

    private float GetBestFitness()
    {
        float fitness = 0;
        foreach (Genome g in population)
        {
            if (fitness < g.fitness)
                fitness = g.fitness;
        }

        return fitness;
    }
    
    private float GetAvgFitness()
    {
        float fitness = 0;
        foreach (Genome g in population)
        {
            fitness += g.fitness;
        }

        return fitness / population.Count;
    }

    private float GetWorstFitness()
    {
        float fitness = float.MaxValue;
        foreach (Genome g in population)
        {
            if (fitness > g.fitness)
                fitness = g.fitness;
        }

        return fitness;
    }

    public BirdBase GetBestAgent()
    {
        if (populationGOs.Count == 0)
        {
            Debug.Log("Population count is zero");
            return null;
        }

        BirdBase bird = populationGOs[0];
        Genome bestGenome = population[0];
        for (int i = 0; i < population.Count; i++)
        {
            if (populationGOs[i].state == BirdBase.State.Alive && population[i].fitness > bestGenome.fitness)
            {
                bestGenome = population[i];
                bird = populationGOs[i];
            }
        }

        return bird;
    }

    static PopulationManager instance = null;

    public static PopulationManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PopulationManager>();

            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        Load();
    }

    public void Load()
    {
        PopulationCount = PlayerPrefs.GetInt("PopulationCount", 2);
        EliteCount = PlayerPrefs.GetInt("EliteCount", 0);
        MutationChance = PlayerPrefs.GetFloat("MutationChance", 0);
        MutationRate = PlayerPrefs.GetFloat("MutationRate", 0);
        InputsCount = PlayerPrefs.GetInt("InputsCount", 1);
        HiddenLayers = PlayerPrefs.GetInt("HiddenLayers", 5);
        OutputsCount = PlayerPrefs.GetInt("OutputsCount", 1);
        NeuronsCountPerHL = PlayerPrefs.GetInt("NeuronsCountPerHL", 1);
        Bias = PlayerPrefs.GetFloat("Bias", 0);
        Sigmoid = PlayerPrefs.GetFloat("P", 1);
    }

    void Save()
    {
        PlayerPrefs.SetInt("PopulationCount", PopulationCount);
        PlayerPrefs.SetInt("EliteCount", EliteCount);
        PlayerPrefs.SetFloat("MutationChance", MutationChance);
        PlayerPrefs.SetFloat("MutationRate", MutationRate);
        PlayerPrefs.SetInt("InputsCount", InputsCount);
        PlayerPrefs.SetInt("HiddenLayers", HiddenLayers);
        PlayerPrefs.SetInt("OutputsCount", OutputsCount);
        PlayerPrefs.SetInt("NeuronsCountPerHL", NeuronsCountPerHL);
        PlayerPrefs.SetFloat("Bias", Bias);
        PlayerPrefs.SetFloat("P", Sigmoid);
    }

    public void SaveBrain()
    {
        DataModel dataModel = new DataModel();

        dataModel.population = GetBestAgent().genome;
        dataModel.Bias = Bias;
        dataModel.outputThreshold = OutputThreshold;
        dataModel.Sigmoid = Sigmoid;
        dataModel.HiddenLayers = HiddenLayers;
        dataModel.InputsCount = InputsCount;
        dataModel.OutputsCount = OutputsCount;
        dataModel.NeuronsCountPerHL = NeuronsCountPerHL;
        dataModel.type = currentObstacleType;
        
        PlayerPrefs.SetString("Brain_" + currentDataName, JsonUtility.ToJson(dataModel));
    }

    public void DeleteBrains()
    {
        for (int i = 0; i < brainsDataName.Count; i++)
        {
            PlayerPrefs.DeleteKey("Brain_" + brainsDataName[i]);
        }
    }

    public void LoadBrains()
    {
        for (int i = 0; i < brainsDataName.Count; i++)
        {
            if (PlayerPrefs.HasKey("Brain_" + brainsDataName[i]))
            {
                string json = PlayerPrefs.GetString("Brain_" + brainsDataName[i]);

                DataModel dataModel = JsonUtility.FromJson<DataModel>(json);
                
                NeuralNetwork brain = CreateBrain(dataModel.InputsCount, dataModel.Bias, dataModel.Sigmoid, dataModel.HiddenLayers, dataModel.NeuronsCountPerHL, dataModel.OutputsCount);
                BrainData brainData = new BrainData();
                brainData.brain = brain;
                brainData.genome = dataModel.population;
                brainData.OutputThreshold = dataModel.outputThreshold;
                
                savedBrains.Add(brainData);
            }
        }
    }

    public void StartSimulation()
    {
        Save();
        LoadBrains();
        // Create and confiugre the Genetic Algorithm
        genAlg = new GeneticAlgorithm(EliteCount, MutationChance, MutationRate);

        GenerateInitialPopulation();

        isRunning = true;
    }

    public void PauseSimulation()
    {
        isRunning = !isRunning;
    }

    public void StopSimulation()
    {
        Save();

        isRunning = false;

        generation = 0;

        BackgroundManager.Instance.Reset();
        ObstacleManager.Instance.Reset();
        CameraFollow.Instance.Reset();

        DestroyBirds();
    }

    // Generate the random initial population
    void GenerateInitialPopulation()
    {
        generation = 0;
        DestroyBirds();
        
        for (int i = 0; i < PopulationCount; i++)
        {
            NeuralNetwork brain = CreateBrain();
        
            Genome genome = new Genome(brain.GetTotalWeightsCount());
        
            brain.SetWeights(genome.genome);
            brains.Add(brain);
        
            population.Add(genome);
            populationGOs.Add(CreateBird(genome, brain));
        }
    }

    // Creates a new NeuralNetwork
    NeuralNetwork CreateBrain()
    {
        NeuralNetwork brain = new NeuralNetwork();

        // Add first neuron layer that has as many neurons as inputs
        brain.AddFirstNeuronLayer(InputsCount, Bias, Sigmoid);

        for (int i = 0; i < HiddenLayers; i++)
        {
            // Add each hidden layer with custom neurons count
            brain.AddNeuronLayer(NeuronsCountPerHL, Bias, Sigmoid);
        }

        // Add the output layer with as many neurons as outputs
        brain.AddNeuronLayer(OutputsCount, Bias, Sigmoid);

        return brain;
    }

    NeuralNetwork CreateBrain(int inputsCount, float bias, float sigmoid, float hiddenLayers, int neuronsCountPerHl, int outputsCount)
    {
        NeuralNetwork brain = new NeuralNetwork();

        // Add first neuron layer that has as many neurons as inputs
        brain.AddFirstNeuronLayer(inputsCount, bias, sigmoid);

        for (int i = 0; i < hiddenLayers; i++)
        {
            // Add each hidden layer with custom neurons count
            brain.AddNeuronLayer(neuronsCountPerHl, bias, sigmoid);
        }

        // Add the output layer with as many neurons as outputs
        brain.AddNeuronLayer(outputsCount, bias, sigmoid);

        return brain;
    }

    void Epoch() //Evolve
    {
        CameraFollow.Instance.Reset();
        ObstacleManager.Instance.Reset();
        BackgroundManager.Instance.Reset();

        // Increment generation counter
        generation++;

        // Calculate best, average and worst fitness
        bestFitness = GetBestFitness();
        avgFitness = GetAvgFitness();
        worstFitness = GetWorstFitness();

        // Evolve each genome and create a new array of genomes
        Genome[] newGenomes = genAlg.Epoch(population.ToArray());

        // Clear current population
        population.Clear();

        // Add new population
        population.AddRange(newGenomes);

        // Set the new genomes as each NeuralNetwork weights
        for (int i = 0; i < PopulationCount; i++)
        {
            NeuralNetwork brain = brains[i];
            brain.SetWeights(newGenomes[i].genome);
            populationGOs[i].SetBrain(newGenomes[i], brain);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isRunning)
            return;

        float dt = Time.fixedDeltaTime;

        for (int i = 0; i < Mathf.Clamp((float)IterationCount, 1, 100); i++)
        {
            CameraFollow.Instance.UpdateCamera();
            ObstacleManager.Instance.CheckAndInstatiate();

            bool areAllDead = true;

            foreach (BirdBase b in populationGOs)
            {
                // Think!! 
                b.Think(dt, OutputThreshold, savedBrains);
                if (b.state == BirdBase.State.Alive)
                    areAllDead = false;
            }

            // Check the time to evolve
            if (areAllDead)
            {
                Epoch();
                break;
            }
        }
    }

    #region Helpers
    BirdBase CreateBird(Genome genome, NeuralNetwork brain)
    {
        Vector3 position = Vector3.zero;
        GameObject go = Instantiate<GameObject>(BirdPrefab, position, Quaternion.identity);
        BirdBase b = go.GetComponent<BirdBase>();
        b.SetBrain(genome, brain);
        return b;
    }

    void DestroyBirds()
    {
        foreach (BirdBase go in populationGOs)
            Destroy(go.gameObject);

        populationGOs.Clear();
        population.Clear();
        brains.Clear();
    }
    #endregion
}