using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AusgabeObjekt : NeuronBase
{
    public TextMeshPro mTextMeshProName;

    public TextMeshPro mTextMeshProBias;
    public TextMeshPro mTextMeshProWert;
    public TextMeshPro mTextMeshProWeights;

    public TextMeshPro mTextMeshProSollWert;

    public float mSollOutput;

    public float mDiffMal2MalLernRate;

    public void Init(int pNummer, float pBiasStart,float pWeightStart, Dictionary<int, Neuron> mHiddenElemente)
    {
        Initialisiere(pNummer, pBiasStart);

        foreach (var lNeuron in mHiddenElemente)
        {
            mWeightsZuInput.Add(lNeuron.Value, new Weight(pWeightStart));
            lNeuron.Value.mOutputNeuronen.Add(this);
        }

        mTextMeshProName.text = "Y" + mNummer;
        ErmittelOutput();
    }

    void Update()
    {
        mTextMeshProWert.text = string.Format("{0:F2}", LieferOutput());
        mTextMeshProBias.text = string.Format("{0:F2}", LieferBias());
        mTextMeshProWeights.text = string.Format("{0:F2}", LieferWeights());
        mTextMeshProSollWert.text = string.Format("{0:F1}", mSollOutput);
    }
    internal void ErmittelOutput()
    {
        mSumme = ErmittelSumme();
        mOutput = mSumme;
    }

    internal void Change(float pSoll)
    {
        mSollOutput = pSoll;
    }
    internal float ErmittelNeueWeightsAndBias(float pLernrate)
    {
        mDiffMal2MalLernRate = pLernrate * 2 * (LieferOutput() - mSollOutput);

        mBias -= mDiffMal2MalLernRate;

        foreach (var lWeight in mWeightsZuInput)
        {
            lWeight.Value.setWeight(lWeight.Value.mWeight - mDiffMal2MalLernRate * lWeight.Key.LieferOutput());
        }

        return (LieferOutput() - mSollOutput) * (LieferOutput() - mSollOutput);
    }

    public override float LieferCostFunctionAbleitung_DiffMal2MalLernRate()
    {
        return mDiffMal2MalLernRate;
    }
}
