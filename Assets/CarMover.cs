using UnityEngine;

public class CarMover : MonoBehaviour
{
    public Transform targetPosition;
    public float speed = 3f;

    void Start()
    {
        targetPosition = new GameObject("Target").transform;
        targetPosition.position = new Vector3(9.13f, -1.99f, 77.83f);

    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
    }

}