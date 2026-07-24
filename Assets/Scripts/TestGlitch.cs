using UnityEngine;

public class TestGlitch : MonoBehaviour
{
    public GlitchManager glitchManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            glitchManager.TriggerGlitch();
        }
    }
}
