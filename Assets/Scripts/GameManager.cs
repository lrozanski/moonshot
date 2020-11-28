using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public int Shots { get; private set; }

    public bool CanShoot { get; private set; }

    [SerializeField]
    private Moon targetMoon;

    private List<Rigidbody2D> _controllableMoons;

    private void Start() {
        Instance = this;
        _controllableMoons = GameObject.FindGameObjectsWithTag("Controllable Moon")
            .Select(moon => moon.GetComponent<Rigidbody2D>())
            .ToList();
    }

    private void OnEnable() {
        targetMoon.onControllableMoonAbsorbed += RemoveMoon;
    }

    private void OnDisable() {
        targetMoon.onControllableMoonAbsorbed -= RemoveMoon;
    }

    private void Update() {
        var moving = _controllableMoons.Any(moon => moon.bodyType != RigidbodyType2D.Kinematic);
        if (moving) {
            CanShoot = false;
        }

        if (!CanShoot && !moving) {
            EndTurn();
        }
    }

    public void EndTurn() {
        CanShoot = true;
        Shots++;

        Debug.Log("End turn");
    }

    private void RemoveMoon(Rigidbody2D moonRigidBody2D) {
        _controllableMoons.Remove(moonRigidBody2D);
    }
}
