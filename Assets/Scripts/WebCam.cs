    using UnityEngine;
    using System.Collections;
    using System.IO;
    using UnityEditor;
    
    public class WebCam : MonoBehaviour {
        public int Width  = 1280;
        public int Height = 720;
        public int FPS    = 15;
    
        public Material material;
    
        WebCamTexture webcamTexture;
    
        void Start () {
            WebCamDevice[] devices = WebCamTexture.devices;
    
            // display all cameras
            for (var i = 0; i < devices.Length; i++) {
                // get camera name
                string camname = devices[i].name;
                print(i+":"+camname);
    
                if ( camname.Contains("Camera") ) {
                    webcamTexture = new WebCamTexture(camname, Width, Height, FPS);
                    print ( webcamTexture );
                    material.mainTexture = webcamTexture;
                    webcamTexture.Play();
                    break;
                }
            }
        }

        public Material save;
        
        void Update() {
            if (Input.GetKeyDown (KeyCode.Space)) {
                if ( webcamTexture != null ) {
                    Debug.Log(Application.dataPath);
                    SaveToPNGFile( webcamTexture.GetPixels(), Application.dataPath + "/../SavedScreen.png" );
                }
            }else if (Input.GetKeyDown(KeyCode.A))
            {
                Texture t = ReadTexture(Application.dataPath + "/../SavedScreen.png",Width, Height);
                save.mainTexture = t;
            }
        }
    
        void SaveToPNGFile( Color[] texData , string filename ) {
            Texture2D takenPhoto = new Texture2D(Width, Height, TextureFormat.ARGB32, false);
            
            takenPhoto.SetPixels(texData);
            takenPhoto.Apply();
            
            byte[] png = takenPhoto.EncodeToPNG();
            Destroy(takenPhoto);
            
            // For testing purposes, also write to a file in the project folder
            File.WriteAllBytes(filename, png);
        }
        
        //ReadTexture
        
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
        
    }