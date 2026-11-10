using UnityEngine;

public class Pickupable : MonoBehaviour
{
    [SerializeField] private TacoAssembly tacoActual;

    public void AsignarTaco(TacoAssembly taco)
    {
        tacoActual = taco;
    }

    public void QuitarDelTaco()
    {
        if (tacoActual == null)
        {
            return;
        }
        tacoActual.QuitarIngrediente(gameObject);
        tacoActual=null;
    }
}
