using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public float savedScale = 0.01f;
    void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetScaleForStage(string stageName)
    {
        switch(stageName)
        {
            case "Stage1":
            savedScale = 0.01f;
                break;
            case "Stage2":
                savedScale = 0.03f;
                break;
            case "Stage3":
                savedScale = 0.06f;
                break;
            
            default:
                savedScale = 0.01f;
                break;

        }
    }

}
