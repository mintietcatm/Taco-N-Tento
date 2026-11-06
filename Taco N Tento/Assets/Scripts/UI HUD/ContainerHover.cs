using TMPro;
using UnityEngine;

public class ContainerHover : MonoBehaviour
{

    #region Elementos en el canvas
    [SerializeField] TextMeshProUGUI ContainerDescription;
    [SerializeField] TextMeshProUGUI ContainerContent;
    [SerializeField] private GameObject PanelDetalles; //El panel que tiene toda la información

    #endregion

    #region Referencias
    [SerializeField] private RaycastDetector raycastDetector; //Referenciamos al raycast detector para poder usarlo
    [SerializeField] private PickupObject pickupObject;

    #endregion

    void Start()
    {
        //Agregamos alerta por si algo no esta referenciado
        ContainerDescription.text = "Missing ref!";
        ContainerContent.text = "Missing ref!";

        if (PanelDetalles == null)
        {
            Debug.Log("Container Details not referenced.");
        }
    }


    void Update()
    {
        if (pickupObject.IsPlacing()) // este metodo revisa si estamos en Placing
        {
            PanelDetalles.SetActive(false);
            return;
        }

        if (raycastDetector.Hit()) //el raycast golpeo algo
        {
            RaycastHit hit = raycastDetector.GetHit(); // que golpeo

            if (hit.collider.CompareTag("Contenedor")) // tiene el tag de contenedor
            {
                IngredientContainer container = hit.collider.GetComponent<IngredientContainer>();

                PanelDetalles.SetActive(true);
                ContainerDescription.text = hit.collider.gameObject.name;

                if (container != null)
                {
                    Debug.Log("Contenedor encontrado");
                }

            }
            else //si no es contenedor se borra el texto
            {
                PanelDetalles.SetActive(false);
            }
        }

        else // si no golpea nada no ponemos nada en el ui
        {
            PanelDetalles.SetActive(false);
        }


    }
}
