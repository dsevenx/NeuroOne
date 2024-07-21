using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System;
using System.Reflection.Emit;

public class Neuron : NeuronBase
{
    public TextMeshPro mTextMeshProName;

    public TextMeshPro mTextMeshProBias;
    public TextMeshPro mTextMeshProWert;
    public TextMeshPro mTextMeshProWeights;

    public void Init(int pNummer, float pBiasStart, float pWeightStart, Dictionary<int, EingabeObjekt> mInputElemente)
    {
        Initialisiere(pNummer, pBiasStart);

        foreach (var lInpurneuron in mInputElemente)
        {
            mWeightsZuInput.Add(lInpurneuron.Value, new Weight(pWeightStart));
        }

        mTextMeshProName.text = "H" + mNummer;
        ErmittelOutput();
    }

    void Update()
    {
        mTextMeshProWert.text = "g" + mNummer + "=" + string.Format("{0:F2}", LieferOutput());
        mTextMeshProBias.text = string.Format("{0:F2}", LieferBias());
        mTextMeshProWeights.text = string.Format("{0:F2}", LieferWeights());
    }

    private float ErmittelReLU(float pValue)
    {
        if (pValue < 0)
        {
            return 0;
        }

        return pValue;
    }

    internal void ErmittelOutput()
    {
        mSumme = ErmittelSumme();
        mOutput = ErmittelReLU(mSumme);
    }

    internal void ErmittelNeueWeightsAndBias(float pLernrate)
    {
        foreach (var lNeuronWeight in mWeightsZuInput)
        {

            float lAbweichungGesamt = 0;
            foreach (NeuronBase lNeuronOutput in mOutputNeuronen)
            {

                lAbweichungGesamt = lAbweichungGesamt
                      + (lNeuronOutput.LieferCostFunctionAbleitung_DiffMal2MalLernRate()
                      * lNeuronOutput.LieferWeightZuInputNeuron(this)
                      * ermittelReLUAbleitung()
                      * lNeuronWeight.Key.LieferOutput());
            }

            lNeuronWeight.Value.setWeight(lNeuronWeight.Value.mWeight - lAbweichungGesamt);
        }

        float lAbweichungGesamtBias = 0;
        foreach (NeuronBase lNeuronOutput in mOutputNeuronen)
        {

            lAbweichungGesamtBias = lAbweichungGesamtBias
                  + (lNeuronOutput.LieferCostFunctionAbleitung_DiffMal2MalLernRate()
                  * lNeuronOutput.LieferWeightZuInputNeuron(this)
                  * ermittelReLUAbleitung());
        }

        mBias -= lAbweichungGesamtBias;
    }

    private float ermittelReLUAbleitung()
    {
        if (mSumme > 0)
        {
            return 1f;
        }

        return 0.001f;
    }
}
