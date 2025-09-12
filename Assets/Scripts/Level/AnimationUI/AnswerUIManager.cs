using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class AnswerUIManager : MonoBehaviour
{
    public Text askText;
    public Text askText2;
    public PlayerController player;
    public PlayerController player2;

    private string Language = "";
    private int UIcheckpoint = 0;

    // Java
    private Level2Java level2Java;
    private Level3Java level3Java;
    private Level4Java level4Java;
    private Level5Java level5Java;
    private Level6Java level6Java;
    private Level7Java level7Java;
    private Level8Java level8Java;
    private Level9Java level9Java;
    private Level10Java level10Java;
    private Level11Java level11Java;
    private Level12Java level12Java;
    private Level13Java level13Java;
    private Level14Java level14Java;
    private Level15Java level15Java;
    private Level16Java level16Java;
    private Level17Java level17Java;
    private Level18Java_1 level18Java_1;
    private Level18Java_2 level18Java_2;
    private Level18Java_3 level18Java_3;
    private Level19Java level19Java;
    private Level20Java level20Java;
    private Level21Java level21Java;
    private Level22Java level22Java;
    private Level23Java level23Java;
    private Level24Java level24Java;
    private Level25Java level25Java;
    private Level26Java level26Java;
    private Level27Java level27Java;

    // Python
    private Level2Python level2Python;
    private Level3Python level3Python;
    private Level4Python level4Python;
    private Level5Python level5Python;
    private Level6Python level6Python;
    private Level7Python level7Python;
    private Level8Python level8Python;
    private Level9Python level9Python;
    private Level10Python level10Python;
    private Level11Python level11Python;
    private Level12Python level12Python;
    private Level13Python level13Python;
    private Level14Python level14Python;
    private Level15Python level15Python;
    private Level16Python_1 level16Python_1;
    private Level16Python_2 level16Python_2;
    private Level16Python_3 level16Python_3;
    private Level17Python level17Python;
    private Level18Python level18Python;
    private Level19Python level19Python;
    private Level20Python level20Python;
    private Level21Python level21Python;
    private Level22Python level22Python;
    private Level23Python level23Python;
    private Level24Python level24Python;

    string sceneName;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"AnswerUIManager Start: {sceneName}");
        UIcheckpoint = ChangeScenes.checkpoint;
        Language = ChangeScenes.Language;
        // UIcheckpoint = 14;
        // Language = "Java";
    }

    [System.Obsolete]
    public void ShowCorrectAnswer(string answer)
    {
        Debug.Log($"ShowCorrectAnswer {Language} C{UIcheckpoint}");

        if (Language == "Java")
        {
            if (UIcheckpoint == 2)
            {
                // Lazy load Level2Java
                if (level2Java == null)
                {
                    level2Java = FindObjectOfType<Level2Java>();
                    Debug.Log(level2Java == null ? "❌ Level2Java not found" : "✅ Level2Java found");
                }

                level2Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 3)
            {
                if (level3Java == null)
                {
                    level3Java = FindObjectOfType<Level3Java>();
                    Debug.Log(level3Java == null ? "❌ Level3Java not found" : "✅ Level3Java found");
                }

                level3Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 4)
            {
                if (level4Java == null)
                {
                    level4Java = FindObjectOfType<Level4Java>();
                    Debug.Log(level4Java == null ? "❌ Level4Java not found" : "✅ Level4Java found");
                }

                level4Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 5)
            {
                if (level5Java == null)
                {
                    level5Java = FindObjectOfType<Level5Java>();
                    Debug.Log(level5Java == null ? "❌ Level5Java not found" : "✅ Level5Java found");
                }

                level5Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 6)
            {
                if (level6Java == null)
                {
                    level6Java = FindObjectOfType<Level6Java>();
                    Debug.Log(level6Java == null ? "❌ Level6Java not found" : "✅ Level6Java found");
                }

                level6Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 7)
            {
                if (level7Java == null)
                {
                    level7Java = FindObjectOfType<Level7Java>();
                    Debug.Log(level7Java == null ? "❌ Level7Java not found" : "✅ Level7Java found");
                }

                level7Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 8)
            {
                if (level8Java == null)
                {
                    level8Java = FindObjectOfType<Level8Java>();
                    Debug.Log(level8Java == null ? "❌ Level8Java not found" : "✅ Level8Java found");
                }

                level8Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 9)
            {
                if (level9Java == null)
                {
                    level9Java = FindObjectOfType<Level9Java>();
                    Debug.Log(level9Java == null ? "❌ Level9Java not found" : "✅ Level9Java found");
                }

                level9Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 10)
            {
                if (level10Java == null)
                {
                    level10Java = FindObjectOfType<Level10Java>();
                    Debug.Log(level10Java == null ? "❌ Level10Java not found" : "✅ Level10Java found");
                }

                level10Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 11)
            {
                if (level11Java == null)
                {
                    level11Java = FindObjectOfType<Level11Java>();
                    Debug.Log(level11Java == null ? "❌ Level11Java not found" : "✅ Level11Java found");
                }

                level11Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 12)
            {
                if (level12Java == null)
                {
                    level12Java = FindObjectOfType<Level12Java>();
                    Debug.Log(level12Java == null ? "❌ Level12Java not found" : "✅ Level12Java found");
                }

                level12Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 13)
            {
                if (level13Java == null)
                {
                    level13Java = FindObjectOfType<Level13Java>();
                    Debug.Log(level13Java == null ? "❌ Level13Java not found" : "✅ Level13Java found");
                }

                level13Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 14)
            {
                if (level14Java == null)
                {
                    level14Java = FindObjectOfType<Level14Java>();
                    Debug.Log(level14Java == null ? "❌ Level15Java not found (Wrong)" : "✅ Level15Java found (Wrong)");
                }

                level14Java?.Correct(answer, player);
            }
            else if (UIcheckpoint == 15)
            {
                if (level15Java == null)
                {
                    level15Java = FindObjectOfType<Level15Java>();
                    Debug.Log(level15Java == null ? "❌ Level15Java not found" : "✅ Level15Java found");
                }

                level15Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 16)
            {
                if (level16Java == null)
                {
                    level16Java = FindObjectOfType<Level16Java>();
                    Debug.Log(level16Java == null ? "❌ Level16Java not found" : "✅ Level16Java found");
                }

                level16Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 17)
            {
                if (level17Java == null)
                {
                    level17Java = FindObjectOfType<Level17Java>();
                    Debug.Log(level17Java == null ? "❌ Level17Java not found" : "✅ Level17Java found");
                }

                level17Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 18)
            {
                Debug.Log($"Scene name: {sceneName}");
                if (sceneName.Equals("JavaLevel18-1"))
                {
                    if (level18Java_1 == null)
                    {
                        level18Java_1 = FindObjectOfType<Level18Java_1>();
                        Debug.Log(level18Java_1 == null ? "❌ Level18Java not found" : "✅ Level18Java found");
                    }

                    level18Java_1?.Correct(answer);
                }
                else if (sceneName.Equals("JavaLevel18-2"))
                {
                    if (level18Java_2 == null)
                    {
                        level18Java_2 = FindObjectOfType<Level18Java_2>();
                        Debug.Log(level18Java_2 == null ? "❌ Level18Java not found" : "✅ Level18Java found");
                    }

                    level18Java_2?.Correct(answer);
                }
                else if (sceneName.Equals("JavaLevel18-3"))
                {
                    if (level18Java_3 == null)
                    {
                        level18Java_3 = FindObjectOfType<Level18Java_3>();
                        Debug.Log(level18Java_3 == null ? "❌ Level18Java not found" : "✅ Level18Java found");
                    }

                    level18Java_3?.Correct(answer);
                }
            }

            else if (UIcheckpoint == 19)
            {
                if (level19Java == null)
                {
                    level19Java = FindObjectOfType<Level19Java>();
                    Debug.Log(level19Java == null ? "❌ Level19Java not found" : "✅ Level19Java found");
                }

                level19Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 20)
            {
                if (level20Java == null)
                {
                    level20Java = FindObjectOfType<Level20Java>();
                    Debug.Log(level20Java == null ? "❌ Level20Java not found" : "✅ Level20Java found");
                }

                level20Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 21)
            {
                if (level21Java == null)
                {
                    level21Java = FindObjectOfType<Level21Java>();
                    Debug.Log(level21Java == null ? "❌ Level21Java not found" : "✅ Level21Java found");
                }

                level21Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 22)
            {
                if (level22Java == null)
                {
                    level22Java = FindObjectOfType<Level22Java>();
                    Debug.Log(level22Java == null ? "❌ Level22Java not found" : "✅ Level22Java found");
                }

                level22Java?.Correct(answer, askText, askText2, player);
            }
            else if (UIcheckpoint == 23)
            {
                if (level23Java == null)
                {
                    level23Java = FindObjectOfType<Level23Java>();
                    Debug.Log(level23Java == null ? "❌ Level23Java not found" : "✅ Level23Java found");
                }

                level23Java?.Correct(answer, askText, askText2, player, player2);
            }
            else if (UIcheckpoint == 24)
            {
                if (level24Java == null)
                {
                    level24Java = FindObjectOfType<Level24Java>();
                    Debug.Log(level24Java == null ? "❌ Level24Java not found" : "✅ Level24Java found");
                }

                level24Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 25)
            {
                if (level25Java == null)
                {
                    level25Java = FindObjectOfType<Level25Java>();
                    Debug.Log(level25Java == null ? "❌ Level25Java not found" : "✅ Level25Java found");
                }

                level25Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 26)
            {
                if (level26Java == null)
                {
                    level26Java = FindObjectOfType<Level26Java>();
                    Debug.Log(level26Java == null ? "❌ Level26Java not found" : "✅ Level26Java found");
                }

                level26Java?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 27)
            {
                if (level27Java == null)
                {
                    level27Java = FindObjectOfType<Level27Java>();
                    Debug.Log(level27Java == null ? "❌ Level27Java not found" : "✅ Level26Java found");
                }

                level27Java?.Correct(answer);
            }
        }

        else if (Language == "Python")
        {
            if (UIcheckpoint == 2)
            {
                if (level2Python == null)
                {
                    level2Python = FindObjectOfType<Level2Python>();
                    Debug.Log(level2Python == null ? "❌ Level2Python not found" : "✅ Level2Python found");
                }

                level2Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 3)
            {
                if (level3Python == null)
                {
                    level3Python = FindObjectOfType<Level3Python>();
                    Debug.Log(level3Python == null ? "❌ Level3Python not found" : "✅ Level3Python found");
                }

                level3Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 4)
            {
                if (level4Python == null)
                {
                    level4Python = FindObjectOfType<Level4Python>();
                    Debug.Log(level4Python == null ? "❌ Level4Python not found" : "✅ Level4Python found");
                }

                level4Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 5)
            {
                if (level5Python == null)
                {
                    level5Python = FindObjectOfType<Level5Python>();
                    Debug.Log(level5Python == null ? "❌ Level5Python not found" : "✅ Level5Python found");
                }

                level5Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 6)
            {
                if (level6Python == null)
                {
                    level6Python = FindObjectOfType<Level6Python>();
                    Debug.Log(level6Python == null ? "❌ Level6Python not found" : "✅ Level6Python found");
                }

                level6Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 7)
            {
                if (level7Python == null)
                {
                    level7Python = FindObjectOfType<Level7Python>();
                    Debug.Log(level7Python == null ? "❌ Level7Python not found" : "✅ Level7Python found");
                }

                level7Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 8)
            {
                if (level8Python == null)
                {
                    level8Python = FindObjectOfType<Level8Python>();
                    Debug.Log(level8Python == null ? "❌ Level8Python not found" : "✅ Level8Python found");
                }

                level8Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 9)
            {
                if (level9Python == null)
                {
                    level9Python = FindObjectOfType<Level9Python>();
                    Debug.Log(level9Python == null ? "❌ Level9Python not found" : "✅ Level9Python found");
                }

                level9Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 10)
            {
                if (level10Python == null)
                {
                    level10Python = FindObjectOfType<Level10Python>();
                    Debug.Log(level10Python == null ? "❌ Level10Python not found" : "✅ Level10Python found");
                }

                level10Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 11)
            {
                if (level11Python == null)
                {
                    level11Python = FindObjectOfType<Level11Python>();
                    Debug.Log(level11Python == null ? "❌ Level11Python not found" : "✅ Level11Python found");
                }

                level11Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 12)
            {
                if (level12Python == null)
                {
                    level12Python = FindObjectOfType<Level12Python>();
                    Debug.Log(level12Python == null ? "❌ Level12Python not found" : "✅ Level12Python found");
                }

                level12Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 13)
            {
                if (level13Python == null)
                {
                    level13Python = FindObjectOfType<Level13Python>();
                    Debug.Log(level13Python == null ? "❌ Level13Python not found" : "✅ Level13Python found");
                }

                level13Python?.Correct(answer, player);
            }
            else if (UIcheckpoint == 14)
            {
                if (level14Python == null)
                {
                    level14Python = FindObjectOfType<Level14Python>();
                    Debug.Log(level14Python == null ? "❌ Level14Python not found" : "✅ Level14Python found");
                }

                level14Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 15)
            {
                if (level15Python == null)
                {
                    level15Python = FindObjectOfType<Level15Python>();
                    Debug.Log(level15Python == null ? "❌ Level15Python not found" : "✅ Level15Python found");
                }

                level15Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 16)
            {
                Debug.Log($"Scene name: {sceneName}");
                if (sceneName.Equals("PythonLevel16-1"))
                {
                    if (level16Python_1 == null)
                    {
                        level16Python_1 = FindObjectOfType<Level16Python_1>();
                        Debug.Log(level16Python_1 == null ? "❌ level16Python_1 not found" : "✅ level16Python_1 found");
                    }

                    level16Python_1?.Correct(answer);
                }
                else if (sceneName.Equals("PythonLevel16-2"))
                {
                    if (level16Python_2 == null)
                    {
                        level16Python_2 = FindObjectOfType<Level16Python_2>();
                        Debug.Log(level16Python_2 == null ? "❌ level16Python_2 not found" : "✅ level16Python_2 found");
                    }

                    level16Python_2?.Correct(answer);
                }
                else if (sceneName.Equals("PythonLevel16-3"))
                {
                    if (level16Python_3 == null)
                    {
                        level16Python_3 = FindObjectOfType<Level16Python_3>();
                        Debug.Log(level16Python_3 == null ? "❌ level16Python_3 not found" : "✅ level16Python_3 found");
                    }

                    level16Python_3?.Correct(answer);
                }
            }
            else if (UIcheckpoint == 17)
            {
                if (level17Python == null)
                {
                    level17Python = FindObjectOfType<Level17Python>();
                    Debug.Log(level17Python == null ? "❌ Level17Python not found" : "✅ Level17Python found");
                }

                level17Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 18)
            {
                if (level18Python == null)
                {
                    level18Python = FindObjectOfType<Level18Python>();
                    Debug.Log(level18Python == null ? "❌ Level18Python not found" : "✅ Level18Python found");
                }

                level18Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 19)
            {
                if (level19Python == null)
                {
                    level19Python = FindObjectOfType<Level19Python>();
                    Debug.Log(level19Python == null ? "❌ Level19Python not found" : "✅ Level19Python found");
                }

                level19Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 20)
            {
                if (level20Python == null)
                {
                    level20Python = FindObjectOfType<Level20Python>();
                    Debug.Log(level20Python == null ? "❌ Level20Python not found" : "✅ Level20Python found");
                }

                level20Python?.Correct(answer, askText, askText2, player);
            }
            else if (UIcheckpoint == 21)
            {
                if (level21Python == null)
                {
                    level21Python = FindObjectOfType<Level21Python>();
                    Debug.Log(level21Python == null ? "❌ Level21Python not found" : "✅ Level21Python found");
                }

                level21Python?.Correct(answer, askText, askText2, player, player2);
            }
            else if (UIcheckpoint == 22)
            {
                if (level22Python == null)
                {
                    level22Python = FindObjectOfType<Level22Python>();
                    Debug.Log(level22Python == null ? "❌ Level22Python not found" : "✅ Level22Python found");
                }

                level22Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 23)
            {
                if (level23Python == null)
                {
                    level23Python = FindObjectOfType<Level23Python>();
                    Debug.Log(level23Python == null ? "❌ Level23Python not found" : "✅ Level23Python found");
                }

                level23Python?.Correct(answer, askText, player);
            }
            else if (UIcheckpoint == 24)
            {
                if (level24Python == null)
                {
                    level24Python = FindObjectOfType<Level24Python>();
                    Debug.Log(level24Python == null ? "❌ Level24Python not found" : "✅ Level24Python found");
                }

                level24Python?.Correct(answer, askText, player);
            }
        }
    }

    [System.Obsolete]
    public void ShowWrongAnswer(string answer)
    {
        Debug.Log($"ShowWrongAnswer {Language} C{UIcheckpoint}");

        if (Language == "Java")
        {
            if (UIcheckpoint == 2)
            {
                if (level2Java == null)
                {
                    level2Java = FindObjectOfType<Level2Java>();
                    Debug.Log(level2Java == null ? "❌ Level2Java not found (Wrong)" : "✅ Level2Java found (Wrong)");
                }

                level2Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 3)
            {
                if (level3Java == null)
                {
                    level3Java = FindObjectOfType<Level3Java>();
                    Debug.Log(level3Java == null ? "❌ Level3Java not found (Wrong)" : "✅ Level3Java found (Wrong)");
                }

                level3Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 4)
            {
                if (level4Java == null)
                {
                    level4Java = FindObjectOfType<Level4Java>();
                    Debug.Log(level4Java == null ? "❌ Level4Java not found" : "✅ Level4Java found");
                }

                level4Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 5)
            {
                if (level5Java == null)
                {
                    level5Java = FindObjectOfType<Level5Java>();
                    Debug.Log(level5Java == null ? "❌ Level5Java not found" : "✅ Level5Java found");
                }

                level5Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 6)
            {
                if (level6Java == null)
                {
                    level6Java = FindObjectOfType<Level6Java>();
                    Debug.Log(level6Java == null ? "❌ Level6Java not found" : "✅ Level6Java found");
                }

                level6Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 7)
            {
                if (level7Java == null)
                {
                    level7Java = FindObjectOfType<Level7Java>();
                    Debug.Log(level7Java == null ? "❌ Level7Java not found" : "✅ Level7Java found");
                }

                level7Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 8)
            {
                if (level8Java == null)
                {
                    level8Java = FindObjectOfType<Level8Java>();
                    Debug.Log(level8Java == null ? "❌ Level8Java not found" : "✅ Level8Java found");
                }

                level8Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 9)
            {
                if (level9Java == null)
                {
                    level9Java = FindObjectOfType<Level9Java>();
                    Debug.Log(level9Java == null ? "❌ Level9Java not found (Wrong)" : "✅ Level9Java found (Wrong)");
                }

                level9Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 10)
            {
                if (level10Java == null)
                {
                    level10Java = FindObjectOfType<Level10Java>();
                    Debug.Log(level10Java == null ? "❌ Level10Java not found (Wrong)" : "✅ Level10Java found (Wrong)");
                }

                level10Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 11)
            {
                if (level11Java == null)
                {
                    level11Java = FindObjectOfType<Level11Java>();
                    Debug.Log(level11Java == null ? "❌ Level11Java not found (Wrong)" : "✅ Level11Java found (Wrong)");
                }

                level11Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 12)
            {
                if (level12Java == null)
                {
                    level12Java = FindObjectOfType<Level12Java>();
                    Debug.Log(level12Java == null ? "❌ Level12Java not found (Wrong)" : "✅ Level12Java found (Wrong)");
                }

                level12Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 13)
            {
                if (level13Java == null)
                {
                    level13Java = FindObjectOfType<Level13Java>();
                    Debug.Log(level13Java == null ? "❌ Level13Java not found (Wrong)" : "✅ Level13Java found (Wrong)");
                }

                level13Java?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 14)
            {
                if (level14Java == null)
                {
                    level14Java = FindObjectOfType<Level14Java>();
                    Debug.Log(level14Java == null ? "❌ Level15Java not found (Wrong)" : "✅ Level15Java found (Wrong)");
                }

                level14Java?.Wrong(answer, player);
            }
            else if (UIcheckpoint == 15)
            {
                if (level15Java == null)
                {
                    level15Java = FindObjectOfType<Level15Java>();
                    Debug.Log(level15Java == null ? "❌ Level15Java not found (Wrong)" : "✅ Level15Java found (Wrong)");
                }

                level15Java?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 16)
            {
                if (level16Java == null)
                {
                    level16Java = FindObjectOfType<Level16Java>();
                    Debug.Log(level16Java == null ? "❌ Level16Java not found (Wrong)" : "✅ Level16Java found (Wrong)");
                }

                level16Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 17)
            {
                if (level17Java == null)
                {
                    level17Java = FindObjectOfType<Level17Java>();
                    Debug.Log(level17Java == null ? "❌ Level17Java not found (Wrong)" : "✅ Level17Java found (Wrong)");
                }

                level17Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 18)
            {
                Debug.Log($"Scene name: {sceneName}");
                if (sceneName.Equals("JavaLevel18-1"))
                {
                    if (level18Java_1 == null)
                    {
                        level18Java_1 = FindObjectOfType<Level18Java_1>();
                        Debug.Log(level18Java_1 == null ? "❌ Level18Java not found" : "✅ Level18Java found");
                    }

                    level18Java_1?.Wrong(answer);
                }
                else if (sceneName.Equals("JavaLevel18-2"))
                {
                    if (level18Java_2 == null)
                    {
                        level18Java_2 = FindObjectOfType<Level18Java_2>();
                        Debug.Log(level18Java_2 == null ? "❌ Level18Java not found" : "✅ Level18Java found");
                    }

                    level18Java_2?.Wrong(answer);
                }
                else if (sceneName.Equals("JavaLevel18-3"))
                {
                    if (level18Java_3 == null)
                    {
                        level18Java_3 = FindObjectOfType<Level18Java_3>();
                        Debug.Log(level18Java_3 == null ? "❌ Level18Java not found" : "✅ Level18Java found");
                    }

                    level18Java_3?.Wrong(answer);
                }
            }
            else if (UIcheckpoint == 19)
            {
                if (level19Java == null)
                {
                    level19Java = FindObjectOfType<Level19Java>();
                    Debug.Log(level19Java == null ? "❌ Level19Java not found" : "✅ Level19Java found");
                }

                level19Java?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 20)
            {
                if (level20Java == null)
                {
                    level20Java = FindObjectOfType<Level20Java>();
                    Debug.Log(level20Java == null ? "❌ Level20Java not found" : "✅ Level20Java found");
                }

                level20Java?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 21)
            {
                if (level21Java == null)
                {
                    level21Java = FindObjectOfType<Level21Java>();
                    Debug.Log(level21Java == null ? "❌ Level21Java not found" : "✅ Level21Java found");
                }

                level21Java?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 22)
            {
                if (level22Java == null)
                {
                    level22Java = FindObjectOfType<Level22Java>();
                    Debug.Log(level22Java == null ? "❌ Level22Java not found" : "✅ Level22Java found");
                }

                level22Java?.Wrong(askText, askText2, player);
            }
            else if (UIcheckpoint == 23)
            {
                if (level23Java == null)
                {
                    level23Java = FindObjectOfType<Level23Java>();
                    Debug.Log(level23Java == null ? "❌ Level23Java not found" : "✅ Level23Java found");
                }

                level23Java?.Wrong(askText, askText2, player, player2);
            }
            else if (UIcheckpoint == 24)
            {
                if (level24Java == null)
                {
                    level24Java = FindObjectOfType<Level24Java>();
                    Debug.Log(level24Java == null ? "❌ Level24Java not found" : "✅ Level24Java found");
                }

                level24Java?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 25)
            {
                if (level25Java == null)
                {
                    level25Java = FindObjectOfType<Level25Java>();
                    Debug.Log(level25Java == null ? "❌ Level25Java not found" : "✅ Level25Java found");
                }

                level25Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 26)
            {
                if (level26Java == null)
                {
                    level26Java = FindObjectOfType<Level26Java>();
                    Debug.Log(level26Java == null ? "❌ Level26Java not found" : "✅ Level26Java found");
                }

                level26Java?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 27)
            {
                if (level27Java == null)
                {
                    level27Java = FindObjectOfType<Level27Java>();
                    Debug.Log(level27Java == null ? "❌ Level27Java not found" : "✅ Level26Java found");
                }

                level27Java?.Wrong(answer);
            }
        }
        else if (Language == "Python")
        {
            if (UIcheckpoint == 2)
            {
                if (level2Python == null)
                {
                    level2Python = FindObjectOfType<Level2Python>();
                    Debug.Log(level2Python == null ? "❌ Level2Python not found (Wrong)" : "✅ Level2Python found (Wrong)");
                }

                level2Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 3)
            {
                if (level3Python == null)
                {
                    level3Python = FindObjectOfType<Level3Python>();
                    Debug.Log(level3Python == null ? "❌ Level3Python not found (Wrong)" : "✅ Level3Python found (Wrong)");
                }

                level3Python?.Wrong(askText, player);

            }
            else if (UIcheckpoint == 4)
            {
                if (level4Python == null)
                {
                    level4Python = FindObjectOfType<Level4Python>();
                    Debug.Log(level4Python == null ? "❌ Level4Python not found (Wrong)" : "✅ Level4Python found (Wrong)");
                }

                level4Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 5)
            {
                if (level5Python == null)
                {
                    level5Python = FindObjectOfType<Level5Python>();
                    Debug.Log(level5Python == null ? "❌ Level5Python not found (Wrong)" : "✅ Level5Python found (Wrong)");
                }

                level5Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 6)
            {
                if (level6Python == null)
                {
                    level6Python = FindObjectOfType<Level6Python>();
                    Debug.Log(level6Python == null ? "❌ Level6Python not found (Wrong)" : "✅ Level6Python found (Wrong)");
                }

                level6Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 7)
            {
                if (level7Python == null)
                {
                    level7Python = FindObjectOfType<Level7Python>();
                    Debug.Log(level7Python == null ? "❌ Level7Python not found (Wrong)" : "✅ Level7Python found (Wrong)");
                }

                level7Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 8)
            {
                if (level8Python == null)
                {
                    level8Python = FindObjectOfType<Level8Python>();
                    Debug.Log(level8Python == null ? "❌ Level8Python not found (Wrong)" : "✅ Level8Python found (Wrong)");
                }

                level8Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 9)
            {
                if (level9Python == null)
                {
                    level9Python = FindObjectOfType<Level9Python>();
                    Debug.Log(level9Python == null ? "❌ Level9Python not found (Wrong)" : "✅ Level9Python found (Wrong)");
                }

                level9Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 10)
            {
                if (level10Python == null)
                {
                    level10Python = FindObjectOfType<Level10Python>();
                    Debug.Log(level10Python == null ? "❌ Level10Python not found (Wrong)" : "✅ Level10Python found (Wrong)");
                }

                level10Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 11)
            {
                if (level11Python == null)
                {
                    level11Python = FindObjectOfType<Level11Python>();
                    Debug.Log(level11Python == null ? "❌ Level11Python not found (Wrong)" : "✅ Level11Python found (Wrong)");
                }

                level11Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 12)
            {
                if (level12Python == null)
                {
                    level12Python = FindObjectOfType<Level12Python>();
                    Debug.Log(level12Python == null ? "❌ Level12Python not found (Wrong)" : "✅ Level12Python found (Wrong)");
                }

                level12Python?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 13)
            {
                if (level13Python == null)
                {
                    level13Python = FindObjectOfType<Level13Python>();
                    Debug.Log(level13Python == null ? "❌ Level13Python not found (Wrong)" : "✅ Level13Python found (Wrong)");
                }

                level13Python?.Wrong(answer, player);
            }
            else if (UIcheckpoint == 14)
            {
                if (level14Python == null)
                {
                    level14Python = FindObjectOfType<Level14Python>();
                    Debug.Log(level14Python == null ? "❌ Level14Python not found (Wrong)" : "✅ Level14Python found (Wrong)");
                }

                level14Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 15)
            {
                if (level15Python == null)
                {
                    level15Python = FindObjectOfType<Level15Python>();
                    Debug.Log(level15Python == null ? "❌ Level15Python not found (Wrong)" : "✅ Level15Python found (Wrong)");
                }

                level15Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 16)
            {
                Debug.Log($"Scene name: {sceneName}");
                if (sceneName.Equals("PythonLevel16-1"))
                {
                    if (level16Python_1 == null)
                    {
                        level16Python_1 = FindObjectOfType<Level16Python_1>();
                        Debug.Log(level16Python_1 == null ? "❌ level16Python_1 not found" : "✅ level16Python_1 found");
                    }

                    level16Python_1?.Wrong(answer);
                }
                else if (sceneName.Equals("PythonLevel16-2"))
                {
                    if (level16Python_2 == null)
                    {
                        level16Python_2 = FindObjectOfType<Level16Python_2>();
                        Debug.Log(level16Python_2 == null ? "❌ level16Python_2 not found" : "✅ level16Python_2 found");
                    }

                    level16Python_2?.Wrong(answer);
                }
                else if (sceneName.Equals("PythonLevel16-3"))
                {
                    if (level16Python_3 == null)
                    {
                        level16Python_3 = FindObjectOfType<Level16Python_3>();
                        Debug.Log(level16Python_3 == null ? "❌ level16Python_3 not found" : "✅ level16Python_3 found");
                    }

                    level16Python_3?.Wrong(answer);
                }
            }
            else if (UIcheckpoint == 17)
            {
                if (level17Python == null)
                {
                    level17Python = FindObjectOfType<Level17Python>();
                    Debug.Log(level17Python == null ? "❌ Level17Python not found (Wrong)" : "✅ Level17Python found (Wrong)");
                }

                level17Python?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 18)
            {
                if (level18Python == null)
                {
                    level18Python = FindObjectOfType<Level18Python>();
                    Debug.Log(level18Python == null ? "❌ Level18Python not found (Wrong)" : "✅ Level18Python found (Wrong)");
                }

                level18Python?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 19)
            {
                if (level19Python == null)
                {
                    level19Python = FindObjectOfType<Level19Python>();
                    Debug.Log(level19Python == null ? "❌ Level19Python not found (Wrong)" : "✅ Level19Python found (Wrong)");
                }

                level19Python?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 20)
            {
                if (level20Python == null)
                {
                    level20Python = FindObjectOfType<Level20Python>();
                    Debug.Log(level20Python == null ? "❌ Level20Python not found (Wrong)" : "✅ Level20Python found (Wrong)");
                }

                level20Python?.Wrong(askText, askText2, player);
            }
            else if (UIcheckpoint == 21)
            {
                if (level21Python == null)
                {
                    level21Python = FindObjectOfType<Level21Python>();
                    Debug.Log(level21Python == null ? "❌ Level21Python not found (Wrong)" : "✅ Level21Python found (Wrong)");
                }

                level21Python?.Wrong(askText, askText2, player, player2);
            }
            else if (UIcheckpoint == 22)
            {
                if (level22Python == null)
                {
                    level22Python = FindObjectOfType<Level22Python>();
                    Debug.Log(level22Python == null ? "❌ Level22Python not found (Wrong)" : "✅ Level22Python found (Wrong)");
                }

                level22Python?.Wrong(answer, askText, player);
            }
            else if (UIcheckpoint == 23)
            {
                if (level23Python == null)
                {
                    level23Python = FindObjectOfType<Level23Python>();
                    Debug.Log(level23Python == null ? "❌ Level23Python not found (Wrong)" : "✅ Level23Python found (Wrong)");
                }

                level23Python?.Wrong(askText, player);
            }
            else if (UIcheckpoint == 24)
            {
                if (level24Python == null)
                {
                    level24Python = FindObjectOfType<Level24Python>();
                    Debug.Log(level24Python == null ? "❌ Level24Python not found (Wrong)" : "✅ Level24Python found (Wrong)");
                }

                level24Python?.Wrong(askText, player);
            }
        }
    }

    public void HideCharacter()
    {
        if (player != null && player.CurrentCharacter != null)
        {
            player.CurrentCharacter.SetActive(false);
        }
        if (player2 != null && player2.CurrentCharacter != null)
        {
            player2.CurrentCharacter.SetActive(false);
        }
    }

    public void ResetUI()
    {
        askText.text = "...";

        if (player != null && player.CurrentCharacter != null)
        {
            Animator animator = player.CurrentCharacter.GetComponent<Animator>();
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.SetTrigger("Idle");
            }
            else
            {
                Debug.LogWarning("⚠️ Animator หรือ Animator Controller ยังไม่พร้อม (Idle)");
            }
        }
    }
}
