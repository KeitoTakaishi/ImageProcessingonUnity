using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTest : MonoBehaviour {
	[SerializeField]
	private int     m_width     = 1920;
	[SerializeField]
	private int     m_height    = 1080;
	[SerializeField]
	private RawImage    m_displayUI = null;
	private WebCamTexture   m_webCamTexture     = null;
	
	private IEnumerator Start()
	{
		if( WebCamTexture.devices.Length == 0 )
		{
			Debug.LogFormat( "No Cam is Connectted" );
			yield break;
			
		}
		Debug.Log("Can Use Camera");

		yield return Application.RequestUserAuthorization( UserAuthorization.WebCam );
		if( !Application.HasUserAuthorization( UserAuthorization.WebCam ) )
		{
			Debug.LogFormat( "Don't use Cam" );
			yield break;
		}
		
		Debug.Log("Cam Num :" + WebCamTexture.devices.Length);
		Debug.Log("CamDev[0].name :" + WebCamTexture.devices[0].name);
		//WebCamDevice userCameraDevice = WebCamTexture.devices[ 0 ];
		m_webCamTexture = new WebCamTexture( WebCamTexture.devices[ 0 ].name, m_width, m_height );
		m_displayUI.texture = m_webCamTexture;
		m_webCamTexture.Play();
		
	}
}
