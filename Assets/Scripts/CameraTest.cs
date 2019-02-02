using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTest : MonoBehaviour {
	[SerializeField] private int m_width;
	[SerializeField] private int m_height;
	[SerializeField] private int FPS;
	
	[SerializeField] private Material WebCamMat;	
	[SerializeField] private Material ResultMat;	
	
	private WebCamTexture webcamTexture = null;
	private int id = 0;

	public GameObject TakePicScene;
	public GameObject ResultScene;


	#region MonoFunc

	void Awake()
	{
		TakePicScene.SetActive(true);
		ResultScene.SetActive(false);
	}

	void Start()
	{
		WebCamInit();
	}
	
	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			TakePicture();
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			ShowPicture();
		}
	}

	#endregion


	#region MyFunc


	void WebCamInit()
	{
		
		//m_width = (int)TakePicScene.GetComponent < RectTransform >().rect.width;
		//m_height = (int)TakePicScene.GetComponent<RectTransform>().rect.height;
		
		m_width = 1280;
		m_height = 720;
		
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

	public void TakePicture()
	{
		if ( webcamTexture != null ) {
			//change UI
			TakePicScene.SetActive(true);
			ResultScene.SetActive(false);
			//process
			SaveToPNGFile( webcamTexture.GetPixels(), Application.dataPath + "/../SavedScreen" + id.ToString() + ".png" );
			id++;
		}
	}


	public void ShowPicture()
	{
		//change UI
		TakePicScene.SetActive(!TakePicScene.active);
		ResultScene.SetActive(!ResultScene.active);
		Texture tex = ReadTexture(Application.dataPath + "/../SavedScreen" + (id-1).ToString() + ".png", m_width, m_height);
		ResultMat.mainTexture = tex;
		
		ResultScene.GetComponent < RawImage >().texture = ResultMat.mainTexture;
	}
	
	
	void SaveToPNGFile( Color[] texData , string filename ) {
		Texture2D takenPhoto = new Texture2D(m_width, m_height, TextureFormat.ARGB32, false);
		Debug.Log(takenPhoto.GetPixels().Length);
		takenPhoto.SetPixels(texData);
		takenPhoto.Apply();            
		byte[] png = takenPhoto.EncodeToPNG();
		Destroy(takenPhoto);
        		
		File.WriteAllBytes(filename, png);
	}
	
	
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

	#endregion
	
}
