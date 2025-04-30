using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAtraction : MonoBehaviour
{
    const float G = 6.67428e-11f;
    public List<Planet> planetsAtractionList = new List<Planet>();
    public Planet planet;
    void Start()
    {
        gameObject.transform.localScale = Vector3.one;
        gameObject.AddComponent<SphereCollider>().radius = Mathf.Sqrt(planet.m * G * 1e5f);
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
    }
    public void FixedUpdate()
    {
        Vector3 a = Vector3.zero;
        planetsAtractionList.RemoveAll(Planet => Planet == null);
        for (int i = 0; i < planetsAtractionList.Count; i++)
        {
            Vector3 R = planetsAtractionList[i].gameObject.transform.position - planet.gameObject.transform.position;
            Vector3 RNormalized = R.normalized;
            float RInvSqrMagnitude = 1f / R.sqrMagnitude;
            a += planetsAtractionList[i].m * RInvSqrMagnitude * RNormalized;
        }
        planet.a = a * G;
    }
    public void OnTriggerEnter(Collider other)
    {
        PlanetAtraction otherPlanetAtraction = other.GetComponentInChildren<PlanetAtraction>();
        otherPlanetAtraction.planetsAtractionList.Add(planet);
    }
    public void OnTriggerExit(Collider other)
    {
        PlanetAtraction otherPlanetAtraction = other.GetComponentInChildren<PlanetAtraction>();
        otherPlanetAtraction.planetsAtractionList.Remove(planet);
    }
}
