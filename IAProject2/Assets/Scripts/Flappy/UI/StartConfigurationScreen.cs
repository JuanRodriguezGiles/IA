using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class StartConfigurationScreen : MonoBehaviour
{
    public Text populationCountTxt;
    public Slider populationCountSlider;
    public Text eliteCountTxt;
    public Slider eliteCountSlider;
    public Text mutationChanceTxt;
    public Slider mutationChanceSlider;
    public Text mutationRateTxt;
    public Slider mutationRateSlider;
    public Text hiddenLayersCountTxt;
    public Slider hiddenLayersCountSlider;
    public Text neuronsPerHLCountTxt;
    public Slider neuronsPerHLSlider;
    public Text biasTxt;
    public Slider biasSlider;
    public Text sigmoidSlopeTxt;
    public Slider sigmoidSlopeSlider;
    public Text inputsTxt;
    public Slider inputsSlider;
    public Text outputsTxt;
    public Slider outputsSlider;
    public Text outputsThresholdTxt;
    public Slider outputsThresholdSlider;
    public Button startButton;
    public GameObject simulationScreen;

    string populationText;
    string minesText;
    string generationDurationText;
    string elitesText;
    string mutationChanceText;
    string mutationRateText;
    string hiddenLayersCountText;
    string biasText;
    string sigmoidSlopeText;
    string neuronsPerHLCountText;
    string inputsText;
    string outputsText;
    string outputsThresholdText;

    void Start()
    {
        populationCountSlider.onValueChanged.AddListener(OnPopulationCountChange);
        eliteCountSlider.onValueChanged.AddListener(OnEliteCountChange);
        mutationChanceSlider.onValueChanged.AddListener(OnMutationChanceChange);
        mutationRateSlider.onValueChanged.AddListener(OnMutationRateChange);
        hiddenLayersCountSlider.onValueChanged.AddListener(OnHiddenLayersCountChange);
        neuronsPerHLSlider.onValueChanged.AddListener(OnNeuronsPerHLChange);
        biasSlider.onValueChanged.AddListener(OnBiasChange);
        sigmoidSlopeSlider.onValueChanged.AddListener(OnSigmoidSlopeChange);
        inputsSlider.onValueChanged.AddListener(OnInputsChange);
        outputsSlider.onValueChanged.AddListener(OnOutputsChange);
        outputsThresholdSlider.onValueChanged.AddListener(OnOutputsThresholdChange);

        populationText = populationCountTxt.text;
        elitesText = eliteCountTxt.text;
        mutationChanceText = mutationChanceTxt.text;
        mutationRateText = mutationRateTxt.text;
        hiddenLayersCountText = hiddenLayersCountTxt.text;
        neuronsPerHLCountText = neuronsPerHLCountTxt.text;
        biasText = biasTxt.text;
        sigmoidSlopeText = sigmoidSlopeTxt.text;
        inputsText = inputsTxt.text;
        outputsText = outputsTxt.text;
        outputsThresholdText = outputsThresholdTxt.text;

        populationCountSlider.value = PopulationManager.Instance.PopulationCount;
        eliteCountSlider.value = PopulationManager.Instance.EliteCount;
        mutationChanceSlider.value = Mathf.Round(PopulationManager.Instance.MutationChance * 100.0f);
        mutationRateSlider.value = Mathf.Round(PopulationManager.Instance.MutationRate * 100.0f);
        hiddenLayersCountSlider.value = PopulationManager.Instance.HiddenLayers;
        neuronsPerHLSlider.value = PopulationManager.Instance.NeuronsCountPerHL;
        biasSlider.value = PopulationManager.Instance.Bias;
        sigmoidSlopeSlider.value = PopulationManager.Instance.Sigmoid;
        inputsSlider.value = PopulationManager.Instance.InputsCount;
        outputsSlider.value = PopulationManager.Instance.OutputsCount;
        outputsThresholdSlider.value = PopulationManager.Instance.OutputThreshold;

        startButton.onClick.AddListener(OnStartButtonClick);

        Refresh();
    }

    void OnPopulationCountChange(float value)
    {
        PopulationManager.Instance.PopulationCount = (int)value;

        populationCountTxt.text = string.Format(populationText, PopulationManager.Instance.PopulationCount);
    }

    void OnEliteCountChange(float value)
    {
        PopulationManager.Instance.EliteCount = (int)value;

        eliteCountTxt.text = string.Format(elitesText, PopulationManager.Instance.EliteCount);
    }

    void OnMutationChanceChange(float value)
    {
        PopulationManager.Instance.MutationChance = value / 100.0f;

        mutationChanceTxt.text = string.Format(mutationChanceText, (int)(PopulationManager.Instance.MutationChance * 100));
    }

    void OnMutationRateChange(float value)
    {
        PopulationManager.Instance.MutationRate = value / 100.0f;

        mutationRateTxt.text = string.Format(mutationRateText, (int)(PopulationManager.Instance.MutationRate * 100));
    }

    void OnHiddenLayersCountChange(float value)
    {
        PopulationManager.Instance.HiddenLayers = (int)value;


        hiddenLayersCountTxt.text = string.Format(hiddenLayersCountText, PopulationManager.Instance.HiddenLayers);
    }

    void OnNeuronsPerHLChange(float value)
    {
        PopulationManager.Instance.NeuronsCountPerHL = (int)value;

        neuronsPerHLCountTxt.text = string.Format(neuronsPerHLCountText, PopulationManager.Instance.NeuronsCountPerHL);
    }

    void OnBiasChange(float value)
    {
        PopulationManager.Instance.Bias = -value;

        biasTxt.text = string.Format(biasText, PopulationManager.Instance.Bias.ToString("0.00"));
    }

    void OnSigmoidSlopeChange(float value)
    {
        PopulationManager.Instance.Sigmoid = value;

        sigmoidSlopeTxt.text = string.Format(sigmoidSlopeText, PopulationManager.Instance.Sigmoid.ToString("0.00"));
    }


    void OnStartButtonClick()
    {
        PopulationManager.Instance.StartSimulation();
        this.gameObject.SetActive(false);
        simulationScreen.SetActive(true);
    }

    void OnInputsChange(float value)
    {
        PopulationManager.Instance.InputsCount = (int)value;

        inputsTxt.text = string.Format(inputsText, PopulationManager.Instance.InputsCount);
    }

    void OnOutputsChange(float value)
    {
        PopulationManager.Instance.OutputsCount = (int)value;

        outputsTxt.text = string.Format(outputsText, PopulationManager.Instance.OutputsCount);
    }

    void OnOutputsThresholdChange(float value)
    {
        PopulationManager.Instance.OutputThreshold = value;

        outputsThresholdTxt.text = string.Format(outputsThresholdText, PopulationManager.Instance.OutputThreshold.ToString("0.00"));
    }

    void Refresh()
    {
        populationCountTxt.text = string.Format(populationText, PopulationManager.Instance.PopulationCount);
        eliteCountTxt.text = string.Format(elitesText, PopulationManager.Instance.EliteCount);
        mutationChanceTxt.text = string.Format(mutationChanceText, (int)(PopulationManager.Instance.MutationChance * 100));
        mutationRateTxt.text = string.Format(mutationRateText, (int)(PopulationManager.Instance.MutationRate * 100));
        hiddenLayersCountTxt.text = string.Format(hiddenLayersCountText, PopulationManager.Instance.HiddenLayers);
        neuronsPerHLCountTxt.text = string.Format(neuronsPerHLCountText, PopulationManager.Instance.NeuronsCountPerHL);
        biasTxt.text = string.Format(biasText, PopulationManager.Instance.Bias.ToString("0.00"));
        sigmoidSlopeTxt.text = string.Format(sigmoidSlopeText, PopulationManager.Instance.Sigmoid.ToString("0.00"));
        inputsTxt.text = string.Format(inputsText, PopulationManager.Instance.InputsCount);
        outputsTxt.text = string.Format(outputsText, PopulationManager.Instance.OutputsCount);
        outputsThresholdTxt.text = string.Format(outputsThresholdText, PopulationManager.Instance.OutputThreshold.ToString("0.00"));
    }
}