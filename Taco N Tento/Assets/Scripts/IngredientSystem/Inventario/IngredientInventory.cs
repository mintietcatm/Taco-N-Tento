using System.Collections.Generic;
using UnityEngine;

public class IngredientInventory : MonoBehaviour
{
    private Dictionary<IngredientData, int> ingredients = new Dictionary<IngredientData, int>();

    public void AgregarIngredientes(IngredientData ingrediente, int cantidad)
    {

        if (ingrediente == null || cantidad <= 0) //si el valor es nulo O es menor que 0 
        {
            return; //termina el metodo, como el metodo es un VOID no necesita devolver ningun valor. 
        }

        if (ingredients.ContainsKey(ingrediente)) //Revisa si YA esta registrado el ingrediente. ContainsKey te da un bool
        {
            ingredients[ingrediente] += cantidad; //Si ya existe, le sumas al valor de cantidad acutal.
        }

        else //si NO esta registrado
        {
            ingredients.Add(ingrediente, cantidad); //pues lo agrega 
        }

    }

    public int ObtenerCantidad(IngredientData ingrediente) //si recibe un ingrediente vacio o no registrado devuelve cero. va a funcionar para cuando haya que ponerlo en un recipiente
    {
        if (ingrediente == null)
        {
            return 0;
        }

        if (ingredients.ContainsKey(ingrediente))
        {

            return ingredients[ingrediente];
        }

        return 0;
    }

    public bool RetirarIngredientes(IngredientData ingrediente, int cantidad)
    {
        if (ingrediente == null || cantidad <= 0) //si la cantidad es nula o menor o igual a cero 
        {
            return false;
        }

        if (ObtenerCantidad(ingrediente) < cantidad) // revisa si hay ingredientes disponibles
        {
            return false;
        }

        ingredients[ingrediente] -= cantidad;
        return true;

    }


}
