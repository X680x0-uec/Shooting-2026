using UnityEngine;

public class UIManager : MonoBehaviour {
    //ポーズ中の表示だけのために作られたプログラム
    [SerializeField] private GameObject PosePanel;

    public void ShowPosePanel() {
        if (PosePanel.activeSelf) {
            PosePanel.SetActive(false);
        } else {
            PosePanel.SetActive(true);
        }
    }
}
