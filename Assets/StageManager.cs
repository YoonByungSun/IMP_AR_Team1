using UnityEngine;
using UnityEngine.UI;  // Text 사용 시 반드시 필요!

public class StageManager : MonoBehaviour
{
    public Text stage1Text;
    public Text stage2Text;
    public Text stage3Text;

    private int currentStage = 1;

    private readonly Color activeColor = new Color32(0xFF, 0x83, 0x9E, 0xFF);  // #FF839E
    private readonly Color inactiveColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF); // #FFFFFF

    void Start()
    {
        InitStageTexts();
        UpdateStageUI();
    }

    public void NextStage()
    {
        if (currentStage < 3)
        {
            currentStage++;
            UpdateStageUI();
        }
        else
        {
            Debug.Log("모든 스테이지를 클리어했습니다!");
        }
    }

    private void InitStageTexts()
    {
        stage1Text.text = "AM 12:00";
        stage2Text.text = "AM 3:00";
        stage3Text.text = "AM 6:00";
    }

    private void UpdateStageUI()
    {
        // 전체 텍스트 스타일 업데이트
        SetStageStyle(stage1Text, 1);
        SetStageStyle(stage2Text, 2);
        SetStageStyle(stage3Text, 3);
    }

    private void SetStageStyle(Text text, int stageNumber)
    {
        if (stageNumber == currentStage)
        {
            text.color = activeColor;
            text.fontSize = 55;
        }
        else
        {
            text.color = inactiveColor;
            text.fontSize = 36;
        }
    }
}

