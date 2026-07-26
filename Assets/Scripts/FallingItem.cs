using UnityEngine;

public enum ItemType { Rain, Lightning }

public class FallingItem : MonoBehaviour
{
    public ItemType type;
    public float fallSpeed = 4f;

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < -6f)
            Destroy(gameObject); // missed
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Umbrella"))
        {
            Debug.Log("Hit by: " + type);

            if (type == ItemType.Rain)
            {
                WeatherAudioManager.Instance.PlayRainSplash();
                GameManager.Instance.AddRain();
            }
            else if (type == ItemType.Lightning)
            {
                WeatherAudioManager.Instance.PlayLightningStruck();
                WeatherAudioManager.Instance.PlayBatteryDraining();
                GameManager.Instance.HitByLightning(transform.position);
            }

            Destroy(gameObject);
        }
    }
}