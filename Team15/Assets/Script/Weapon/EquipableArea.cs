using UnityEngine;

// todo. UI가 회전하는 버그 해결하기
public class EquipableArea : MonoBehaviour
{
    [SerializeField] public GameObject objectUI;
    public bool isEquipable = false;
    void LateUpdate()
    {
        // 현재 회전값을 가져옴
        Vector3 rot = transform.rotation.eulerAngles;
        // Y축 회전을 0으로 고정
        transform.rotation = Quaternion.Euler(rot.x, 0f, rot.z);
    }

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
