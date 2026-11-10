using System.Collections.Generic;
using UnityEngine;

public class TacoAssembly : MonoBehaviour
{
    [SerializeField] private List <IngredientData> ingredientes = new List<IngredientData> ();
    private List<GameObject> porcionesAgregadas = new List<GameObject>();
    //[SerializeField] private IngredientData ingredientePrueba;

    public void AgregarIngrediente(IngredientData ingrediente, GameObject porcion)
    {

        if ( ingrediente == null || porcion== null )
        {
            return;
        }

        if (porcionesAgregadas.Contains(porcion) )
        {
            return;
        }
        ingredientes.Add(ingrediente);
        porcionesAgregadas.Add(porcion);
    }

    public void QuitarIngrediente(GameObject porcion)
    {
        int Indice = porcionesAgregadas.IndexOf(porcion);

        if ( Indice == -1 )
        {
            return;
        }

        ingredientes.RemoveAt(Indice);
        porcionesAgregadas.RemoveAt(Indice);
    }


}
