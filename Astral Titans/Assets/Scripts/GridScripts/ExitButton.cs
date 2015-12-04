using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour
{
	public void onClick ()
	{
		Application.LoadLevel ("start_menu");
	}
}
