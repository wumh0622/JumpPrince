using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup mMaskObj = null;
    [SerializeField] private CanvasGroup mWindowsMenu = null;
    [SerializeField] private Button m_WindowsBtn = null;
    [SerializeField] private Button m_CloseBtn = null;
    [SerializeField] private Button m_GameBtn = null;
    [SerializeField] private Button m_CloseWinMenuBtn = null;

    [SerializeField] private float m_WindowsFadeTime = 0;

    private bool m_IsOpen = false;
    private string m_RecordName = "FirstGame";

    private void Awake()
    {
        LoadGame();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            PlayerPrefs.SetInt(m_RecordName, 0);
        }
    }

    private void LoadGame()
    {
        if (PlayerPrefs.GetInt(m_RecordName, 0) == 0)
        {
            PlayerPrefs.SetInt(m_RecordName, 1);
            StartGame();
            Debug.Log("턨놓뉴1");
        }
        else
        {
            mMaskObj.DOFade(0, 1).OnComplete(() => { mMaskObj.gameObject.SetActive(false); });
            Debug.Log("턨놓뉴0");
            Init();
        }
    }

    private void Init()
    {
        m_IsOpen = false;
        mWindowsMenu.alpha = 0;
        m_CloseWinMenuBtn.gameObject.SetActive(false);
        SetBtn();
    }

    private void SetBtn()
    {
        m_WindowsBtn.onClick.AddListener(SetWindowsMenu);
        m_CloseBtn.onClick.AddListener(CloseGame);
        m_GameBtn.onClick.AddListener(StartGame);
        m_CloseWinMenuBtn.onClick.AddListener(SetWindowsMenu);
    }
    private void SetWindowsMenu()
    {
        mWindowsMenu.DOFade(m_IsOpen ? 0 : 1, m_WindowsFadeTime);
        mWindowsMenu.interactable = !m_IsOpen;
        mWindowsMenu.blocksRaycasts = !m_IsOpen;
        m_CloseWinMenuBtn.gameObject.SetActive(!m_IsOpen);
        m_IsOpen = !m_IsOpen;
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void CloseGame()
    {
        Application.Quit();
    }
}