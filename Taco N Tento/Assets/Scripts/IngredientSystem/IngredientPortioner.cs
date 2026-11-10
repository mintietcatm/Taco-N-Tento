using UnityEngine;

public class IngredientPortioner : MonoBehaviour
{
    [SerializeField] private IngredientInventory inventario;
    [SerializeField] private ProductData producto;

    public void Porcionar()
    {
        if (inventario == null|| producto== null)
        {
            return;
        }

        IngredientData ingredienteResultado = producto.ObtenerIngredienteResultado();

    }
}
