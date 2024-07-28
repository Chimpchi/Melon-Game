using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Assets/Planet", order = 1)]
public class PlanetScriptObject : ScriptableObject
{

    public string planetName = "Planet";
    public Vector3 scale = Vector3.one;
    public float mass = 1;
    public Color color = Color.white;

}
