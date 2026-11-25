using UnityEngine;

public class DispensadorDePorciones : MonoBehaviour
{
    [SerializeField] private ProductInstance producto;
    [SerializeField] private GameObject prefabPorcion;

    public GameObject ExtraerPorcion() //ok como es un metodo de tipo GameObject este tiene que devolver un GameObject
    {
        if (producto == null || prefabPorcion == null) //revisa si existen las referencias
        {
            Debug.Log("Prefab no asignado.");
            return null;
        }

        if (!producto.RetirarPorciones(1)) //si no quedan porciones ps no retiramos
        {
            Debug.Log("No quedan porciones");
            return null;
        }

        //Generamos la porcion

        GameObject porcion = Instantiate(prefabPorcion);

        //borrar esto cuando jale el UI del inventario
        Debug.Log("Quedan " + producto.ObtenerCantidadRestante() + " porciones");

        return porcion;


    }
}
