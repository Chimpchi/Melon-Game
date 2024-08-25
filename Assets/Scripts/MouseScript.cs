using UnityEngine;

public class MouseScript : MonoBehaviour
{

    [Min(0f)] [SerializeField] private float dropRate = 0.2f;

    private float lastDropTime;

    private void Update()
    {
        if (Time.time - lastDropTime < dropRate || !Input.GetKeyDown(KeyCode.Mouse0) || GameManager.GetInstance().currentState != GameState.Gameplay)
            return;

        MelonManager.GetInstance().DropMelon();
        lastDropTime = Time.time;
    }

}
