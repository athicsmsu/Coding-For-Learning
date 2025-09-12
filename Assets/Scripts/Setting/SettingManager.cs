using UnityEngine;

public static class SettingManager
{
    private const string MUSIC_KEY = "setting_music";
    private const string SOUND_KEY = "setting_sound";
    private const string CODE_OPTION_JAVA_KEY = "setting_code_option_java";
    private const string CODE_OPTION_PYTHON_KEY = "setting_code_option_python";

    // Music Volume
    public static void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(MUSIC_KEY, Mathf.Clamp01(value));
        PlayerPrefs.Save();
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_KEY, 1.0f); // ค่าเริ่มต้น 1.0
    }

    // Sound Volume
    public static void SetSoundVolume(float value)
    {
        PlayerPrefs.SetFloat(SOUND_KEY, Mathf.Clamp01(value));
        PlayerPrefs.Save();
    }

    public static float GetSoundVolume()
    {
        return PlayerPrefs.GetFloat(SOUND_KEY, 1.0f);
    }

    // Code Option Compile for Java (1 หรือ 2)
    public static void SetCodeOptionCompileJava(int option)
    {
        PlayerPrefs.SetInt(CODE_OPTION_JAVA_KEY, Mathf.Clamp(option, 1, 2));
        PlayerPrefs.Save();
    }

    public static int GetCodeOptionCompileJava()
    {
        return PlayerPrefs.GetInt(CODE_OPTION_JAVA_KEY, 1);
    }

    // Code Option Compile for Python (1 หรือ 2)
    public static void SetCodeOptionCompilePython(int option)
    {
        PlayerPrefs.SetInt(CODE_OPTION_PYTHON_KEY, Mathf.Clamp(option, 1, 2));
        PlayerPrefs.Save();
    }

    public static int GetCodeOptionCompilePython()
    {
        return PlayerPrefs.GetInt(CODE_OPTION_PYTHON_KEY, 1);
    }
}
