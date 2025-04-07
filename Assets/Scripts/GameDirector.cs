using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    public static GameDirector instance;
    public LevelInfo mainMenuInfo;
    public List<LevelInfo> levels;
    public ForwardMovement forwardMovement;
    public ShipMovement ship;
    public AudioSource enterLevelSound;
    public AudioSource musicSound;
    public int nextLevel = 0;
    public float transitionTime = -1;
    private int soundState = 0;
    public enum GameState {
        InLevel,
        InMenu,
    }
    public GameState state = GameState.InMenu;
    private LevelEndAnimator endAnim;
    public Image screenBlack;
    public float levelStartTime = -100;
    public GameObject menuItems;
    void OnEnable() {
        instance = this;
    }
    void Start() {
        endAnim = forwardMovement.GetComponent<LevelEndAnimator>();
        endAnim.currentLevel = mainMenuInfo;
        endAnim.endAnimStartTime = Time.time - 6;
        endAnim.soundState = 3;
    }
    void Update() {
        menuItems.SetActive(state == GameState.InMenu);

        if (transitionTime <= 0 && state == GameState.InMenu) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (nextLevel < levels.Count) {
                    transitionTime = Time.time;
                    endAnim.originalCamPos = Camera.main.transform.position;
                    soundState = 0;
                    if (nextLevel == 0) {
                        musicSound.Play();
                    }
                } else {
                    Debug.Log($"No more levels!");
                }
            }
        }

        if (transitionTime > 0) {
            float dt = Time.time - transitionTime;
            if (dt > 0 && soundState == 0) { enterLevelSound.Play(); soundState++; }
            if (dt > 1.5f) {
                forwardMovement.currentLevel = levels[nextLevel];
                forwardMovement.transform.position = levels[nextLevel].startPos.position;
                endAnim.endAnimStartTime = -100;
                endAnim.soundState = 0;
                endAnim.currentLevel = levels[nextLevel];
                endAnim.audioSource.Stop();
                state = GameState.InLevel;
                transitionTime = -1;
                levelStartTime = Time.time;
            }
        }

        float a = 0;
        if (state == GameState.InMenu && transitionTime > 0) {
            a = Mathf.Clamp01((Time.time - transitionTime - 1.1f) / .4f);
        }
        if (state == GameState.InLevel) {
            a = 1 - Mathf.Clamp01((Time.time - levelStartTime)/.4f);
        }
        screenBlack.color = new Color(0, 0, 0, a);
    }

    public static bool PlayingLevel() {
        return instance.state == GameState.InLevel;
    }
}
