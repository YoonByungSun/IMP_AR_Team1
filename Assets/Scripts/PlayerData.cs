using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public float savedScale = 1f;
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
            case "Stage2":
                savedScale = 0.06f;
                break;
            case "Stage3":
                savedScale = 0.2f;
                break;
            
            default:
                savedScale = 1f;
                break;

        }
    }

}
