using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class SteuererNeuronalesNetz : MonoBehaviour
{
    public Aufloesungskuemmer mAufloesungskuemmer;
    public GameObject mPrefabOfInputElement;
    public GameObject mPrefabOfHiddenElement;
    public GameObject mPrefabOfOutputElement;

    public Transform parentTransformInput;

    public Transform parentTransformHidden;

    public Transform parentTransformOutput;

    public Transform mCubeFuerPhase;
    public TextMeshPro mCubeFuerPhaseTextMeshPro;

    public Transform mCubeFuerLostFunction;
    public TextMeshPro mLostFunctionextMeshPro;

    private int mAnzahlInputElemente;
    public int mAnzahlHiddenElemente;
    private int mAnzahlOutputElemente;

    private Dictionary<int, EingabeObjekt> mInputElemente;
    private Dictionary<int, AusgabeObjekt> mOutputElemente;
    private Dictionary<int, Neuron> mHiddenElemente;

    public Dictionary<int, EinLernenderVersuch> mZuLernendesMaterial;

    public int mSchritt;

    public int mGeklickt;

    public float[] mInput;


    void Start()
    {
        mGeklickt = 0;
        mSchritt = 0;
        mZuLernendesMaterial = new Dictionary<int, EinLernenderVersuch>();
        mZuLernendesMaterial.Add(1, new EinLernenderVersuch(new LernFeld(10, 20, 30), new LernFeld(55, 65)));
        mZuLernendesMaterial.Add(2, new EinLernenderVersuch(new LernFeld(15, 25, 35), new LernFeld(85, 85)));
        mZuLernendesMaterial.Add(3, new EinLernenderVersuch(new LernFeld(45, 55, 65), new LernFeld(155, 165)));
        mZuLernendesMaterial.Add(4, new EinLernenderVersuch(new LernFeld(38, 48, 58), new LernFeld(108, 111)));
        mAnzahlInputElemente = 3;
        mAnzahlOutputElemente = 2;
        mInput = new float[mAnzahlInputElemente];

        mCubeFuerPhase.position = new Vector3(mAufloesungskuemmer.lieferPosCubeFuerPhase().x, mAufloesungskuemmer.lieferPosCubeFuerPhase().y, 10);
        mCubeFuerPhaseTextMeshPro.text = "idle";   
        mCubeFuerLostFunction.position = new Vector3(mAufloesungskuemmer.lieferPosCubeFuerLostFunction().x, mAufloesungskuemmer.lieferPosCubeFuerLostFunction().y, 10);
        mLostFunctionextMeshPro.text = "nichts";
        erstelleXElemente();

        erstelleHiddenElemente();

        erstelleYElemente();
    }

    private void erstelleXElemente()
    {
        mInputElemente = new Dictionary<int, EingabeObjekt>();

        List<Vector2> lPositionen = mAufloesungskuemmer.ErmittelXPostionen(mAnzahlInputElemente);

        for (int i = 0; i < mAnzahlInputElemente; i++)
        {
            GameObject lGameObject = Instantiate(mPrefabOfInputElement, parentTransformInput);
            EingabeObjekt lEingabeObjekt = lGameObject.GetComponent<EingabeObjekt>();
            mInputElemente.Add(i, lEingabeObjekt);
            float lStartWert = 9 + i + ((float)i) / 100f;
            lEingabeObjekt.Init(i);
            lEingabeObjekt.Change(lStartWert);
            lEingabeObjekt.GetComponent<Transform>().position = new Vector3(lPositionen[i].x, lPositionen[i].y, 10);
        }
    }

    private void erstelleHiddenElemente()
    {
        mHiddenElemente = new Dictionary<int, Neuron>();

        List<Vector2> lPositionen = mAufloesungskuemmer.ErmittelHPostionen(mAnzahlHiddenElemente);
        for (int i = 0; i < mAnzahlHiddenElemente; i++)
        {
            GameObject lGameObject = Instantiate(mPrefabOfHiddenElement, parentTransformHidden);
            Neuron lNeuron = lGameObject.GetComponent<Neuron>();
            mHiddenElemente.Add(i, lNeuron);
            lNeuron.Init(i, ErmittelStartWeight(i,2.5f), ErmittelStartWeight(i, 0.01f), mInputElemente);
            lNeuron.GetComponent<Transform>().position = new Vector3(lPositionen[i].x, lPositionen[i].y, 10);
        }
    }

    

    private void erstelleYElemente()
    {
        mOutputElemente = new Dictionary<int, AusgabeObjekt>();

        List<Vector2> lPositionen = mAufloesungskuemmer.ErmittelYPostionen(mAnzahlOutputElemente);
        for (int i = 0; i < mAnzahlOutputElemente; i++)
        {
            GameObject lGameObject = Instantiate(mPrefabOfOutputElement, parentTransformOutput);
            AusgabeObjekt lAusgabeObjekt = lGameObject.GetComponent<AusgabeObjekt>();
            mOutputElemente.Add(i, lAusgabeObjekt);
            lAusgabeObjekt.Init(i, ErmittelStartWeight(i,1.5f), ErmittelStartWeight(i,0.02f), mHiddenElemente);
            lAusgabeObjekt.GetComponent<Transform>().position = new Vector3(lPositionen[i].x, lPositionen[i].y, 10);
            lAusgabeObjekt.Change(99.9f + 10 * i);
        }
    }

    private static float ErmittelStartWeight(int i,float pStart)
    {
        return i * 0.02f + 0.7f+ pStart;
    }

    private static float ErmittelStartBias(int i,float pStart)
    {
        return 6.75f + i * .5f;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray lRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit lRaycastHit;

            if (Physics.Raycast(lRay, out lRaycastHit))
            {
                string lName = lRaycastHit.transform.name;

                if (lName.Equals("PhasenCube"))
                {
                    mGeklickt++;
                    // Debug.Log("Treffer:" + lName);
                }
            }

            if (mGeklickt >= 2)
            {
                mSchritt = mGeklickt - 1;
                SetzemSchrittZurueck();

                mCubeFuerPhaseTextMeshPro.text ="Set-"+mSchritt;
                
                EineVarianteSetzen();
            }

            VorwaertsPropogation();
        }

        if (mGeklickt == 1)
        {
            mCubeFuerPhaseTextMeshPro.text = "lerne";
            Anlernen();
        }
    }

    private void Anlernen()
    {
        float lLernrate = 0.00000002f;

        // Lernmaterial einbilden
        mSchritt++;
        SetzemSchrittZurueck();
      
        EineVarianteSetzen();

        VorwaertsPropogation();

        BackPropagation(lLernrate);
    }

    private void EineVarianteSetzen()
    {
        foreach (var lInputNeuron in mInputElemente)
        {
            lInputNeuron.Value.Change(mZuLernendesMaterial[mSchritt].mInput.mWerte[lInputNeuron.Key]);
        }
        foreach (var lOutputNeuron in mOutputElemente)
        {
            lOutputNeuron.Value.Change(mZuLernendesMaterial[mSchritt].mOutput.mWerte[lOutputNeuron.Key]);
        }
    }

    private void SetzemSchrittZurueck()
    {
        if (mSchritt > mZuLernendesMaterial.Count)
        {
            mSchritt = 1;
        }
    }

    private float BackPropagation(float lLernrate)
    {
        float lErg = 0;

        foreach (var lOutputNeuron in mOutputElemente)
        {
            lErg = lErg + lOutputNeuron.Value.ErmittelNeueWeightsAndBias(lLernrate);
        }

        foreach (var lNeuron in mHiddenElemente)
        {
            lNeuron.Value.ErmittelNeueWeightsAndBias(lLernrate);
        }

        return lErg;
    }

    private void VorwaertsPropogation()
    {
        foreach (var lNeuron in mHiddenElemente)
        {
            lNeuron.Value.ErmittelOutput();
        }
        foreach (var lOutputNeuron in mOutputElemente)
        {
            lOutputNeuron.Value.ErmittelOutput();
        }
    }
}

