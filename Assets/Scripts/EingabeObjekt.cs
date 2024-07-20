using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EingabeObjekt : NeuronBase
{
    public TextMeshPro mTextMeshProName;
    public TextMeshPro mTextMeshProWert;

    public float mInputwert;

    public void Init(int pNummer, float pStartwert) {
        Initialisiere(pNummer,0f);
        mInputwert = pStartwert;
        mTextMeshProName.text = "X" + mNummer;
    }

    void Update()
    {
         mTextMeshProWert.text = string.Format("{0:F2}", LieferOutput());
    }

    public override float LieferOutput()
    {
        return mInputwert;
    }
}
