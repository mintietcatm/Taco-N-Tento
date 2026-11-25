using UnityEngine;

public class IngredientContainer : MonoBehaviour
{
    [SerializeField] private IngredientData ingredienteActual; //guarda que ingrediente esta almacenado en el recipiente
    [SerializeField] private int capacidadMaxima = 20;
    [SerializeField] private int cantidadActual = 0;

    [SerializeField] private IngredientInventory inventario;
    [SerializeField] private GameObject prefabPorcion;

    public int ObtenerCantidadActual()
    {
        return cantidadActual;
    }

   

    public int ObtenerCapacidadMaxima()
    {
        return capacidadMaxima;
    }

    public bool AgregarPorcion(IngredientData ingrediente)
    {
        if (ingrediente != ingredienteActual)
        {
            return false;
        }

        if (cantidadActual >= capacidadMaxima)
        {
            return false;
        }

        cantidadActual++;
        return true;
     
    }

    public IngredientData ObtenerIngredienteActual()
    {
        return ingredienteActual;
    }

    public bool RetirarPorcion()
    {
        if (cantidadActual <= 0)
        {
            return false;
        }

        cantidadActual--;
        return true;

    }

    public bool RellenarRecipiente()
    {
        if (inventario == null || ingredienteActual == null) //si no existen las referencias
        {
            return false;
        }

        int espacioDisponible = capacidadMaxima - cantidadActual; //self explanatory xd

        int cantidadInventario = inventario.ObtenerCantidad(ingredienteActual); //revisamos cuantos ingredientes hay en el inventario

        int cantidadATransferir = Mathf.Min(espacioDisponible, cantidadInventario); //mathf.min es para elegir la cantidad menor entre las dos

        if (cantidadATransferir <= 0) //si es menor o igual a 0 pues no se transfiere nada
        {
            return false;
        }

        if (inventario.RetirarIngredientes(ingredienteActual, cantidadATransferir))
        {
            cantidadActual += cantidadATransferir; // += es lo mismo q poner dq. + se lo agrega al valor
            return true;
        }
        return false;
    }

    public GameObject ExtraerPorcion() //metodo tipo GameObject devuelve un GameObject
    {
        if (prefabPorcion == null)
        {
            return null;
        }

        if (!RetirarPorcion())
        {
            return null;
        }

        GameObject porcion = Instantiate(prefabPorcion);
        return porcion;
    }


    //PRUEBA. BORRAR DESPUES
    [ContextMenu("Probar agregar porcion")]
    private void ProbarAgregarPorcion()
    {
        bool resultado = AgregarPorcion(ingredienteActual);
        Debug.Log($"bool: {resultado}. Cantidad: {cantidadActual}");
    }
    [ContextMenu("Probar quitar porcion")]
    private void ProbarQuitarPorcion()
    {
        bool resultado = RetirarPorcion();
        Debug.Log($"bool: {resultado}. Cantidad: {cantidadActual}");
    }
}
