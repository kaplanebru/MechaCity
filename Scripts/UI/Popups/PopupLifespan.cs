using UnityEngine;

public class PopupLifespan : MonoBehaviour
{
    [SerializeField] float lifespan = 1f;
    private void OnEnable()
    {
        Invoke(nameof(DisablePopup), lifespan);
    }

    void DisablePopup()
    {
        gameObject.SetActive(false);
    }
}
