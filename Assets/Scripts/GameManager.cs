using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {

    private const string TotalShots = "TotalShots";
    private const string PreviousTotalShots = "PreviousTotalShots";

    public static GameManager Instance { get; private set; }

    public int Shots { get; private set; }

    public bool CanShoot { get; private set; }

    [SerializeField]
    private Moon targetMoon;

    [SerializeField]
    private int par;

    [SerializeField]
    private TextMeshProUGUI shotsText;

    [SerializeField]
    private TextMeshProUGUI parText;

    [SerializeField]
    private TextMeshProUGUI fastForwardText;

    [SerializeField]
    private float fastForwardSpeed = 5f;
    
    private List<Rigidbody2D> _controllableMoons;

    private bool _started;

    private void Start() {
        Instance = this;
        _controllableMoons = GameObject.FindGameObjectsWithTag("Controllable Moon")
            .Select(moon => moon.GetComponent<Rigidbody2D>())
            .ToList();

        parText.text = $"Par: {par}";
        CanShoot = true;

        if (SceneManager.GetActiveScene().buildIndex == 0) {
            var prevShots = PlayerPrefs.GetInt(TotalShots, 0);
            if (prevShots > 0) {
                PlayerPrefs.SetInt(PreviousTotalShots, prevShots);
            }
            PlayerPrefs.SetInt(TotalShots, 0);
        }
    }

    private void OnEnable() {
        targetMoon.onControllableMoonAbsorbed += RemoveMoon;
    }

    private void OnDisable() {
        targetMoon.onControllableMoonAbsorbed -= RemoveMoon;
    }

    private void Update() {
        if (!_started) {
            return;
        }
        var moving = _controllableMoons.Any(moon => moon != null && moon.bodyType != RigidbodyType2D.Kinematic);
        if (moving) {
            CanShoot = false;

            if (Input.GetMouseButton((int) MouseButton.LeftMouse)) {
                fastForwardText.gameObject.SetActive(true);
                Time.timeScale = fastForwardSpeed;
            } else {
                fastForwardText.gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        } else {
            fastForwardText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        if (!CanShoot && !moving) {
            EndTurn();
        }
    }

    public void StartLevel() {
        _started = true;
    }

    public void EndTurn() {
        CanShoot = true;
        Shots++;

        shotsText.text = $"Shots: {Shots}";
    }

    private void RemoveMoon(Rigidbody2D moonRigidBody2D) {
        _controllableMoons.Remove(moonRigidBody2D);
    }
}
