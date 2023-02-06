using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public GameObject logo;
    private bool firstInput = false;
    public TMP_InputField inputName;
    public TextMeshProUGUI log;
    private string errorMsg = "잘못된 입력입니다";
    private bool validInputExist = false;
    private bool showLog = false;
    public float logDuration = 0.5f;
    private float logTimer = 0f;

    void Start()
    {
        logo.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        inputName.text = string.Empty;
        log.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!firstInput && Input.anyKeyDown)
        {
            if (Input.anyKeyDown)
            {
                logo.GetComponent<LogoMove>()?.Move();
            }
        }

        if (showLog)
        {
            logTimer += Time.deltaTime;
            if (logTimer >= logDuration)
            {
                if (validInputExist)
                {
                    PlayDataManager.instance.playerName = inputName.text;
                    SceneManager.LoadScene("LobbyScene");
                    return;
                }
                ShowLog(false);
                logTimer = 0f;
            }
        }
    }

    public void CheckInput()
    {
        if (validInputExist)
            return;
        validInputExist = !string.IsNullOrWhiteSpace(inputName.text);
        if (validInputExist)
            log.SetText($"환영합니다, {inputName.text}님!");
        else
            log.SetText(errorMsg);
        ShowLog(true);
    }

    private void ShowLog(bool show)
    {
        showLog = show;
        log.gameObject.SetActive(showLog);
    }
}
