using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Efficiency : MonoBehaviour
{
    public float numberOfCalculation;
    public float density;
    int objectTotalDensity = 0;
    int exp = 0;
    public int majorPlanets = 0;
    public int asteroidNumber;
    public int objectInstantDensity =0;
    public float accelerationLimit;
    public float mediumRadius;
    public float mediumMass;
    public float massOfMajorPlanets;
    const float G = 6.67428e-11f;
    public List<Planet> planets = new List<Planet>();
    public List<Planet> majorPlanetsList = new List<Planet>();
    public void Calculate()
    {
        mediumMass = 0;
        majorPlanets = 0;
        planets.Clear();
        planets.AddRange(FindObjectsOfType<Planet>());
        majorPlanetsList.Clear();
        for (int i=0;i<planets.Count;i++)
        {
            if (planets[i].m < massOfMajorPlanets)
                mediumMass += planets[i].m;
            else
            {
                majorPlanetsList.Add(planets[i]);
                majorPlanets++;
            }
        }
        asteroidNumber = planets.Count - majorPlanets;
        mediumMass /= asteroidNumber;
        mediumRadius = Mathf.Sqrt(mediumMass * G * accelerationLimit);
        gameObject.GetComponent<SphereCollider>().radius = mediumRadius;
    }
    public void Update()
    {
        Calculate();
        objectTotalDensity += objectInstantDensity;
        exp++;
        density = (float)objectTotalDensity / exp + majorPlanets;
        numberOfCalculation = planets.Count * density / Time.fixedDeltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        objectInstantDensity ++;
    }
    private void OnTriggerExit(Collider other)
    {
        objectInstantDensity --;
    }
}
