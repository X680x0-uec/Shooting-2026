using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragHandlerST : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    private RectTransform rectTransform;
    private Vector2 dragDirection;
    private Canvas parentCanvas;
    [SerializeField] private float skilltreeFieldW = 600f;
    [SerializeField] private float skilltreeFieldH = 1080f;
    [SerializeField] private float magnificationLimit = 3.0f;
    [SerializeField] private float shrinkageLlimit = 0.25f;
    [SerializeField] private float scrollSensitivity = 0.01f;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        // 親要素を遡ってCanvasを取得（CanvasScaler対策）
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        // ドラッグ開始時に最前面へ移動させたい場
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint)) {
            dragDirection = rectTransform.localPosition - new Vector3(localPoint.x, localPoint.y, 0);
        }
        rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData) {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint)) {
            Vector3 nextPosition = localPoint + dragDirection;

            if (nextPosition.x > 0.5f * skilltreeFieldW || nextPosition.x < -0.5f * skilltreeFieldW) {
                nextPosition.x = 0.5f * skilltreeFieldW * Mathf.Sign(nextPosition.x);
            }

            if (nextPosition.y > 0.5f * skilltreeFieldH || nextPosition.y < -0.5f * skilltreeFieldH) {
                nextPosition.y = 0.5f * skilltreeFieldH * Mathf.Sign(nextPosition.y);
            }

            rectTransform.localPosition = nextPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        // ドラッグ終了時の処理（ドロップ判定やSE再生など）が必要なら記述
    }

    public void OnWheel(InputAction.CallbackContext context) {
        float scrollDelta = context.ReadValue<Vector2>().y * scrollSensitivity;
        rectTransform.localScale = new(scrollDelta, scrollDelta, 0);
    }
}
