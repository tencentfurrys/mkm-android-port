using System.Collections.Generic;

public class GameConfigData
{
	public int intDifficulty;

	public int intLanguage;

	public int intCamera;

	public int intInput;

	public int intTextSpeed;

	public float fltTextVol;

	public float fltMusicVol;

	public float fltSoundVol;

	public float fltVoiceVol;

	public float fltVoiceRate;

	public int intLastZoomLevel;

	public bool toggleMinimapState;

	public List<KeyBindingItem> listOfKeyBinding;

	public List<KeyBindingItem> listOfKeyBindingGamepadType1;

	public List<KeyBindingItem> listOfKeyBindingGamepadType2;

	public GameConfigDataAdvanced adv;

	public CFG_FetishToggle fetishes;

	public List<CFG_OverrideFetishToggle> listFetishOverride;

	public bool configIsGalleryEnabled;

	public bool galleryRoomBeach;

	public bool galleryRoomForest;

	public bool galleryRoomCastle;

	public bool galleryRoomCaves;

	public bool galleryRoomSwamp;

	public bool galleryRoomHill;

	public bool galleryRoomHighTech;

	public bool galleryRoomCity;

	public GameConfigData()
	{
		intDifficulty = 1;
		intLanguage = 0;
		intCamera = 0;
		intInput = 0;
		intTextSpeed = 1;
		fltTextVol = 0.6f;
		fltMusicVol = 0.7f;
		fltSoundVol = 1f;
		fltVoiceVol = 0.9f;
		fltVoiceRate = 10f;
		intLastZoomLevel = 0;
		toggleMinimapState = false;
		listOfKeyBinding = new List<KeyBindingItem>();
		listOfKeyBindingGamepadType1 = new List<KeyBindingItem>();
		listOfKeyBindingGamepadType2 = new List<KeyBindingItem>();
		adv = new GameConfigDataAdvanced();
		fetishes = new CFG_FetishToggle();
		listFetishOverride = new List<CFG_OverrideFetishToggle>();
		configIsGalleryEnabled = false;
		galleryRoomBeach = false;
		galleryRoomForest = false;
		galleryRoomCastle = false;
		galleryRoomCaves = false;
		galleryRoomSwamp = false;
		galleryRoomHill = false;
		galleryRoomHighTech = false;
		galleryRoomCity = false;
	}
}
