using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ControllableMoon : MonoBehaviour {

    [SerializeField]
    private LineRenderer forceLine;

    [SerializeField]
    private float forceMultiplier = 1f;

    [SerializeField]
    private float maxLength = 2;

    [field: SerializeField]
    public float PositionDelta { get; private set; }

    [field: SerializeField]
    public float DistanceTraveled { get; private set; }

    private bool _selected;
    private Vector2 _aimVector;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _startPosition;
    private Vector2 _previousPosition;

    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
    }

    private void OnMouseDown() {
        if (!GameManager.Instance.CanShoot || _rigidbody2D.bodyType != RigidbodyType2D.Kinematic) {
            return;
        }
        _selected = true;
        forceLine.enabled = true;
    }

    private void OnMouseUp() {
        if (_selected) {
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            forceLine.enabled = false;
            _rigidbody2D.AddForce(_aimVector * forceMultiplier, ForceMode2D.Impulse);

            GameManager.Instance.StartLevel();
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

    private void OnCollisionEnter2D(Collision2D other) {
        if (_rigidbody2D.bodyType != RigidbodyType2D.Kinematic) {
            return;
        }
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody2D.AddForce(other.relativeVelocity / 2f, ForceMode2D.Impulse);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("MainCamera")) {
            return;
        }
        Reset();
    }

    private void Reset() {
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.Sleep();

        transform.position = _startPosition;
        Physics2D.SyncTransforms();

        DistanceTraveled = 0f;
    }

    private void FixedUpdate() {
        var position = transform.position;

        PositionDelta = Vector2.Distance(position, _previousPosition);
        DistanceTraveled += PositionDelta;
        _previousPosition = position;
    }

    private void LateUpdate() {
        if (!(DistanceTraveled > 0f) || !(PositionDelta < 0.002f)) {
            return;
        }
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        Physics2D.SyncTransforms();

        _startPosition = _previousPosition;
        DistanceTraveled = 0f;
    }
}
