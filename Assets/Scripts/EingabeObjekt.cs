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

    public void Init(int pNummer) {
        Initialisiere(pNummer,0f);
        mTextMeshProName.text = "X" + mNummer;
    }

     public void Change( float pStartwert) {
        mInputwert = pStartwert;
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
