using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlanetsGenerator : MonoBehaviour
{
    enum TypeOfCelestialBody { Planet, Moon, Asteroid };
    public SettingsOfCelestialBody innerRockyPlanetsSettings;
    public SettingsOfCelestialBody outerGasGiantsSettings;
    public SettingsOfCelestialBody dwarfPlanetsSettings;
    public SettingsOfCelestialBody asteroidBeltSettings;
    public SettingsOfCelestialBody kuiperBeltSettings;
    public List<Planet> planets=new List<Planet>();
    public Planet Sun;
    public bool add=false;
    const float G = 6.67428e-11f;
    public Efficiency efficiency;
    void Start()
    {
        innerRockyPlanetsSettings.n = 5;
        outerGasGiantsSettings.n = 5;
        dwarfPlanetsSettings.n = 2;
        Sun.satelits = GenerateCelestialBodyGrup(Sun, innerRockyPlanetsSettings);
        Sun.satelits.AddRange(GenerateCelestialBodyGrup(Sun, outerGasGiantsSettings));
        Sun.satelits.AddRange(GenerateCelestialBodyGrup(Sun, dwarfPlanetsSettings));
        Sun.satelits.AddRange(GenerateCelestialBodyGrup(Sun, asteroidBeltSettings));
        Sun.satelits.AddRange(GenerateCelestialBodyGrup(Sun, kuiperBeltSettings));
        efficiency.Calculate();
    }
    public static string RandomName(int lenght)
    {
        string randomName = "";
        const string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        for(int i=0;i<lenght;i++)
        {
            randomName += alpha[(int)Random.Range(0, alpha.Length)];
        }
        //randomName += (int)Random.Range(0, 1000);
        return randomName;
    }
    List<Planet> GenerateCelestialBodyGrup(Planet cBodyOfOrbit,SettingsOfCelestialBody settings)
    {
        List<Planet> celestialBodyGrup = new List<Planet>();
        for (int i = 0; i < settings.n; i++)
        {
            Planet cBody = Generate(cBodyOfOrbit, cBodyOfOrbit.transform.position, cBodyOfOrbit.v0, settings, celestialBodyGrup);
            if (cBody != null)
            {
                celestialBodyGrup.Add(cBody);
                float R = (cBody.transform.position - cBodyOfOrbit.transform.position).sqrMagnitude;
                float maxDistanceToPlanet = Mathf.Sqrt(cBody.m * R / Sun.m);
                if (0.1f * maxDistanceToPlanet > Mathf.Log(cBody.m, 100) / 5f)
                    if (Random.Range(0, 5) < 3)
                    {
                        SettingsOfCelestialBody moonsSettings = SettingsOfCelestialBody.CreateInstance<SettingsOfCelestialBody>();
                        moonsSettings.n = (int)Random.Range(0, Mathf.Log(cBody.m, 100));
                        moonsSettings.nameLenght = 2;
                        Vector3 vector3 = new Vector3(settings.satelitLocation.x, settings.satelitLocation.y, 0.01f) * maxDistanceToPlanet;
                        moonsSettings.reachBounds = vector3;
                        Vector2 vector2 = new Vector2(settings.massBounds.x / 2, Mathf.Log(cBody.m, 100));
                        moonsSettings.massBounds = vector2;
                        moonsSettings.speedError = new Vector2(Random.Range(0f, 0.05f), Random.Range(0f, 0.05f));
                        moonsSettings.velocityNormalVector = Random.onUnitSphere;
                        moonsSettings.planNormalVector = moonsSettings.velocityNormalVector;
                        moonsSettings.satelitLocation = new Vector2(0.1f, 0.9f);
                        cBody.satelits = GenerateCelestialBodyGrup(cBody, moonsSettings);
                    }
                    else if(cBody.m>=1e15f)
                    {
                        SettingsOfCelestialBody ringSettings = SettingsOfCelestialBody.CreateInstance<SettingsOfCelestialBody>();
                        ringSettings.n = (int)Random.Range(1, Mathf.Log(cBody.m, 10)) * 100;
                        ringSettings.nameLenght = 3;
                        Vector3 vector3 = new Vector3(settings.satelitLocation.x, settings.satelitLocation.y, 0.01f) * maxDistanceToPlanet;
                        ringSettings.reachBounds = vector3;
                        Vector2 vector2 = new Vector2(settings.massBounds.x / 3, Mathf.Log(cBody.m, 1000));
                        ringSettings.massBounds = vector2;
                        ringSettings.speedError = new Vector2(Random.Range(0f, 0.07f), Random.Range(0f, 0.07f));
                        ringSettings.velocityNormalVector = Random.onUnitSphere;
                        ringSettings.planNormalVector = ringSettings.velocityNormalVector;
                        ringSettings.satelitLocation = new Vector2(0.1f, 0.9f);
                        cBody.satelits = GenerateCelestialBodyGrup(cBody, ringSettings);
                    }
            }
        }
        return celestialBodyGrup;
    }
    Planet Generate(Planet planetOfOrbit, Vector3 planetOfOrbitPosition, Vector3 vOfPlanetOfOrbit, SettingsOfCelestialBody settings,List<Planet> neighboringPlanets)
    {
        GameObject p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        p.name = RandomName(settings.nameLenght);
        p.AddComponent<Planet>();
        float m = Mathf.Pow(10f, Random.Range(settings.massBounds.x, settings.massBounds.y));
        p.GetComponent<Planet>().m = m;
        float down, up;
        if (m < Mathf.Pow(10f, 0.5f * (settings.massBounds.x + settings.massBounds.y)))
        {
            down = settings.reachBounds.x;
            up = 0.5f*(settings.reachBounds.x+settings.reachBounds.y);
        }
        else
        {
            down = 0.5f * (settings.reachBounds.x + settings.reachBounds.y);
            up = settings.reachBounds.y;
        }
        Vector3 pointOnUnitSphere = Random.onUnitSphere;
        Vector3 randomPosition = Vector3.ProjectOnPlane(pointOnUnitSphere, settings.planNormalVector).normalized * Random.Range(down, up) + planetOfOrbitPosition + settings.planNormalVector * Random.Range(-settings.reachBounds.z, settings.reachBounds.z);
        int k = 0;
        int maxTry = 0;
        while(k<neighboringPlanets.Count&&maxTry<50)
        {
            if (Mathf.Abs(randomPosition.magnitude - neighboringPlanets[k].transform.position.magnitude) < settings.minDistance)
            {
                randomPosition = Vector3.ProjectOnPlane(pointOnUnitSphere, settings.planNormalVector).normalized * Random.Range(down, up) + planetOfOrbitPosition + settings.planNormalVector * Random.Range(-settings.reachBounds.z, settings.reachBounds.z);
                k = 0;
                maxTry++;
            }
            else
                k++;
        }
        if(maxTry==50)
        {
            settings.n = 0;
            Destroy(p);
            return null;
        }
        p.transform.position = randomPosition;
        p.GetComponent<Planet>().v0 = vOfPlanetOfOrbit + Mathf.Sqrt(G * planetOfOrbit.m / (p.transform.position - planetOfOrbitPosition).magnitude) * Random.Range(1f - settings.speedError.x, 1f + settings.speedError.y) * Vector3.Cross((planetOfOrbitPosition - p.transform.position).normalized, settings.velocityNormalVector);
        p.GetComponent<Planet>().planetOfOrbit = planetOfOrbit;
        return p.GetComponent<Planet>();
    }

}