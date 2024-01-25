/// ksPark
/// 
/// Input Manager

using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public PathFinder pf;

    public Vector3 targetPos = Vector3.zero;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            Vector3 pos = Input.mousePosition;
            pos.z = Camera.main.farClipPlane;
            targetPos = Camera.main.ScreenToWorldPoint(pos);
            pf.GetPath(targetPos);

            pf.isMove = Input.GetMouseButton(1);
        }
    }
}
