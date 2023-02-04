using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
// [DefaultExecutionOrder(1000)]
public class GameManager : MonoBehaviour
{
    // Expose best score and name input field
    [SerializeField]  public TextMeshProUGUI bestScoreText;
    [SerializeField]  public TMP_InputField nameField;

    public string playerName = "";

    public string bestName;
    public int bestScore;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGameInfo();
    }

    private void Start()
    {
        if (bestName != "")
        {
            bestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
    }

    public void SetBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            bestName = playerName;
            SaveGameInfo();
            MainManager.Instance.BestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
        Debug.Log("Score: " + score + "  Player: " + playerName);
    }

    public void StartNew()
    {
        if (nameField.text != "")
        {
            playerName = nameField.text;
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogWarning("Please enter a name!");
            nameField.placeholder.SendMessage("Please enter a Name!");
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public int bestScore;
    }

    public void SaveGameInfo()
    {
        SaveData data = new SaveData();
        data.name = bestName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGameInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestName = data.name;
            bestScore = data.bestScore;
        }
    }
}