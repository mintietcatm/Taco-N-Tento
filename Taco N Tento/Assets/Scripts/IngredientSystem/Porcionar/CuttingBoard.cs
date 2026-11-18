using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    [SerializeField] private IngredientInventory inventario; //almacena las porciones 
    private ProductInstance productoActual; //almacena que producto estamos cortando

    public bool ColocarProducto (ProductInstance producto)
    {
        if (productoActual != null || producto == null)
        {
            return false;
        }

        productoActual = producto;
        return true;
    }

    public void RetirarProducto()
    {
        productoActual=null;
    }

    public bool Cortar()
    {
        if (productoActual ==null)
        {
            return false;
        }
        ProductData datosProducto = productoActual.ObtenerProducto();

        if (datosProducto==null)
        {
            return false;
        }

        IngredientData ingredienteResultado = datosProducto.ObtenerIngredienteResultado();

        if (ingredienteResultado==null||inventario== null)
        {
            return false;
        }

        int porcionesRestantes = productoActual.ObtenerCantidadRestante();
        if (porcionesRestantes < 2)
        {
            return false;
        }

        int porcionesACortar = 1;

        if (porcionesRestantes==2)
        {
            porcionesACortar = 2;
        }

        if (!productoActual.RetirarPorciones(porcionesACortar))
        {
            return false;
        }

        inventario.AgregarIngredientes(ingredienteResultado,  porcionesACortar);

        if (productoActual.ObtenerCantidadRestante() <= 0)
        {
            Destroy(productoActual.gameObject);
            RetirarProducto();
        }

        return true;

    }
}
