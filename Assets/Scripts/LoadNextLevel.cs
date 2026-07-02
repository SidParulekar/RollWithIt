using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    [SerializeField] private AudioClip levelDone;
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySound(levelDone);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
