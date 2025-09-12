using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingUI : MonoBehaviour
{
    public List<Image> musicButtons;
    public List<Image> soundButtons;
    public Sprite greenSprite;
    public Sprite notGreenSprite;

    public Image MusicButton;
    public Image SoundButton;

    int codeOptionJava;
    int codeOptionPython;
    int musicVolume;
    int soundVolume;
    public MusicVolumeController musicVolumeController;
    public SoundVolumeController soundVolumeController;
    public GameObject infoServerJava;
    public GameObject infoServerPython;
    public GameObject infoLocalJava;
    public GameObject infoLocalPython;

    void Start()
    {
        // Load Volume
        musicVolume = Mathf.RoundToInt(SettingManager.GetMusicVolume() * musicButtons.Count);
        soundVolume = Mathf.RoundToInt(SettingManager.GetSoundVolume() * soundButtons.Count);
        UpdateMusicUI();
        UpdateSoundUI();
        // ซ่อนข้อความแจ้งเตือนเริ่มต้น
        infoServerJava.SetActive(false);
        infoServerPython.SetActive(false);
        infoLocalJava.SetActive(false);
        infoLocalPython.SetActive(false);

        if (musicVolume == 0)
            MusicButton.sprite = closeSprite;
        if (soundVolume == 0)
            SoundButton.sprite = closeSprite;

        // ✅ ตรวจสอบ Java และ Python
        bool hasJava = IsExecutableAvailable("javac");
        bool hasPython = IsExecutableAvailable("python");

        // Java ตรวจสอบ
        if (!hasJava)
        {
            // javaCodeButton2.color = Color.gray;
            // javaCodeButton2.raycastTarget = false;
            var btn = javaCodeButton2.GetComponent<Button>();
            if (btn != null)
                btn.interactable = false;

            // บังคับให้ใช้ตัวเลือก 1 และอัปเดต
            SettingManager.SetCodeOptionCompileJava(1);
            codeOptionJava = 1;
            Debug.LogWarning("Java ไม่พร้อมใช้งานในระบบ: บังคับใช้ Option 1");
        }
        else
        {
            codeOptionJava = SettingManager.GetCodeOptionCompileJava();
        }

        // Python ตรวจสอบ
        if (!hasPython)
        {
            // pythonCodeButton2.color = Color.gray;
            // pythonCodeButton2.raycastTarget = false;
            var btn = pythonCodeButton2.GetComponent<Button>();
            if (btn != null)
                btn.interactable = false;

            // บังคับให้ใช้ตัวเลือก 1 และอัปเดต
            SettingManager.SetCodeOptionCompilePython(1);
            codeOptionPython = 1;
            Debug.LogWarning("Python ไม่พร้อมใช้งานในระบบ: บังคับใช้ Option 1");
        }
        else
        {
            codeOptionPython = SettingManager.GetCodeOptionCompilePython();
        }

        // อัปเดต UI ปุ่มเลือก
        UpdateCodeOptionUI();

        Debug.Log("Loaded Java Option: " + codeOptionJava);
        Debug.Log("Loaded Python Option: " + codeOptionPython);
    }



    void UpdateMusicUI()
    {
        for (int i = 0; i < musicButtons.Count; i++)
        {
            musicButtons[i].sprite = (i < musicVolume) ? greenSprite : notGreenSprite;
        }
    }

    void UpdateSoundUI()
    {
        for (int i = 0; i < soundButtons.Count; i++)
        {
            soundButtons[i].sprite = (i < soundVolume) ? greenSprite : notGreenSprite;
        }
    }
    public void OnMusicButtonClick(int index)
    {
        musicVolume = index + 1;
        SettingManager.SetMusicVolume(musicVolume / (float)musicButtons.Count);
        UpdateMusicUI();

        if (MusicButton.sprite == closeSprite)
        {
            MusicButton.sprite = openSprite;
        }

        // เรียก update realtime
        musicVolumeController.UpdateVolume();

        Debug.Log("Music Volume: " + musicVolume);
    }

    public void OnSoundButtonClick(int index)
    {
        soundVolume = index + 1;
        SettingManager.SetSoundVolume(soundVolume / (float)soundButtons.Count);
        UpdateSoundUI();

        if (SoundButton.sprite == closeSprite)
        {
            SoundButton.sprite = openSprite;
        }

        // เรียก update realtime
        soundVolumeController.UpdateVolume();

        Debug.Log("Sound Volume: " + soundVolume);
    }

    public void MuteMusic()
    {
        if (musicVolume == 0)
        {
            musicVolume = 1;
            SettingManager.SetMusicVolume(musicVolume / (float)musicButtons.Count);
            UpdateMusicUI();
            MusicButton.sprite = openSprite;
            Debug.Log("Music Unmuted");
        }
        else
        {
            musicVolume = 0;
            SettingManager.SetMusicVolume(0f);
            UpdateMusicUI();
            MusicButton.sprite = closeSprite;
            Debug.Log("Music Muted");
        }

        // เรียก update realtime
        musicVolumeController.UpdateVolume();
    }

    public void MuteSound()
    {
        if (soundVolume == 0)
        {
            soundVolume = 1;
            SettingManager.SetSoundVolume(soundVolume / (float)soundButtons.Count);
            UpdateSoundUI();
            SoundButton.sprite = openSprite;
            Debug.Log("Sound Unmuted");
        }
        else
        {
            soundVolume = 0;
            SettingManager.SetSoundVolume(0f);
            UpdateSoundUI();
            SoundButton.sprite = closeSprite;
            Debug.Log("Sound Muted");
        }

        // เรียก update realtime
        soundVolumeController.UpdateVolume();
    }




    // สำหรับ Java
    public Image javaCodeButton1;
    public Image javaCodeButton2;

    // สำหรับ Python
    public Image pythonCodeButton1;
    public Image pythonCodeButton2;

    public Sprite openSprite;
    public Sprite closeSprite;

    // === JAVA ===
    public void SetJavaCodeOptionTo1()
    {
        SettingManager.SetCodeOptionCompileJava(1);
        codeOptionJava = 1;
        UpdateCodeOptionUI();
        Debug.Log("Set Java Code Option: 1");
    }

    public void SetJavaCodeOptionTo2()
    {
        SettingManager.SetCodeOptionCompileJava(2);
        codeOptionJava = 2;
        UpdateCodeOptionUI();
        Debug.Log("Set Java Code Option: 2");
    }

    // === PYTHON ===
    public void SetPythonCodeOptionTo1()
    {
        SettingManager.SetCodeOptionCompilePython(1);
        codeOptionPython = 1;
        UpdateCodeOptionUI();
        Debug.Log("Set Python Code Option: 1");
    }

    public void SetPythonCodeOptionTo2()
    {
        SettingManager.SetCodeOptionCompilePython(2);
        codeOptionPython = 2;
        UpdateCodeOptionUI();
        Debug.Log("Set Python Code Option: 2");
    }

    void UpdateCodeOptionUI()
    {
        // Java
        javaCodeButton1.sprite = (codeOptionJava == 1) ? openSprite : closeSprite;
        javaCodeButton2.sprite = (codeOptionJava == 2) ? openSprite : closeSprite;

        // Python
        pythonCodeButton1.sprite = (codeOptionPython == 1) ? openSprite : closeSprite;
        pythonCodeButton2.sprite = (codeOptionPython == 2) ? openSprite : closeSprite;
    }

    private bool IsExecutableAvailable(string executable)
    {
        try
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = executable;
            process.StartInfo.Arguments = " --version";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            return true;
        }
        catch
        {
            return false;
        }
    }


}
