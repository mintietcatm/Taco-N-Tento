using UnityEngine;

[CreateAssetMenu(fileName = "Ingrediente", menuName = "Nuevo Ingrediente /Crear PORCION (Ingrediente Preparado) Scriptable Object")]
public class IngredientData : ScriptableObject
{
    [SerializeField] private string Nombre;
    [SerializeField] private float Precio;
    [SerializeField] private string Descripcion;
    [SerializeField] private Sprite Icono;
    [SerializeField] TipoDeIngrediente ingredienteActual;
    [SerializeField] private GameObject Modelo;
    [SerializeField] private bool RequiereCoccion;
    [SerializeField] private float TiempoCoccion;
    [SerializeField] private float TiempoQuemado;

    public string ObtenerNombre()
    {
        return Nombre;
    }

    public bool ObtenerRequiereCoccion()
    {
        return RequiereCoccion;
    }

    public float ObtenerTiempoCoccion()
    {
        return TiempoCoccion;
    }

    public float ObtenerTiempoQuemado()
    {
        return TiempoQuemado;
    }

    public Sprite ObtenerIcono()
    {
        return Icono;
    }
}
