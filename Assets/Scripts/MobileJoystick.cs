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

    // '이벤트 트리거' 컴포넌트의 'Pointer Down' 이벤트에 대한 콜백함수
    public void OnControlZoneClicked()
    {
        clickedPostion = Input.mousePosition;
        outer.position = clickedPostion;
        thumbstick.position = clickedPostion;
        ShowJoystick();
    }

    void ShowJoystick()
    {
        move = new Vector3(0, 0, 0);
        outer.gameObject.SetActive(true);
        isActive = true;
    }

    void HideJoystick()
    {
        move = new Vector3(0, 0, 0);
        outer.gameObject.SetActive(false);
        isActive = false;
    }

    void ControlThumbstick()
    {
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

    public Vector3 GetMoveVector()
    {
        return move;
    }
}
