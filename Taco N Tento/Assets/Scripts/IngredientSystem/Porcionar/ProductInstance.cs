using UnityEngine;

public class ProductInstance : MonoBehaviour
{
    [SerializeField] private InteractableData datosInteractuables; //accede al SO que tiene asignado el objeto fisico
    private ProductData producto; //guarda el SO como un ProductData
    [SerializeField] private int cantidadRestante;
    private CuttingBoard tablaActual = null;

    private void Awake()
    {
        if (datosInteractuables == null)
        {
            return;
        }

        if (datosInteractuables.ObtenerDatos()is ProductData datosProducto) //comprueba que el SO sea un ProductData y si lo es crea una variable que se llama datosProducto
        {
            producto = datosProducto; // y luego se guarda la referencia para usarla despues en el script :3
            cantidadRestante = producto.ObtenerCantidadProducida();

            Debug.Log (gameObject.name + "porciones restantes: "+ cantidadRestante);
        }
    }

    public  bool RetirarPorciones(int cantidad)
    {
        if (cantidad <= 0||cantidadRestante<cantidad)
        {
            return false;
        }

       cantidadRestante -= cantidad;
        return true;
    }

    public int ObtenerCantidadRestante()
    {
        return cantidadRestante;
    }

    public ProductData ObtenerProducto()
    {
        return producto;
    }

    public CuttingBoard ObtenerTabla()
    {
        return tablaActual;
    }

    public void AsignarLaTabla(CuttingBoard tabla)
    {
        tablaActual = tabla;    
    }

    public void QuitarDeLaTabla()
    {
        if (tablaActual != null)
        {
            tablaActual.RetirarProducto();
            tablaActual = null;
        }
    }


    [ContextMenu("RetirarPorcion")]
    private void RetirarPorcion()
    {
        RetirarPorciones (1);
    }

}
