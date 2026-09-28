using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObject : MonoBehaviour
{
    [SerializeField] private Transform handPoint; //punto de donde la recojo
    [SerializeField] private Camera playerCamera; // la camara del jugador para lanzar el Raycast
    [SerializeField] private float pickupDistance = 3f; // que tan lejos puede llegar el jugador para recoger algo
    [SerializeField] private LayerMask placeableLayer;

    private GameObject pickedObject = null; // que tenemos en la mano
    private Rigidbody pickedObjectRb = null;

    
    private void Awake()
    {
        placeableLayer = LayerMask.GetMask("PlaceableSurface");
    }
    void Update()
    {
        if (pickedObject == null)
        {
            //puedes recoger cosas. tienes las manos vacias
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                TryPickUp();
            }

        }

        else
        {
            //ya tienes algo en la mano
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Drop();
            }
        }
    }

    private void TryPickUp()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupDistance))
        {
            if (hit.collider.CompareTag("Ingrediente"))
            {
                PickUp(hit.collider.gameObject);
            }
            Debug.Log(hit.collider.gameObject.name);

        }
    }

    private void PickUp(GameObject objectToPickUp)
    {
        pickedObject = objectToPickUp;
        pickedObjectRb = pickedObject.GetComponent<Rigidbody>();

        if (pickedObjectRb != null)
        {
            pickedObjectRb.isKinematic = true; //Controla si la fisica afecta al rb del objeto
            pickedObjectRb.linearVelocity = Vector3.zero; //representa el cambio de la posicion, aqui ando convirtiendolo al vector3 osea x,y,z
            pickedObjectRb.angularVelocity = Vector3.zero;
        }

        pickedObject.transform.SetParent(handPoint);

        pickedObject.transform.localPosition = Vector3.zero;
        //pickedObject.transform.localRotation = Quaternion.identity;
    }

    private void Drop()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupDistance, placeableLayer))
        {
            if (hit.collider.CompareTag("PlacementSurface"))
            {
                Transform surface = hit.collider.transform;
                PlacementPoint[] puntos = surface.GetComponentsInChildren<PlacementPoint>();
                Debug.Log("Raycast toco la mesa");

                foreach (PlacementPoint slot in puntos)
                {
                    Debug.Log(slot.gameObject.name);
                    //if (slot.IsOccupied(!true))
                    //{
                    //    pickedObject.transform.SetParent(slot);
                    //    pickedObject.transform.localPosition = Vector3.zero;
                    //    pickedObjectRb.isKinematic = false;
                    //    pickedObject = null; //Registra que ya no traemos nada en la mano

                    //}

                }

                return;

            }

        }



        if (pickedObjectRb != null)
        {
            pickedObject.transform.SetParent(null); //deja de ser hijo de la mano
            pickedObjectRb.isKinematic = false; //reactivamos las fisicas del objeto para que se caiga

        }
        pickedObject = null;

    }

}


