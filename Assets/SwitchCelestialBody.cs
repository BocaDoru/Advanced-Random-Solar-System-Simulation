using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SwitchCelestialBody : MonoBehaviour
{
    public Planet cBody;
    public SelectCelestialBody select;
    public Text text;
    public void Update()
    {
        if (cBody == null)
        {
            select.GenerateButtonListForLastBody(select.curentCBody);
            Destroy(gameObject);
        }
        else
        {
            text.text = text.text.Split(':')[0] + ':' + cBody.satelits.Count;
        }
    }
    public void OnClick()
    {
        select.back.Add(select.curentCBody);
        select.curentCBody = cBody;
        select.mainCamera.curentCBody = cBody;
        select.GenerateButtonListForLastBody(cBody);
    }
}
