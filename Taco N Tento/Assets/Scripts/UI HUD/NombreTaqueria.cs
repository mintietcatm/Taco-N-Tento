using TMPro;
using UnityEngine;

public class NombreTaqueria : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TaqueriaName;
    [SerializeField] private TextMeshProUGUI TaqueriaNameUser;

    public void Start()
    {
        TaqueriaName.text = "test";

    }
}
