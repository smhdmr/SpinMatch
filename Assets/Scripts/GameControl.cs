using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YsoCorp.GameUtils;

public class GameControl : MonoBehaviour
{
    public GameObject Puzzle;

    public bool[] piecesCompleteBools = new bool[4];

    int currentScene;

    public bool isFinish = false;
    public bool isHintAvaible = true;

    public static GameControl Instance;

    public int random;
    public int randomPiece;

    public Quaternion rotation1;
    public Quaternion rotation2;
    public List<int> PiecesList;
    public List<GameObject> Clouds;
    public List<Transform> Transforms;
    public Animator transition;
    public Animator finishAni;
    public Animator FingerAni;
    public Animator PuzzleAni;


    public int hint;
    public Text hintText;

    public GameObject PuzzleFinis;
    public ParticleSystem Confetti1;
    public ParticleSystem Confetti2;



    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        YCManager.instance.OnGameStarted(currentScene);

        Confetti1.Stop();
        Confetti2.Stop();

        for (int i = 0; i < Puzzle.transform.childCount ; i++)
        {
            PiecesList.Add(i);
        }
        currentScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("level", currentScene);

        if (gameObject.tag == "Level1")
        {
            hint = 3;
            PlayerPrefs.SetInt("hint", 3);
        }
        StartCoroutine(TutorialAni());
    }

    // Update is called once per frame
    void Update()
    {

        hintText.text = "X" + PlayerPrefs.GetInt("hint");

        piecesCompleteBools[0] = Puzzle.transform.GetChild(0).GetComponent<ObjectControl>().isPieceComplete;

        if (Puzzle.transform.childCount > 1)
        {
            piecesCompleteBools[1] = Puzzle.transform.GetChild(1).GetComponent<ObjectControl>().isPieceComplete;
        }
        else
        {
            piecesCompleteBools[1] = true;
        }

        if (Puzzle.transform.childCount > 2)
        {
            piecesCompleteBools[2] = Puzzle.transform.GetChild(2).GetComponent<ObjectControl>().isPieceComplete;
        }
        else
        {
            piecesCompleteBools[2] = true;
        }

        if (Puzzle.transform.childCount > 3)
        {
            piecesCompleteBools[3] = Puzzle.transform.GetChild(3).GetComponent<ObjectControl>().isPieceComplete;
        }
        else 
        {
            piecesCompleteBools[3] = true;
        }

        if (isAllPiecesComplete() == true)
        {
            isFinish = true;

            YCManager.instance.OnGameFinished(true);

            if (!Confetti1.isPlaying)
            {
                Confetti1.Play();
            }

            if (!Confetti2.isPlaying)
            {
                Confetti2.Play();
            }
            StartCoroutine(Finish());

            Puzzle.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
            Puzzle.transform.GetChild(1).transform.localEulerAngles = new Vector3(0, 0, 0);

         //   Puzzle.transform.GetChild(0).transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
           // Puzzle.transform.GetChild(1).transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            if (Puzzle.transform.childCount > 2)
            {
               // Puzzle.transform.GetChild(2).transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                Puzzle.transform.GetChild(2).transform.localEulerAngles = new Vector3(0, 0, 0);

            }
            if (Puzzle.transform.childCount > 3)
            {
               // Puzzle.transform.GetChild(3).transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                Puzzle.transform.GetChild(3).transform.localEulerAngles = new Vector3(0, 0, 0);

            }

        }

    }

    private bool isAllPiecesComplete()
    {
        for (int i = 0; i < piecesCompleteBools.Length; ++i)
        {
            if (piecesCompleteBools[i] == false)
            {
                return false;
            }
        }
        return true;
    }

    public void HintButton()
    {
        Vibration.Vibrate(40);

        if (isHintAvaible && PlayerPrefs.GetInt("hint") > 0 && !isFinish )
        {
            PlayerPrefs.SetInt("hint", PlayerPrefs.GetInt("hint") - 1);
            random = Random.Range(0, PiecesList.Count);
            randomPiece = PiecesList[random];
            PiecesList.RemoveAt(random);

            StartCoroutine(RotateOverTime());
            isHintAvaible = false;
        }
        
    }

    IEnumerator RotateOverTime()
    {
        if (Puzzle.transform.GetChild(0).tag == "yAxis")
        {
            rotation1 = Quaternion.Euler(Puzzle.transform.GetChild(randomPiece).transform.localRotation.eulerAngles.x, Puzzle.transform.GetChild(randomPiece).transform.localRotation.eulerAngles.y, Puzzle.transform.GetChild(randomPiece).transform.localRotation.eulerAngles.z);
            rotation2 = Quaternion.Euler(Puzzle.transform.GetChild(randomPiece).transform.localRotation.eulerAngles.x, 0, Puzzle.transform.GetChild(randomPiece).transform.localRotation.eulerAngles.z);
        }
        if (Puzzle.transform.GetChild(0).tag == "zAxis")
        {
            rotation1 = Quaternion.Euler(Puzzle.transform.GetChild(randomPiece).transform.localRotation.eulerAngles.x, Puzzle.transform.GetChild(randomPiece).transform.localRotation.eulerAngles.y, Puzzle.transform.GetChild(randomPiece).transform.localRotation.eulerAngles.z);
            rotation2 = Quaternion.Euler(Puzzle.transform.GetChild(randomPiece).transform.localRotation.eulerAngles.x, Puzzle.transform.GetChild(randomPiece).transform.localRotation.eulerAngles.y,0);
        }


        var a = Quaternion.Angle(rotation1, rotation2);
        var dur = a / 100f;
        float t = 0f;
        while (t < dur)
        {
            Puzzle.transform.GetChild(randomPiece).transform.localRotation = Quaternion.Slerp(rotation1, rotation2, t / dur);

            yield return null;
            t += Time.deltaTime;
        }
        Puzzle.transform.GetChild(randomPiece).transform.localRotation = rotation2;
        isHintAvaible = true;

    }

    IEnumerator Finish()
    {
     
        
        PuzzleFinis.SetActive(true);
        finishAni.SetTrigger("Start");
        Puzzle.SetActive(false);

        yield return new WaitForSeconds(3);

        transition.SetTrigger("Start");
        StartCoroutine(SceneTransition());

    }
    IEnumerator SceneTransition()
    {

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(currentScene + 1);

    }

    IEnumerator TutorialAni()
    {
        yield return new WaitForSeconds(2f);
        if(FingerAni != null && PuzzleAni != null)
        {
            FingerAni.SetTrigger("Start");
            PuzzleAni.SetTrigger("Start");
        }
        
    }
}
