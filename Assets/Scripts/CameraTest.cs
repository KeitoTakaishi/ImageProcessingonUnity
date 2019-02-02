using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTest : MonoBehaviour {
	[SerializeField] private int m_width = 1280;
	[SerializeField] private int m_height = 720;
	[SerializeField] private int FPS;
	
	[SerializeField] private Material WebCamMat;	//撮影中
	[SerializeField] private Material ResultMat;	//結果画像
	
	WebCamTexture webcamTexture;
	private WebCamTexture m_webCamTexture = null;
	private int id = 0;

	public GameObject canv;

	void Start()
	{
		WebCamDevice[] devices = WebCamTexture.devices;
		for (int i = 0; i < devices.Length; i++)
		{
			string camname = devices[i].name;
			Debug.Log(camname);
			
			if(camname.Contains("Camera"))
			{
				Debug.Log("WebCamPlay");
				webcamTexture = new WebCamTexture(camname, m_width, m_height, FPS);
				WebCamMat.mainTexture = webcamTexture;
				webcamTexture.Play();
				break;
			}
		}
	}
	
	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if ( webcamTexture != null ) {
				//SaveToPNGFile( webcamTexture.GetPixels(), Application.dataPath + "/../SavedScreen" + id.ToString() + ".png" );
				SaveToPNGFile( webcamTexture.GetPixels(), Application.dataPath + "/../SavedScreen.png" );
				//id++;
			}
		}//結果画像をmaterialに表示
		else if (Input.GetKeyDown(KeyCode.A))
		{
			//Texture tex = ReadTexture(Application.dataPath + "/../SavedScreen" + id.ToString() + ".png", m_width, m_height);
			Texture tex = ReadTexture(Application.dataPath + "/../SavedScreen.png", m_width, m_height);
			ResultMat.mainTexture = tex;
			canv.GetComponent < RawImage >().texture = ResultMat.mainTexture;
		}
	}
	
	//撮影画像をpngを保存
	void SaveToPNGFile( Color[] texData , string filename ) {
		Texture2D takenPhoto = new Texture2D(m_width, m_height, TextureFormat.ARGB32, false);            
		takenPhoto.SetPixels(texData);
		takenPhoto.Apply();            
		byte[] png = takenPhoto.EncodeToPNG();
		Destroy(takenPhoto);
        		
		File.WriteAllBytes(filename, png);
	}
	
	//撮影画像
	byte[] ReadPngFile(string path)
	{
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		BinaryReader bin = new BinaryReader(fileStream);
		byte[] values = bin.ReadBytes((int) bin.BaseStream.Length);
		bin.Close();
		return values;
	}
	
	Texture ReadTexture(string path, int width, int height)
	{
		byte[] readBinary = ReadPngFile(path);
            
		Texture2D textute = new Texture2D(width, height);
		textute.LoadImage(readBinary);
            
		return textute;
	}
//	private IEnumerator Star()
//	{
//		if( WebCamTexture.devices.Length == 0 )
//		{
//			Debug.LogFormat( "No Cam is Connectted" );
//			yield break;	
//		}
//		Debug.Log("Can Use Camera");
//
//		yield return Application.RequestUserAuthorization( UserAuthorization.WebCam );
//		if( !Application.HasUserAuthorization( UserAuthorization.WebCam ) )
//		{
//			Debug.LogFormat( "Don't use Cam" );
//			yield break;
//		}
//		
//		Debug.Log("Cam Num :" + WebCamTexture.devices.Length);
//		Debug.Log("CamDev[0].name :" + WebCamTexture.devices[0].name);
//		//WebCamDevice userCameraDevice = WebCamTexture.devices[ 0 ];
//		m_webCamTexture = new WebCamTexture( WebCamTexture.devices[ 0 ].name, m_width, m_height );
//		m_displayUI.texture = m_webCamTexture;
//		m_webCamTexture.Play();
//	}
}
