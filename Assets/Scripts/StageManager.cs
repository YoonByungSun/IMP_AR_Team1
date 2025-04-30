using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Text stage1Text;
    public Text stage2Text;
    public Text stage3Text;

    public int currentStage = 1;

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
            Debug.Log("Game Clear !");
            ShowGameClearText();
        }
    }

    private void InitStageTexts()
    {
        stage1Text.text = "> AM 12:00";
        stage2Text.text = "> AM 3:00";
        stage3Text.text = "> AM 6:00";
    }

    private void UpdateStageUI()
    {
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
            text.fontSize = 40;
        }
    }

    private void ShowGameClearText()
    {
        if (gameClearText != null)
        {
            gameClearText.gameObject.SetActive(true);
        }
    }
}

