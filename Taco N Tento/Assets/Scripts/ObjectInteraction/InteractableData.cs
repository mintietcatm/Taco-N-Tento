using UnityEngine;

public class InteractableData : MonoBehaviour
{
    [SerializeField] private ScriptableObject scriptableObject;
    [SerializeField] private string nombreAlternativo; //para los objetos que no son SO. Eventualmente va a funcionar con el refri, la parrilla, etc etc
    

    public string ObtenerNombre()
    {
        if (scriptableObject is IngredientData ingrediente) //si es ingrediente
        {
            return ingrediente.ObtenerNombre();
        }

        if (scriptableObject is ProductData producto) //si es producto
        {
            return producto.ObtenerNombre();
        }

        return nombreAlternativo; //si no es ni uno de los dos (osea un refri o algo asi)
    }

    public ScriptableObject ObtenerDatos()
    {
        return scriptableObject;

    }

 
}
