using UnityEngine;

public class MobileJoystick : MonoBehaviour
{
    [SerializeField] RectTransform outer;
    [SerializeField] RectTransform thumbstick;
    [SerializeField] float sensitiviy;

    Vector3 clickedPostion;
    Vector3 move;
    bool isActive = false;

    void Start()
    {
        HideJoystick();
    }

    void Update()
    {
        if (isActive)
            ControlThumbstick();
    }

    public void OnControlZoneClicked()
    {
        clickedPostion = Input.mousePosition;
        outer.position = clickedPostion;
        thumbstick.position = clickedPostion;
        ShowJoystick();
    }

    void ShowJoystick()
    {
        outer.gameObject.SetActive(true);
        isActive = true;
    }

    void HideJoystick()
    {
        outer.gameObject.SetActive(false);
        isActive = false;
    }

    void ControlThumbstick()
    {
        move = new Vector3(0f, 0f, 0f);

        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - clickedPostion;

        float responsiveDistance = direction.magnitude * sensitiviy / Screen.width;
        float radius = outer.rect.width / 2;
        float magnitude = Mathf.Min(responsiveDistance, radius);

        move = direction.normalized * magnitude;

        thumbstick.position = clickedPostion + move;

        if (Input.GetMouseButtonUp(0))
            HideJoystick();
    }

    public Vector3 GetMove()
    {
        return move;
    }
}
