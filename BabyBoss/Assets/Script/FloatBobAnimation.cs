using UnityEngine;

public class FloatBobAnimation : MonoBehaviour
{
    public float floatStrength = 0.05f;
    public float floatSpeed = 2f;

    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.localPosition = new Vector3(startLocalPos.x, startLocalPos.y + yOffset, startLocalPos.z);
    }
}
