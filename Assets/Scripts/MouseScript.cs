using Unity.VisualScripting;
using UnityEngine;

public class MouseScript : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetMouseButtonDown((int) MouseButton.Left))
        {
            PlanetManager.Instance.DropPlanet();
        }
    }

}
