using UnityEngine;

public class Moon : MonoBehaviour {

    public delegate void SuperMoonAchieved();

    public delegate void ControllableMoonAbsorbed(Rigidbody2D moonRigidbody2D);

    public event SuperMoonAchieved onSuperMoonAchieved;

    public event ControllableMoonAbsorbed onControllableMoonAbsorbed;
    
    [SerializeField]
    private float maxScaleIncrease;

    [SerializeField]
    private float scalePerMoon;

    private void Start() {
        var controllableMoons = GameObject.FindGameObjectsWithTag("Controllable Moon").Length;

        scalePerMoon = maxScaleIncrease / controllableMoons;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("Controllable Moon")) {
            return;
        }
        transform.localScale += (Vector3) (Vector2.one * scalePerMoon);
        onControllableMoonAbsorbed?.Invoke(other.rigidbody);

        Destroy(other.gameObject);

        if (GameObject.FindGameObjectsWithTag("Controllable Moon").Length == 1) {
            onSuperMoonAchieved?.Invoke();
        }
    }

}
