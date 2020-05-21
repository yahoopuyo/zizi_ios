using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadOnline : MonoBehaviour
{

    ModeData md;
    // Start is called before the first frame update
    void Start()
    {
        md = GameObject.Find("ModeData").GetComponent<ModeData>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    void Reload()
    {
        Scene loadScene = SceneManager.GetActiveScene();
        md.OnReloadGame();
        // Sceneの読み直し
        SceneManager.LoadScene(loadScene.name);
    }

    void RestartGame()
    {
        PhotonView view = GetComponent<PhotonView>();
        md.OnReloadGame();
        view.RPC("Reload", PhotonTargets.Others);
    }
    public void ReloadForAll()
    {
        if (md.isHost)
        {
            RestartGame();
            StartCoroutine(DelayMethod(1.0f));
        }
    }
    private IEnumerator DelayMethod(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("reload");
        Scene loadScene = SceneManager.GetActiveScene();
        // Sceneの読み直し
        SceneManager.LoadScene(loadScene.name);
    }
}