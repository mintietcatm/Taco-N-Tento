using UnityEngine;

[CreateAssetMenu(fileName = "Ingrediente", menuName = "Nuevo Ingrediente /Crear PRODUCTO (Ingrediente Sin Preparar) Scriptable Object")]
public class ProductData : ScriptableObject
{
    [SerializeField] private string Nombre;
    [SerializeField] private float Precio;
    [SerializeField] private string Descripcion;
    [SerializeField] private Sprite Icono;
    [SerializeField] TipoDeProducto productoActual;
    [SerializeField] private GameObject Modelo;
    [SerializeField] private IngredientData IngredienteResultado; // IMPORTANTEEEE Debe referenciarse IngredientData especificamente, no ScriptableObject porque es demasiado general. Asi Unity ya sabe donde buscar. 
    [SerializeField] private int CantidadProducida;
    [SerializeField] private bool SePuedeCortar;

    public string ObtenerNombre()
    {
        return Nombre;
    }

    public IngredientData ObtenerIngredienteResultado()
    {
        return IngredienteResultado;
    }

    public int ObtenerCantidadProducida()
    {
        return CantidadProducida;
    }

    public bool ObtenerSePuedeCortar()
    {
        return SePuedeCortar;
    }
}
