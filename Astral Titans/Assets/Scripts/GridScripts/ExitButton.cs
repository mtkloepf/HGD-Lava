using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExitButton : MonoBehaviour
{
	public void onClick () {
		SceneManager.LoadScene("start_menu");
	}
}
