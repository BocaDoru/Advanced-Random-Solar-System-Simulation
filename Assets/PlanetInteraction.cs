using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInteraction : MonoBehaviour
{
    public Planet planet;
    public PlanetAtraction planetAtraction;
    const float G = 6.67428e-11f;
    public void Start()
    {
        gameObject.transform.localScale = Vector3.one;
        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
    }
    void PlasticCollision(Planet otherPlanet)
    {
        float mR = planet.m + otherPlanet.m;
        Vector3 vR = (otherPlanet.v * otherPlanet.m + planet.v * planet.m) / mR;
        planet.m = mR;
        planet.v = vR;
        planet.transform.localScale = Vector3.one * Mathf.Log(mR, 100) / 5f;
        planetAtraction.GetComponent<SphereCollider>().radius = Mathf.Sqrt(planet.m * G * 1e6f);
    }
    void GeneratePlanetInElasticCollision(GameObject newPlanet,Vector3 center, Vector3 direction, float dispersion,float mR,float speed,Vector3 Ec)
    {
         newPlanet.name = RandomPlanetsGenerator.RandomName(5);
         Vector3 randomPoint = Vector3.ProjectOnPlane(Random.onUnitSphere * dispersion, direction) + direction * dispersion;
         newPlanet.transform.position = randomPoint + center;
         newPlanet.AddComponent<Planet>();
         newPlanet.GetComponent<Planet>().m = mR;
         newPlanet.GetComponent<Planet>().v0 = speed * (Ec/dispersion + randomPoint*dispersion).normalized;
    }
    void ElasticCollision(Planet otherPlanet, Vector3 contactVector)
    {
        print('a');
        print(otherPlanet.m);
        print(planet.m);
        float mR = planet.m + otherPlanet.m;//>=2e7f
        float EcMagnitude = 0.5f * (planet.m * planet.v.sqrMagnitude + otherPlanet.m * otherPlanet.v.sqrMagnitude);
        Vector3 EcVector = EcMagnitude * (planet.v + otherPlanet.v).normalized;
        Vector3 direction = (EcVector + contactVector).normalized;
        int numberOfObj = (int)((EcMagnitude != 0 ? Mathf.Log(EcMagnitude, 100) : 2));
        float dispersion = EcMagnitude != 0 ? Mathf.Abs(Mathf.Log(EcMagnitude, 1000)) : 1;
        Vector3 center = transform.position;
        for (int i = 0; i < numberOfObj - 1; i++)
        {
            float randomMass = Random.Range(1, mR - numberOfObj + i - 1);
            float EcRM = EcMagnitude * randomMass / mR;
            float speed = Mathf.Sqrt(2 * EcRM / randomMass);
            GameObject newPlanet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GeneratePlanetInElasticCollision(newPlanet, center, direction, dispersion, randomMass, speed, EcVector);
            mR -= randomMass;
            EcMagnitude -= EcRM;
        }
        Vector3 randomPoint = Vector3.ProjectOnPlane(Random.onUnitSphere * dispersion, direction) + direction * dispersion;
        gameObject.transform.parent.transform.position = randomPoint + center;
        planet.m = mR;
        planet.v = (EcVector/dispersion + randomPoint * dispersion).normalized * Mathf.Sqrt(2 * EcMagnitude / mR);
        planet.transform.localScale = Vector3.one * Mathf.Log(mR, 100) / 5f;
        planetAtraction.GetComponent<SphereCollider>().radius = Mathf.Sqrt(mR * G * 1e6f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (planet.isCollider)
        {
            Planet otherPlanet = other.GetComponentInParent<Planet>();
            otherPlanet.isCollider = false;
            Destroy(otherPlanet.gameObject);
            if ((planet.m > otherPlanet.m ? (planet.m / otherPlanet.m > 100) : (otherPlanet.m / planet.m > 100)) || (otherPlanet.m < 1e9f || planet.m < 1e9f))
                PlasticCollision(otherPlanet);
            else
                ElasticCollision(otherPlanet, transform.position - other.ClosestPoint(transform.position));
        }
        else
            Destroy(planet.gameObject);
    }
}