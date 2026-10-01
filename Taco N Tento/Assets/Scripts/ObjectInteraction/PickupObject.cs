using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObject : MonoBehaviour
{
    [SerializeField] private Transform handPoint; //punto de donde la recojo
    [SerializeField] private Camera playerCamera; // la camara del jugador para lanzar el Raycast
    [SerializeField] private float pickupDistance = 3f; // que tan lejos puede llegar el jugador para recoger algo
    [SerializeField] private LayerMask placeableLayer;

    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    private Renderer pickedObjectRenderer;
    private Material originalMaterial = null;  //el material original

    private GameObject pickedObject = null; // que tenemos en la mano
    private Rigidbody pickedObjectRb = null;
    private BoxCollider pickedObjectCollider = null; // El collider del objeto que estamos agarrando
    PlacementState currentState = PlacementState.EmptyHanded; //pa que el jugador empieze con las manos vacias




    private void Awake()
    {
        placeableLayer = LayerMask.GetMask("PlaceableSurface");
    }
    void Update()
    {
        switch (currentState)
        {
            case PlacementState.EmptyHanded:
                Debug.Log("EmptyHanded");
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    TryPickUp();
                }

                break;
            case PlacementState.Holding:
                Debug.Log("Holding");
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Drop();
                }
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    pickedObject.transform.SetParent(null);
                    currentState = PlacementState.Placing;
                }
                break;

            case PlacementState.Placing:
                Debug.Log("Placing");
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    pickedObject.transform.SetParent(handPoint);
                    pickedObject.transform.localPosition = Vector3.zero;
                    currentState = PlacementState.Holding;
                }
                RaycastHit hit;

                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupDistance, placeableLayer))
                {
                    pickedObject.transform.position = hit.point;

                    Collider[] overlaps;
                    Vector3 overlapCenter = pickedObject.transform.TransformPoint(pickedObjectCollider.center); //Donde esta el centro en coordenadas del mundo (por eso Vector3) 
                    Vector3 halfExtents = Vector3.Scale(pickedObjectCollider.size, pickedObject.transform.lossyScale) / 2; //lossyScale considera la escala aproximada final en el mundo, no de los padres

                    bool canPlace = true;

                    overlaps = Physics.OverlapBox(overlapCenter, halfExtents, pickedObject.transform.rotation);

                    foreach (Collider collider in overlaps) // FOR EACH por cada COLLIDER elemento de tipo Collider durante esta vuelta COLLIDER le voy a llamar collider IN  dentro del array
                    {
                        if (collider==pickedObjectCollider)
                        {
                            continue; //continue literalmente le dice al codigo que siga avanzando
                        }
                        
                        if (collider == hit.collider)
                        {
                            continue;
                        }

                        canPlace = false;
                        Debug.Log(collider.gameObject.name);
                        Debug.Log("Colliders encontrados " + overlaps.Length); //Length singifica cuantos elementos hay en el Array (los [] en Collider)

                    }
                    if (canPlace==true)
                    {
                        pickedObjectRenderer.material = validMaterial;
                    }
                    else
                    {
                        pickedObjectRenderer.material = invalidMaterial;
                    }

                    if (Mouse.current.leftButton.wasPressedThisFrame&&canPlace==true)
                    {
                        currentState=PlacementState.EmptyHanded;

                        pickedObjectRenderer.material=originalMaterial;
                        pickedObject = null;
                        pickedObjectRb=null;
                        pickedObjectCollider=null;
                        pickedObjectRenderer=null;
                    }
                    Debug.Log("colocar " + canPlace);
                    //Debug.Log(hit.point);

                }

                break;

            default:
                return;

        }
    }

    private void TryPickUp()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupDistance))
        {
            if (hit.collider.CompareTag("Interactuable"))
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
        pickedObjectCollider = pickedObject.GetComponent<BoxCollider>();
        pickedObjectRenderer = pickedObject.GetComponent<Renderer>();
        originalMaterial = pickedObjectRenderer.material; //Aqui se guarda el material original del objeto, para no tener que asignarlo manualmente en el inspector

        if (pickedObjectRb != null)
        {
            pickedObjectRb.linearVelocity = Vector3.zero; //representa el cambio de la posicion, la velocidad en xyz ahora es 0
            pickedObjectRb.angularVelocity = Vector3.zero;
            pickedObjectRb.isKinematic = true; //Controla si la fisica afecta al rb del objeto
        }

        pickedObject.transform.SetParent(handPoint);
        pickedObject.transform.localRotation = Quaternion.identity;
        currentState = PlacementState.Holding;

        pickedObject.transform.localPosition = Vector3.zero;
        //pickedObject.transform.localRotation = Quaternion.identity;
    }

    private void Drop()
    {
        pickedObject.transform.SetParent(null); //deja de ser hijo de la mano. sueltas la tortilla

        if (pickedObjectRb != null) //comprueba si tiene un rigidbody
        {
            pickedObjectRb.isKinematic = false; //reactivamos las fisicas del objeto para que se caiga
        }
        pickedObject = null;
        pickedObjectRb = null; //Para que el codigo no se quede con la informacion
        pickedObjectCollider = null;
        currentState = PlacementState.EmptyHanded;
        Debug.Log("Estado actual: " + currentState);
    }

}


