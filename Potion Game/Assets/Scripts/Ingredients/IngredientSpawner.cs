using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientSpawner : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject ingredient;

    private GameObject recentIngredient;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = ingredient.GetComponent<SpriteRenderer>().sprite; // set sprite to current ingredient

        recentIngredient = Instantiate(ingredient, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (recentIngredient.GetComponent<TargetJoint2D>() != null)
        {
            recentIngredient = Instantiate(ingredient, transform.position, Quaternion.identity);
        }
    }

    public void Clicked()
    {
        //Instantiate(ingredient, transform.position, Quaternion.identity);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        //recentIngredient = Instantiate(ingredient, transform.position, Quaternion.identity);
    }
    
}
