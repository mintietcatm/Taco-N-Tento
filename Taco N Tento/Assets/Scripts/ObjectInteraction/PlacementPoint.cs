using UnityEngine;

public class PlacementPoint : MonoBehaviour
{
    private GameObject currentObject = null;

    public bool IsOccupied()
    {
        if (currentObject != null)
        {
            return true;
        }

        else
        {
            return false;
        }
        
    }

    public void Occupy(GameObject occupiedBy)
    {
        currentObject = occupiedBy; //guarda que esta ocupando el slot
    }

    public void Clear()
    {
        currentObject=null; //borra Occupy basicamente
    }
}
