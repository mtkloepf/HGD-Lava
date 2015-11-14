using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
   public void setVolume(float volume) { 
      AudioListener.volume = volume;
   }

}
