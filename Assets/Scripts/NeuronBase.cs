using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Text;

public class NeuronBase : MonoBehaviour
{

  public Dictionary<NeuronBase, Weight> mWeightsZuInput;
  public float mBias;
  public float mOutput;
  public float mSumme;

  public List<NeuronBase> mOutputNeuronen;

  public int mNummer;

  public void Initialisiere(int pNummer,float pBiasStart)
  {
    mNummer = pNummer;
    mBias = pBiasStart;
    mOutput = pBiasStart;
    mBias = pBiasStart;
    mSumme = pBiasStart;
    mWeightsZuInput = new Dictionary<NeuronBase, Weight>();
    mOutputNeuronen = new List<NeuronBase>();
  }

  public string LieferWeights()
  {
    StringBuilder lStringBuilder = new StringBuilder();

    foreach (var lWeight in mWeightsZuInput)
    {
      lStringBuilder.AppendLine("w" + lWeight.Key.mNummer + "=" + string.Format("{0:F2}", lWeight.Value.mWeight));
    }

    return lStringBuilder.ToString();
  }

  public string LieferBias()
  {
    return "b" + mNummer + "=" + string.Format("{0:F2}", mBias);
  }

  public float ErmittelSumme()
  {
    float lErg = mBias;

    foreach (var lWeight in mWeightsZuInput)
    {
      lErg += lWeight.Key.LieferOutput() * lWeight.Value.mWeight;
    }

    return lErg;
  }

  public virtual float LieferOutput()
  {
    return mOutput;
  }

    public virtual float LieferCostFunctionAbleitung_DiffMal2MalLernRate()
    {
        return 1; 
    }

     public virtual float LieferWeightZuInputNeuron(NeuronBase pNeuronBaseBezug)
    {
        return mWeightsZuInput[pNeuronBaseBezug].mWeight; 
    }
}
