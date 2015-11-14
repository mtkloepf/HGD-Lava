using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

   public GameManagerScript game;
   public MusicManager music;

   // Use this for initialization
   void Start () {

   }

   // Update is called once per frame
   void Update () {
      if(Input.GetKeyDown("escape")) {
         game.togglePauseMenu();
      }
   }

   public void musicSliderUpdate(float val) {
      music.setVolume(val);
   }
}
