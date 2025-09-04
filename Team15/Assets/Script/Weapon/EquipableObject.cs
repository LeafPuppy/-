using UnityEngine;

public class EquipableObject : MonoBehaviour
{
    [SerializeField] public GameObject objectUI;
    public bool isEquipable = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            objectUI.SetActive(true);
            isEquipable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            objectUI.SetActive(false);
            isEquipable = false;
        }
    }
}
