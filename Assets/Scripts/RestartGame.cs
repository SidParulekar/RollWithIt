using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private AudioClip ballDrop;
    private void OnTriggerEnter(Collider other)
    {
        SoundManager.Instance.PlaySound(ballDrop);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        
    }
}
