using UnityEngine;

public class IngredientContainer : MonoBehaviour
{
    [SerializeField] private IngredientData ingredienteActual; //guarda que ingrediente esta almacenado en el recipiente
    [SerializeField] private int capacidadMaxima = 20;
    [SerializeField] private int cantidadActual = 0;
}
