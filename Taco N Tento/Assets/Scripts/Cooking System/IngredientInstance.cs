using UnityEngine;

public class IngredientInstance : MonoBehaviour
{
    [SerializeField] private InteractableData datosInteractuables;

    private IngredientData ingrediente;

    [SerializeField] private EstadoIngrediente estadoActual = EstadoIngrediente.SinPreparar;
    [SerializeField] private float tiempoAcumulado = 0f;

    //Referencias de materiales/TX Crudo/Cocinado/Quemado
    [Header("Materiales")]
    [SerializeField] private Renderer ingredienteRenderer;
    [SerializeField] private Material materialCrudo;
    [SerializeField] private Material materialCocinado;
    [SerializeField] private Material materialQuemado;

    [SerializeField] private bool isCooking = false;

    private void Awake()
    {
        if (datosInteractuables == null) //si no hay datos interactuables
        {
            return;
        }

        if (datosInteractuables.ObtenerDatos() is IngredientData datosIngrediente)
        {
            ingrediente = datosIngrediente;
        }
    }

    private void Update()
    {
        if (!isCooking)
        {
            return;
        }
            tiempoAcumulado += Time.deltaTime;

        if (tiempoAcumulado >= ingrediente.ObtenerTiempoQuemado()) //se quemo :c
        {
            estadoActual = EstadoIngrediente.Quemado;
            ingredienteRenderer.material = materialQuemado;

            isCooking = false;

            Debug.Log(ingrediente.ObtenerNombre() + " se quemo :c");
        }

        else if (tiempoAcumulado >= ingrediente.ObtenerTiempoCoccion() && estadoActual != EstadoIngrediente.Listo)
        {
            estadoActual = EstadoIngrediente.Listo;
            ingredienteRenderer.material = materialCocinado;

            Debug.Log(ingrediente.ObtenerNombre() + " se cocino");
        }
    }

    public IngredientData ObtenerIngrediente()
    {
        return ingrediente;
    }

    public EstadoIngrediente ObtenerEstado()
    {
        return estadoActual;
    }

    #region Sistema de coccion

    public bool IniciarCoccion()
    {
        if (ingrediente == null) //si el ingrediente NO es valido
        {
            return false;
        }

        if (!ingrediente.ObtenerRequiereCoccion()) //si el ingrediente NO ocupa cocinarse
        {
            return false;
        }

        if (estadoActual == EstadoIngrediente.Quemado) //no puedes cocinar ingredientes quemados
        {
            return false;
        }

        isCooking = true;

        if (estadoActual != EstadoIngrediente.SinPreparar) //si el ingrediente ya esta cocinado o quemado
        {
            
        estadoActual = EstadoIngrediente.EnPreparacion; //cambiamos el estado a en preparacion
        }

        return true;
    }


    public void DetenerCoccion()
    {
        isCooking = false;

        if (estadoActual == EstadoIngrediente.EnPreparacion)
        {
            estadoActual = EstadoIngrediente.SinPreparar;
        }
    }

    #endregion
}
