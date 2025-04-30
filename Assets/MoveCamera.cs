using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Planet curentCBody;
    public Planet Sun;
    public SelectCelestialBody select;
    public void Update()
    {
        if(curentCBody!=null)
        {
            transform.position = curentCBody.transform.position;
            transform.LookAt(curentCBody.planetOfOrbit.transform, (curentCBody.transform.position - curentCBody.planetOfOrbit.transform.position).normalized);
        }
        else
        {
            curentCBody = select.back[select.back.Count - 1];
            select.GenerateButtonListForLastBody(curentCBody);
        }
    }
}
