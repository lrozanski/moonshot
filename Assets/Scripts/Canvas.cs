using UnityEngine;

public class Canvas : MonoBehaviour {

    [SerializeField]
    private Moon moon;

    [SerializeField]
    private GameObject victoryPanel;

    private void Start() {
        moon.onSuperMoonAchieved += () => victoryPanel.SetActive(true);
    }
}
