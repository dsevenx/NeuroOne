using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SteuererNeuronalesNetz : MonoBehaviour
{
    public Aufloesungskuemmer mAufloesungskuemmer;
    public GameObject mPrefabOfInputElement;
    public GameObject mPrefabOfHiddenElement;
    public GameObject mPrefabOfOutputElement;

    public Transform parentTransformInput;

    public Transform parentTransformHidden;

    public Transform parentTransformOutput;

    public int mAnzahlInputElemente;
    public int mAnzahlHiddenElemente;
    public int mAnzahlOutputElemente;
  
    private Dictionary<int, EingabeObjekt> mInputElemente;
    private Dictionary<int, AusgabeObjekt> mOutputElemente;
    private Dictionary<int, Neuron > mHiddenElemente;

    void Start()
    {
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
            float lStartWert = 10 + i + ((float)i) / 100f;
            lEingabeObjekt.Init(i, lStartWert);
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
            lNeuron.Init(i,0.75f+i,i*0.2f+0.3f,mInputElemente);
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
            lAusgabeObjekt.Init(i,1.5f+i,i*1.2f+1.3f,mHiddenElemente);
            lAusgabeObjekt.GetComponent<Transform>().position = new Vector3(lPositionen[i].x, lPositionen[i].y, 10);
            lAusgabeObjekt.SetSollWert(99.9f);
        }
    }

    void Update()
    {

    }
}
