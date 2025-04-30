using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float m;
    public Vector3 v0;
    public Vector3 a;
    public Vector3 a2;
    public Vector3 v;
    public bool isCollider = true;
    public Planet planetOfOrbit;
    public List<Planet> satelits = new List<Planet>();
    void Start()
    {
        v = v0;
        gameObject.layer = 6;
        transform.localScale = Vector3.one * Mathf.Log(m, 100) / 5f;
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        /*gameObject.AddComponent<TrailRenderer>();
        gameObject.GetComponent<TrailRenderer>().time = 100f;
        gameObject.GetComponent<TrailRenderer>().widthMultiplier = 0.3f;*/
        gameObject.GetComponent<Renderer>().receiveShadows = false;
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        gameObject.GetComponent<Renderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        gameObject.GetComponent<Renderer>().reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        GameObject planetAtractionObj = new GameObject(name+1);
        planetAtractionObj.layer = 7;
        planetAtractionObj.transform.parent = transform;
        planetAtractionObj.transform.position = transform.position;
        planetAtractionObj.AddComponent<PlanetAtraction>().planet = this;
        GameObject planetInteractionObj = new GameObject(name + 2);
        planetInteractionObj.layer = 8;
        planetInteractionObj.transform.parent = transform;
        planetInteractionObj.transform.position = transform.position;
        planetInteractionObj.AddComponent<PlanetInteraction>().planet = this;
        planetInteractionObj.GetComponent<PlanetInteraction>().planetAtraction = planetAtractionObj.GetComponent<PlanetAtraction>();
    }
    void FixedUpdate()
    {
        satelits.RemoveAll(p => p == null);
        v += a * Time.fixedDeltaTime;
        transform.position += v * Time.fixedDeltaTime;
    }
    public void OnBecameInvisible()
    {
        GetComponent<MeshRenderer>().enabled = false;
        //GetComponent<TrailRenderer>().enabled = false;
    }
    public void OnBecameVisible()
    {
        GetComponent<MeshRenderer>().enabled = true;
        //GetComponent<TrailRenderer>().enabled = true;
    }
}
