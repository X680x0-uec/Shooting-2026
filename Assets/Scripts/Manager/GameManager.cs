using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Stage
    {
        stage1,
        stage2,
        stage3
    }
    // ボスを倒したら(アニメーションをするならその後でも)このcurrentStageとstageNumberを変える
    [HideInInspector] public Stage currentStage = Stage.stage1;
    [HideInInspector] public int stageNumber;

    void Awake()
    {
        stageNumber = (int)currentStage;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
