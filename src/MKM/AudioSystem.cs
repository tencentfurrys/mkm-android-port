using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
	private enum TypeOfAudioPlayer
	{
		Music,
		SFX,
		Voice
	}

	private enum TypeOfFade
	{
		FadeIn,
		FadeOut
	}

	public AudioClipsList listOfAudio;

	public MayaVoiceGroups listOfMayaVoiceGroups;

	public AudioSource sourceMusicPlayer;

	public AudioSource sourceEroGrabbedPlayer;

	public AudioMixerGroup defaultSFXMixerGroup;

	public AudioMixerGroup muffledSFXMixerGroup;

	public AudioSource[] arraySFXPlayers;

	private float amountToFade;

	private List<AudioGroupingByName> listAudioGroups;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		listAudioGroups = new List<AudioGroupingByName>();
		InvokeRepeating("InvokeSFXPlayerAudioClipRemoval", 3f, 3f);
	}

	public void PlayMusicTheme(GameCore.MusicTheme thisMusic, bool doFadeIn, float fadeTime, bool doLoop)
	{
		sourceMusicPlayer.clip = listOfAudio.ReturnMusicAudioClipByType(thisMusic);
		sourceMusicPlayer.loop = doLoop;
		if (sourceMusicPlayer.clip != null)
		{
			if ((bool)GC)
			{
				UpdateAndApplyVolumeConfigToAudioPlayers(TypeOfAudioPlayer.Music);
			}
			if (doFadeIn)
			{
				StartFading(TypeOfFade.FadeIn, fadeTime);
			}
			else
			{
				StopMusicFadeLogic();
			}
			sourceMusicPlayer.Play();
		}
	}

	public void StopMusicTheme(bool doFadeOut, float fadeTime)
	{
		if (sourceMusicPlayer.isPlaying)
		{
			if ((bool)GC)
			{
				UpdateAndApplyVolumeConfigToAudioPlayers(TypeOfAudioPlayer.Music);
			}
			if (doFadeOut)
			{
				StartFading(TypeOfFade.FadeOut, fadeTime);
			}
			else
			{
				sourceMusicPlayer.Stop();
			}
		}
	}

	public void PlayEroGrabbedTheme()
	{
		sourceMusicPlayer.mute = true;
		sourceEroGrabbedPlayer.clip = listOfAudio.ReturnMusicAudioClipByType(GameCore.MusicTheme.EroGrabbed);
		sourceEroGrabbedPlayer.loop = true;
		sourceEroGrabbedPlayer.Play();
	}

	public void StopEroGrabbedTheme()
	{
		sourceEroGrabbedPlayer.Stop();
		StartFading(TypeOfFade.FadeIn, 1f);
		sourceMusicPlayer.mute = false;
	}

	private void StartFading(TypeOfFade type, float seconds)
	{
		StopMusicFadeLogic();
		amountToFade = sourceMusicPlayer.volume / (seconds * 10f);
		if (!(amountToFade <= 0f))
		{
			switch (type)
			{
			case TypeOfFade.FadeIn:
				sourceMusicPlayer.volume = 0f;
				InvokeRepeating("InvokedFadeInMusicThemeVolume", 0f, 0.1f);
				break;
			case TypeOfFade.FadeOut:
				InvokeRepeating("InvokedFadeOutMusicThemeVolume", 0f, 0.1f);
				break;
			}
		}
	}

	private void InvokedFadeInMusicThemeVolume()
	{
		sourceMusicPlayer.volume += amountToFade;
		if (sourceMusicPlayer.volume >= GC.configMusicVolume || amountToFade <= 0f)
		{
			sourceMusicPlayer.volume = GC.configMusicVolume;
			StopMusicFadeLogic();
		}
	}

	private void InvokedFadeOutMusicThemeVolume()
	{
		sourceMusicPlayer.volume -= amountToFade;
		if (sourceMusicPlayer.volume <= 0f || amountToFade <= 0f)
		{
			sourceMusicPlayer.Stop();
			StopMusicFadeLogic();
		}
	}

	private void StopMusicFadeLogic()
	{
		if (IsInvoking("InvokedFadeInMusicThemeVolume"))
		{
			CancelInvoke("InvokedFadeInMusicThemeVolume");
		}
		if (IsInvoking("InvokedFadeOutMusicThemeVolume"))
		{
			CancelInvoke("InvokedFadeOutMusicThemeVolume");
		}
	}

	public void ToggleMainMusicMute(bool doMute, bool doFade, float timeFade = 0f)
	{
		sourceMusicPlayer.mute = doMute;
		if (doFade)
		{
			StartFading(TypeOfFade.FadeIn, timeFade);
		}
	}

	public void RealTimeUpdateVolumeOfMusic()
	{
		UpdateAndApplyVolumeConfigToAudioPlayers(TypeOfAudioPlayer.Music);
	}

	public void SetMusicVolumeManually(float volume)
	{
		sourceMusicPlayer.volume = volume;
	}

	public void SetupPlayDiagLetter(GameCore.SFXBasic1 thisSFX, float offsetVolPrct, bool isCheckForDuplicity, float pitchLow, float pitchHigh, string strGroupName, bool doMuffled, bool DeferByGroup)
	{
		float volum = CalculateOffsetVolumeByPercentage(offsetVolPrct, GC.configTextVolume);
		PlaySFX(listOfAudio.ReturnSFXBasic1AudioClipByType(thisSFX), volum, isCheckForDuplicity, thisSFX.ToString(), pitchLow, pitchHigh, strGroupName, doMuffled, DeferByGroup);
	}

	public void SetupPlaySFXBasic(GameCore.SFXBasic1 thisSFX, float offsetVolPrct, bool isCheckForDuplicity, float pitchLow, float pitchHigh, string strGroupName, bool doMuffled, bool DeferByGroup)
	{
		float volum = CalculateOffsetVolumeByPercentage(offsetVolPrct, GC.configSoundEffectVolume);
		PlaySFX(listOfAudio.ReturnSFXBasic1AudioClipByType(thisSFX), volum, isCheckForDuplicity, thisSFX.ToString(), pitchLow, pitchHigh, strGroupName, doMuffled, DeferByGroup);
	}

	public void SetupPlaySFXBasic(GameCore.SFXBasic2 thisSFX, float offsetVolPrct, bool isCheckForDuplicity, float pitchLow, float pitchHigh, string strGroupName, bool doMuffled, bool DeferByGroup)
	{
		float volum = CalculateOffsetVolumeByPercentage(offsetVolPrct, GC.configSoundEffectVolume);
		PlaySFX(listOfAudio.ReturnSFXBasic2AudioClipByType(thisSFX), volum, isCheckForDuplicity, thisSFX.ToString(), pitchLow, pitchHigh, strGroupName, doMuffled, DeferByGroup);
	}

	public void SetupPlaySFXMonsterSounds(GameCore.MonsterSounds thisSFX, float offsetVolPrct, bool isCheckForDuplicity, float pitchLow, float pitchHigh, string strGroupName, bool doMuffled, bool DeferByGroup)
	{
		float volum = CalculateOffsetVolumeByPercentage(offsetVolPrct, GC.configSoundEffectVolume);
		PlaySFX(listOfAudio.ReturnMonsterAudioClipByType(thisSFX), volum, isCheckForDuplicity, thisSFX.ToString(), pitchLow, pitchHigh, strGroupName, doMuffled, DeferByGroup);
	}

	public void SetupPlaySFXEro(GameCore.EroSound thisSFX, float offsetVolPrct, bool isCheckForDuplicity, float pitchLow, float pitchHigh, string strGroupName, bool doMuffled, bool DeferByGroup)
	{
		float volum = CalculateOffsetVolumeByPercentage(offsetVolPrct, GC.configSoundEffectVolume);
		PlaySFX(listOfAudio.ReturnEroAudioClipByType(thisSFX), volum, isCheckForDuplicity, thisSFX.ToString(), pitchLow, pitchHigh, strGroupName, doMuffled, DeferByGroup);
	}

	public void SetupPlaySFXMayaVoice(GameCore.MayaVoice thisSFX, float offsetVolPrct, bool isCheckForDuplicity, float pitchLow, float pitchHigh, string strGroupName, bool doMuffled, bool DeferByGroup)
	{
		float volum = CalculateOffsetVolumeByPercentage(offsetVolPrct, GC.configVoiceVolume);
		PlaySFX(listOfAudio.ReturnMayaVoiceAudioClipByType(thisSFX), volum, isCheckForDuplicity, thisSFX.ToString(), pitchLow, pitchHigh, strGroupName, doMuffled, DeferByGroup);
	}

	public void SetupPlaySFXMayaVoice(AudioClip directClip, float offsetVolPrct, bool isCheckForDuplicity, float pitchLow, float pitchHigh, string strGroupName, bool doMuffled, bool DeferByGroup)
	{
		float volum = CalculateOffsetVolumeByPercentage(offsetVolPrct, GC.configVoiceVolume);
		PlaySFX(directClip, volum, isCheckForDuplicity, directClip.name, pitchLow, pitchHigh, strGroupName, doMuffled, DeferByGroup);
	}

	private void PlaySFX(AudioClip receivedAudio, float volum, bool isChkDupli, string audioTypeName, float pLow, float pHigh, string groupedName, bool applyMuffled, bool dontIfAlreadyPlayingByGroup)
	{
		if (receivedAudio == null)
		{
			Debug.Log("Could not find a SFX type of " + audioTypeName + " in the type list.");
			return;
		}
		AudioSource audioSource = ReturnValidSFXPlayer(isChkDupli, receivedAudio, groupedName);
		if (audioSource != null)
		{
			if (!dontIfAlreadyPlayingByGroup || !ExistAudioGroup(groupedName) || !(audioSource != null) || !audioSource.isPlaying)
			{
				if (groupedName != "" && !ExistAudioGroup(groupedName))
				{
					listAudioGroups.Add(new AudioGroupingByName
					{
						groupName = groupedName,
						audioSourceOfGroup = audioSource
					});
				}
				if (pLow > 0f || pHigh > 0f)
				{
					audioSource.pitch = Random.Range(pLow, pHigh);
				}
				else
				{
					audioSource.pitch = 1f;
				}
				if (applyMuffled)
				{
					audioSource.outputAudioMixerGroup = muffledSFXMixerGroup;
				}
				else if (audioSource.outputAudioMixerGroup != defaultSFXMixerGroup)
				{
					audioSource.outputAudioMixerGroup = defaultSFXMixerGroup;
				}
				audioSource.clip = receivedAudio;
				audioSource.volume = volum;
				audioSource.Play();
			}
		}
		else
		{
			Debug.Log("Could not get a valid reference for a SFX Player in the array. Current audio: " + audioTypeName);
		}
	}

	public void StopAllSFXAudioSources()
	{
		for (int i = 0; i < arraySFXPlayers.Length; i++)
		{
			if (arraySFXPlayers[i].isPlaying)
			{
				arraySFXPlayers[i].Stop();
			}
		}
	}

	public void StopSFX(GameCore.SFXBasic2 thisSFX)
	{
		AudioClip audioClip = listOfAudio.ReturnSFXBasic2AudioClipByType(thisSFX);
		if (audioClip != null)
		{
			for (int i = 0; i < arraySFXPlayers.Length; i++)
			{
				if (arraySFXPlayers[i].clip == audioClip)
				{
					arraySFXPlayers[i].Stop();
					break;
				}
			}
		}
		else
		{
			Debug.Log("Tried to stop SFX named " + thisSFX.ToString() + " but the database returned NULL!");
		}
	}

	private AudioSource ReturnValidSFXPlayer(bool checkDuplicity, AudioClip thisAC, string groupName)
	{
		foreach (AudioGroupingByName listAudioGroup in listAudioGroups)
		{
			if (groupName != "" && groupName == listAudioGroup.groupName)
			{
				return listAudioGroup.audioSourceOfGroup;
			}
		}
		if (checkDuplicity)
		{
			for (int i = 0; i < arraySFXPlayers.Length; i++)
			{
				if (arraySFXPlayers[i].isPlaying && arraySFXPlayers[i].clip == thisAC && !CheckAudioSourceTiedToGroup(arraySFXPlayers[i]))
				{
					return arraySFXPlayers[i];
				}
			}
		}
		for (int j = 0; j < arraySFXPlayers.Length; j++)
		{
			if (!arraySFXPlayers[j].isPlaying && !CheckAudioSourceTiedToGroup(arraySFXPlayers[j]))
			{
				return arraySFXPlayers[j];
			}
		}
		return null;
	}

	private bool CheckAudioSourceTiedToGroup(AudioSource thisAS)
	{
		if (listAudioGroups.Exists((AudioGroupingByName x) => thisAS == x.audioSourceOfGroup))
		{
			return true;
		}
		return false;
	}

	private bool ExistAudioGroup(string checkThisGroupName)
	{
		if (checkThisGroupName != "" && listAudioGroups.Exists((AudioGroupingByName x) => checkThisGroupName == x.groupName))
		{
			return true;
		}
		return false;
	}

	public void SetupPlaySFXGeneric(string genericName, int idSFX, bool isCheckForDuplicity, float pitchLow, float pitchHigh, string volumeBase, string strGroupName, bool doMuffled, bool DeferByGroup)
	{
		AudioClip audioClip = null;
		float volum = 0f;
		switch (genericName)
		{
		case "ForestBoss":
			audioClip = listOfAudio.ReturnForestBossSounds(idSFX);
			break;
		case "HillBoss":
			audioClip = listOfAudio.ReturnHillBossSounds(idSFX);
			break;
		case "WerewolfBoss":
			audioClip = listOfAudio.ReturnWerewolfBossSounds(idSFX);
			break;
		case "MinotaurBoss":
			audioClip = listOfAudio.ReturnMinotaurBossSounds(idSFX);
			break;
		case "PrincessBoss":
			audioClip = listOfAudio.ReturnPrincessBossSounds(idSFX);
			break;
		case "WitchBoss":
			audioClip = listOfAudio.ReturnWitchBossSounds(idSFX);
			break;
		case "BlueFoxBoss":
			audioClip = listOfAudio.ReturnBlueFoxBossSounds(idSFX);
			break;
		case "HarunaNPC":
			audioClip = listOfAudio.ReturnHarunaVoiceMoans(idSFX);
			break;
		}
		if (volumeBase == "sfx")
		{
			volum = GC.configSoundEffectVolume;
		}
		else if (volumeBase == "voice")
		{
			volum = GC.configVoiceVolume;
		}
		if (audioClip != null)
		{
			PlaySFX(audioClip, volum, isCheckForDuplicity, idSFX.ToString(), pitchLow, pitchHigh, strGroupName, doMuffled, DeferByGroup);
		}
		else
		{
			Debug.Log("Trying to play a generic SFX, but the AUDIOCLIP is null! NAME: " + genericName + " -> idSFX: " + idSFX);
		}
	}

	private void UpdateAndApplyVolumeConfigToAudioPlayers(TypeOfAudioPlayer typeOfPlayer)
	{
		if (!GC)
		{
			return;
		}
		switch (typeOfPlayer)
		{
		case TypeOfAudioPlayer.Music:
			sourceMusicPlayer.volume = GC.configMusicVolume;
			sourceEroGrabbedPlayer.volume = GC.configMusicVolume;
			break;
		case TypeOfAudioPlayer.SFX:
		{
			for (int i = 0; i < arraySFXPlayers.Length; i++)
			{
				arraySFXPlayers[i].volume = GC.configSoundEffectVolume;
			}
			break;
		}
		}
	}

	private float CalculateOffsetVolumeByPercentage(float offsetByThisPercentage, float baseVol)
	{
		float num = baseVol;
		if (offsetByThisPercentage != 0f)
		{
			num = baseVol + GC.MATH_ResultValueOfTotalPercentile(offsetByThisPercentage, baseVol);
			if (num > 1f)
			{
				num = 1f;
			}
		}
		else if (offsetByThisPercentage < 0f)
		{
			offsetByThisPercentage *= -1f;
			num = baseVol - GC.MATH_ResultValueOfTotalPercentile(offsetByThisPercentage, baseVol);
			if (num < 0f)
			{
				num = 0f;
			}
		}
		return num;
	}

	private void InvokeSFXPlayerAudioClipRemoval()
	{
		for (int i = 0; i < arraySFXPlayers.Length; i++)
		{
			if (arraySFXPlayers[i].clip != null && !arraySFXPlayers[i].isPlaying)
			{
				arraySFXPlayers[i].clip = null;
			}
		}
		listAudioGroups.RemoveAll((AudioGroupingByName x) => !x.audioSourceOfGroup.isPlaying);
	}
}
