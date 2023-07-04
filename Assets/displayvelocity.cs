using UnityEngine;
using UnityEngine.UI;

public class displayvelocity : MonoBehaviour
{
    // Velocity of the car
    private Vector3 velocity;

    // UI text element to display velocity
    public Text velocityText;

    // Rigidbody attached to the car
    private Rigidbody rb;

    void Start()
    {
        // Get reference to the rigidbody
        rb = GetComponent<Rigidbody>();
        RectTransform rectTransform = velocityText.GetComponent<RectTransform>();

        // Set the size of the text element
        rectTransform.sizeDelta = new Vector2(200, 50);

        // Set the position of the text element
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    void Update()
    {
        // Calculate the velocity of the car
        velocity = rb.velocity;
        Debug.Log("velocity = " + velocity);
        // Update the UI text element with the velocity
        velocityText.text = "Velocity: " + velocity.ToString();
    }
}