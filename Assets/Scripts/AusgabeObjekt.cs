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

        foreach (var lInpurneuron in mHiddenElemente)
        {
            mWeightsZuInput.Add(lInpurneuron.Value, new Weight(pWeightStart));
        }

        mTextMeshProName.text = "Y" + mNummer;
    }

    void Update()
    {
        mTextMeshProWert.text = string.Format("{0:F2}", LieferOutput());
        mTextMeshProBias.text = string.Format("{0:F2}", LieferBias());
        mTextMeshProWeights.text = string.Format("{0:F2}", LieferWeights());
        mTextMeshProSollWert.text = string.Format("{0:F1}", mSollOutput);
    }
    private void ErmittelOutput()
    {
        mSumme = ErmittelSumme();
        mOutput = mSumme;
    }

    internal void SetSollWert(float pSoll)
    {
        mSollOutput = pSoll;
    }
    internal void ErmittelNeueWeightsAndBias(float pLernrate)
    {
        mDiffMal2MalLernRate = pLernrate * 2 * (LieferOutput() - mSollOutput);

        mBias -= mDiffMal2MalLernRate;

        foreach (var lWeight in mWeightsZuInput)
        {
            lWeight.Value.setWeight(lWeight.Value.mWeight - mDiffMal2MalLernRate * lWeight.Key.LieferOutput());
        }
    }

    public override float LieferCostFunctionAbleitung_DiffMal2MalLernRate()
    {
        return mDiffMal2MalLernRate;
    }
}
