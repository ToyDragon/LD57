using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public GameObject baseStageIndicator;
    public List<UIStageIndicator> stageIndicators = new List<UIStageIndicator>();
    public GameObject drillIcon;
    public GameObject pressSpace;
    public float levelCompleteAnimStart = 0;
    public GameObject winText;
    public GameObject wasdHint;
    public GameObject clickHint;
    private bool everWASD = false;
    private bool everClick = false;
    void OnEnable() {
        instance = this;
        foreach (var level in levels) {
            level.gameObject.SetActive(false);
        }
    }
    void Start() {
        endAnim = forwardMovement.GetComponent<LevelEndAnimator>();
        endAnim.currentLevel = mainMenuInfo;
        endAnim.endAnimStartTime = Time.time - 6;
        endAnim.soundState = 3;

        var ind = baseStageIndicator.GetComponent<UIStageIndicator>();
        ind.ix = 0;
        ind.stageCount = levels.Count;
        ind.text.text = $"1000m";
        stageIndicators.Add(ind);
        for (int i = 1; i < levels.Count; i++) {
            var go = GameObject.Instantiate(baseStageIndicator);
            go.transform.SetParent(baseStageIndicator.transform.parent);
            ind = go.GetComponent<UIStageIndicator>();
            ind.ix = i;
            ind.stageCount = levels.Count;
            ind.text.text = $"{i+1}000m";
            stageIndicators.Add(ind);
        }
    }
    void Update() {
        menuItems.SetActive(state == GameState.InMenu);

        winText.SetActive(nextLevel >= levels.Count);

        wasdHint.SetActive(state == GameState.InLevel && !everWASD);
        clickHint.SetActive(state == GameState.InLevel && !everClick);

        if (state == GameState.InLevel) {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
                everWASD = true;
            }
            if (Input.GetKey(KeyCode.Mouse0)) {
                everClick = true;
            }
        }


        if (transitionTime <= 0 && state == GameState.InMenu) {
            if (nextLevel == 0) {
                drillIcon.transform.position = stageIndicators[nextLevel].transform.position + Vector3.up * 40;
            } else {
                if (levelCompleteAnimStart == 0) {
                    levelCompleteAnimStart = Time.time;
                }
                if (Time.time - levelCompleteAnimStart < 4 && nextLevel < stageIndicators.Count - 1) {
                    float drillT = Mathf.Clamp01((Time.time - levelCompleteAnimStart - 1) / 3);
                    drillIcon.transform.position = Vector3.Lerp(stageIndicators[nextLevel-1].transform.position, stageIndicators[nextLevel].transform.position, drillT) + Vector3.up * 40;
                    pressSpace.SetActive(false);
                    return;
                }
            }
            pressSpace.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (nextLevel < levels.Count) {
                    transitionTime = Time.time;
                    endAnim.originalCamPos = Camera.main.transform.position;
                    soundState = 0;
                    if (nextLevel == 0) {
                        musicSound.Play();
                    } else {
                        levels[nextLevel-1].gameObject.SetActive(false);
                    }
                    levels[nextLevel].gameObject.SetActive(true);
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
                levelCompleteAnimStart = 0;
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
