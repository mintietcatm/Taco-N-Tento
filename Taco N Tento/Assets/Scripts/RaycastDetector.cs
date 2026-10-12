using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; //La camara de donde se lanza el raycast.
    [SerializeField] private float detectionDistance = 3f; //La distancia maxima que puede alcanzar el raycast. para que no se pueda interactuar con las cosas de super lejos.

    private RaycastHit hit; // Y hit ps guarda donde golpeo el rayo.
    private bool detected; // para guardar la informacion de que golpeo el rayo.


    private void Update()
    {
        DetectObject();
    }
    private void DetectObject()
    {
        detected = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, detectionDistance);
        
    }

    //DetectSurface devuelve TRUE si se encuentra una superficie. LayerMask detecta la capa. 
    //out RaycastHit surfaceHit obtiene informacion de la superficie
    // return Physics.Raycast ejecuta el raycast usando la capa que recibio y devuelve su resultado (return)
    public bool DetectSurface(LayerMask layer, out RaycastHit surfaceHit) // ejecuta otro raycast porque este esta buscando la layer donde podemos poner las cosas
    {
        return Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out surfaceHit, detectionDistance, layer);

    }

    public bool Hit() //bool si tocamos algo
    {
        return detected;
    }

    public RaycastHit GetHit() //devuelve el resultado del raycast sin castear otro
    {
        return hit;
    }
}
