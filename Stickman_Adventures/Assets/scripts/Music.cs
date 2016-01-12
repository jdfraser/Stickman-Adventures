using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	public AudioSource[] tracks;
	private int currentTrack;
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
		currentTrack = 0;
	}

	void Update(){
		if(! tracks[currentTrack].isPlaying && isOn ()){
			nextTrack ();
		}
	}

	public void toggle(){
		tracks[currentTrack].enabled = ! tracks[currentTrack].enabled;
		if(tracks[currentTrack].enabled){
			//music just got turned on. let's mix it up a bit.
			nextTrack();
		}
	}

	public bool isOn(){
		return tracks[currentTrack].enabled;
	}

	private void nextTrack(){
		tracks [currentTrack].enabled = false; //disable old track
		currentTrack ++;
		if(currentTrack > tracks.Length - 1){
			currentTrack = 0;
		}
		tracks [currentTrack].enabled = true; //enable new track
	}

}
