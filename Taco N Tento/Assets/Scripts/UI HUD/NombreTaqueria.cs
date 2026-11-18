using TMPro;
using UnityEngine;

public class NombreTaqueria : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TaqueriaName;

    public void Start()
    {
        TaqueriaName.text = "test";

    }
}
