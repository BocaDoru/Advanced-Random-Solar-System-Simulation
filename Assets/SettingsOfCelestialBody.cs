using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SettingsOfCelestialBody : ScriptableObject
{
    public int n;
    public int nameLenght;
    public Vector3 reachBounds;
    public Vector2 massBounds;
    public Vector2 speedError;
    public Vector3 velocityNormalVector;
    public Vector3 planNormalVector;
    public float minDistance;
    public Vector2 satelitLocation;
}
