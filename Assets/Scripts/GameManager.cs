using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

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

    private List<Rigidbody2D> _controllableMoons;

    private bool _started;

    private void Start() {
        Instance = this;
        _controllableMoons = GameObject.FindGameObjectsWithTag("Controllable Moon")
            .Select(moon => moon.GetComponent<Rigidbody2D>())
            .ToList();

        parText.text = $"Par: {par}";
        CanShoot = true;
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
        Debug.Log($"End turn. Shots: {Shots}");
    }

    private void RemoveMoon(Rigidbody2D moonRigidBody2D) {
        _controllableMoons.Remove(moonRigidBody2D);
    }
}
