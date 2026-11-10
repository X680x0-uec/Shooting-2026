using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandlerST : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    private RectTransform rectTransform;
    private Vector2 dragDirection;
    private Canvas parentCanvas;

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
            rectTransform.localPosition = localPoint + dragDirection;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        // ドラッグ終了時の処理（ドロップ判定やSE再生など）が必要なら記述
    }
}
