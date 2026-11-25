using UnityEngine;

public class CookingStation : MonoBehaviour
{
   public bool ColocarIngrediente (IngredientInstance ingrediente)
    {
        if (ingrediente == null)
        {
            return false;
        }

        return ingrediente.IniciarCoccion();
    }
}
