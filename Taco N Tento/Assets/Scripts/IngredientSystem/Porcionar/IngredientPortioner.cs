using UnityEngine;

public class IngredientPortioner : MonoBehaviour
{
    [SerializeField] private IngredientInventory inventario;
    

    
    public bool Porcionar(ProductData producto)
    {
        if (inventario == null|| producto== null)
        {
            return false;
        }

        IngredientData ingredienteResultado = producto.ObtenerIngredienteResultado();
        int cantidad = producto.ObtenerCantidadProducida();
        inventario.AgregarIngredientes(ingredienteResultado, cantidad);
        Debug.Log("Cantidad guardada en inventario: " + inventario.ObtenerCantidad(ingredienteResultado));
        return true;
    }
}
