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



}
