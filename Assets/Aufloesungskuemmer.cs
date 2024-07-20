using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aufloesungskuemmer : MonoBehaviour
{
    public float mHalfWidth;
    public float mHalfHeight;
    void Start()
    {
        float orthoSize = Camera.main.orthographicSize;

        mHalfWidth = orthoSize * Camera.main.aspect;
        mHalfHeight = orthoSize;
    }

    public List<Vector2> ErmittelXPostionen(int pAnzahl)
    {
        List<Vector2> lErg;
        float lHoeheProElement;
        ermittelBasisDaten(pAnzahl, out lErg, out lHoeheProElement);

        for (int i = pAnzahl - 1; i >= 0; i--)
        {
            lErg.Add(new Vector2(mHalfWidth * (-1) + mHalfWidth / 12, lHoeheProElement * i + 1.5f * lHoeheProElement - mHalfHeight));
        }

        return lErg;
    }

    public List<Vector2> ErmittelYPostionen(int pAnzahl)
    {
        List<Vector2> lErg;
        float lHoeheProElement;
        ermittelBasisDaten(pAnzahl, out lErg, out lHoeheProElement);

        for (int i = pAnzahl - 1; i >= 0; i--)
        {
            lErg.Add(new Vector2(mHalfWidth - mHalfWidth / 6, lHoeheProElement * i + 1.5f * lHoeheProElement - mHalfHeight));
        }

        return lErg;
    }

    public List<Vector2> ErmittelHPostionen(int pAnzahl)
    {
        List<Vector2> lErg;
        float lHoeheProElement;
        ermittelBasisDaten(pAnzahl, out lErg, out lHoeheProElement);

        for (int i = pAnzahl - 1; i >= 0; i--)
        {
            int lHaelfteI = i / 2;
            if (lHaelfteI * 2 == i)
            {
                lErg.Add(new Vector2((-1) * mHalfWidth / 8, lHoeheProElement * i + 1.5f * lHoeheProElement - mHalfHeight));
            }
            else
            {
                lErg.Add(new Vector2(mHalfWidth / 8, lHoeheProElement * i + 1.5f * lHoeheProElement - mHalfHeight));
            }
        }
        return lErg;
    }

    private void ermittelBasisDaten(int pAnzahl, out List<Vector2> lErg, out float lHoeheProElement)
    {
        lErg = new List<Vector2>();
        int lAnzahlMitPlatzhalterElement = pAnzahl + 2;
        float lHoeheInsgesamt = 2 * mHalfHeight;
        lHoeheProElement = lHoeheInsgesamt / lAnzahlMitPlatzhalterElement;
    }
}
