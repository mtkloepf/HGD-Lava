using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{

	private int currency = 0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void setCurrency (int val)
	{
		currency = val;
	}

	public int getCurrency ()
	{
		return currency;
	}

	public void addCurrency (int val)
	{
		currency += val;
	}

	public void subtractCurrency (int val)
	{
		currency -= val;
	}
}
