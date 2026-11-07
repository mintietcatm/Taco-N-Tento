using UnityEngine;

public class IngredientContainer : MonoBehaviour
{
    [SerializeField] private IngredientData ingredienteActual; //guarda que ingrediente esta almacenado en el recipiente
    [SerializeField] private int capacidadMaxima = 20;
    [SerializeField] private int cantidadActual = 0;

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

    public bool RetirarPorcion()
    {
        if (cantidadActual <= 0)
        {
            return false;
        }

        cantidadActual--;
        return true;

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
