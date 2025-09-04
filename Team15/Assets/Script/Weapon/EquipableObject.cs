using UnityEngine;

public class EquipableObject : MonoBehaviour
{
    [SerializeField] public GameObject objectUI;
    public bool isEquipable = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered");
        if (other.CompareTag("Player"))
        {
            objectUI.SetActive(true);
            isEquipable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exit");
        if (collision.CompareTag("Player"))
        {
            objectUI.SetActive(false);
            isEquipable = false;
        }
    }
}
