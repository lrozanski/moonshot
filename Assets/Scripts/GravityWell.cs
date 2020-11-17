using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class GravityWell : MonoBehaviour {

    [SerializeField]
    private float gravity;

    private readonly HashSet<Rigidbody2D> _affectedMoons = new HashSet<Rigidbody2D>();
    private CircleCollider2D _gravityWellCollider;

    private void Start() {
        _gravityWellCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        _affectedMoons.Add(other.GetComponent<Rigidbody2D>());
    }

    private void OnTriggerExit2D(Collider2D other) {
        _affectedMoons.Remove(other.GetComponent<Rigidbody2D>());
    }

    private void FixedUpdate() {
        foreach (var moon in _affectedMoons) {
            var gravityVector = transform.position - moon.transform.position;
            var gravityAmount = Mathf.Lerp(gravity, 0f, gravityVector.magnitude / _gravityWellCollider.radius);

            if (!moon.IsSleeping()) {
                moon.AddForce(gravityVector * gravityAmount);
            }
        }
    }
}
