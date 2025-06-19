using UnityEngine;

[CreateAssetMenu(fileName = "IngredientScriptableObject", menuName = "Scriptable Objects/IngredientScriptableObject")]
public class IngredientScriptableObject : ScriptableObject
{
    [SerializeField] public float temperature;

    [SerializeField] public float carbonation;

    [SerializeField] public float pazaz;

    [SerializeField] public float potency;
}
