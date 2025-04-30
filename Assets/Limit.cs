using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit : MonoBehaviour
{
    public RandomPlanetsGenerator planetsGenerator;
    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
        planetsGenerator.add = true;
    }
}
