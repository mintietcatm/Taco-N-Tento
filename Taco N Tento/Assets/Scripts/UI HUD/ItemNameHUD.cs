using TMPro;
using UnityEngine;

public class ItemNameHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemNameTxt;
    [SerializeField] private TextMeshProUGUI InteractionHint;
    [SerializeField] private RaycastDetector raycastDetector;
    [SerializeField] private PickupObject pickupObject; //de PickupObject sacamos el placement state. Aqui referencio el componente para poder mandar a llamar el metodo

    public void Start()
    {
        ItemNameTxt.text = "ItemNameHud not working. check references in game manager";

    }

    private void Update()
    {
        if (pickupObject.IsPlacing()) // este metodo revisa si estamos en Placing y quita el texto del UI para que no estorbe
        {
            ItemNameTxt.text = "";
            InteractionHint.text = "";
            return;
        }

        if (raycastDetector.Hit()) //el raycast golpeo algo
        {
            RaycastHit hit = raycastDetector.GetHit(); // que golpeo

            if (hit.collider.CompareTag("Interactuable")) // tiene el tag de interactuable
            {
                InteractableData datosInteractuables = hit.collider.gameObject.GetComponent<InteractableData>();

                if (datosInteractuables != null)
                {
                    ItemNameTxt.text = datosInteractuables.ObtenerNombre();
                    InteractionHint.text = "[Click Izq] Interactuar";
                }
                else
                {
                    ItemNameTxt.text = "";
                    InteractionHint.text = "";
                }
            }
            else //si no tiene interactuable se borra el texto
            {
                ItemNameTxt.text = "";
                InteractionHint.text = "";
            }
        }

        else // si no golpea nada no ponemos nada en el ui
        {
            ItemNameTxt.text = "";
            InteractionHint.text = "";
        }

    }

    public void DisplayItemName(string itemName)
    {

    }

}
