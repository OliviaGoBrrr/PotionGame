using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientSpawner : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject ingredient;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = ingredient.GetComponent<SpriteRenderer>().sprite; // set sprite to current ingredient

        Instantiate(ingredient, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IsClicked()
    {
        Instantiate(ingredient, transform.position, Quaternion.identity);
        Debug.Log("HELLO GOOBERS!!");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("HELLO GOOBERS!!");
    }
}
