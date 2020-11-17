using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ControllableMoon : MonoBehaviour {

    [SerializeField]
    private LineRenderer forceLine;

    [SerializeField]
    private float maxLength;

    private bool _selected;
    private Vector2 _aimVector;
    private Rigidbody2D _rigidbody2D;

    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown() {
        _selected = true;
        forceLine.enabled = true;
    }

    private void OnMouseUp() {
        if (_selected) {
            forceLine.enabled = false;
            _rigidbody2D.AddForce(_aimVector, ForceMode2D.Impulse);
        }
        _selected = false;
    }

    private void Update() {
        if (!_selected) {
            return;
        }
        var position = transform.position;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = position.z;

        var vectorToPlanet = position - mouseWorldPos;
        var directionToPlanet = vectorToPlanet.normalized;
        if (vectorToPlanet.magnitude > maxLength) {
            vectorToPlanet = directionToPlanet * maxLength;
        }
        _aimVector = vectorToPlanet;

        forceLine.SetPositions(new[] {
            position,
            position - vectorToPlanet
        });
        forceLine.endWidth = vectorToPlanet.magnitude * 0.2f;
    }
}
