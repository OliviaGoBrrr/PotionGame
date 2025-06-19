using UnityEngine;

public class IngredientBounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x > 1.1 || pos.x < -0.1)
        {
            Destroy(this.gameObject);
        }

        if (pos.y > 3 || pos.y < -0.1)
        {
            Destroy(this.gameObject);
        }
    }
}
