using UnityEngine;

[CreateAssetMenu(fileName = "Melon", menuName = "Assets/Melon", order = 1)]
public class MelonScriptObject : ScriptableObject
{

    public string melonName = "Melon";
    public int mergePoints = 50;
    public float scale = 1f;
    public float mass = 1f;
    public Sprite sprite;
    public Color color = Color.white;

}
