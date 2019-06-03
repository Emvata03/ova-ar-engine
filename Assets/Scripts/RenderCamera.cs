using System;
using System.Collections;
  using System.Collections.Generic;
  using System.IO;
 using UnityEngine;
 
  public class RenderCamera : MonoBehaviour
{
    int index = 0;

    public int maxPhotoCount = 50;
    public string photoDirectoryPath;
    public GameObject cube;

    string user {
        get {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        }
     }

    string ovaPictureDirectory { 
        get
        {
            var directory = user + @"\\OvaPicture";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;


        }
    }


    void Start()
    {
        Debug.Log("Start");
        Debug.Log(photoDirectoryPath);
        

        transform.LookAt(cube.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Save");
        }
        transform.RotateAround(cube.transform.position, Vector3.up, 20 * Time.deltaTime);
        
    }

    private void LateUpdate()
    {

        if (TargetTexture == null || index <= maxPhotoCount)
        {
            Save();
           
        }
      
    }

    private RenderTexture TargetTexture;
 
     private void OnRenderImage(RenderTexture source, RenderTexture destination)
     {
       
         TargetTexture = source;
         Graphics.Blit(source, destination);
     }
 
     private void Save()
     {

        if (TargetTexture == null) {
            return;
        }
        Debug.Log("Save");

        RenderTexture.active = TargetTexture;
        
 
         Texture2D png = new Texture2D(RenderTexture.active.width, RenderTexture.active.height, TextureFormat.ARGB32, true);
         png.ReadPixels(new Rect(0, 0, RenderTexture.active.width, RenderTexture.active.height), 0, 0);
 
         byte[] bytes = png.EncodeToPNG();
        photoDirectoryPath = $"{ovaPictureDirectory}\\pict{index}.png";

        string path = string.Format(photoDirectoryPath);

        index = index + 1;

  
        FileStream file = File.Open(path, FileMode.Create);
         BinaryWriter writer = new BinaryWriter(file);
         writer.Write(bytes);
         file.Close();
         writer.Close();
 
         Destroy(png);
         png = null;
         
        // Debug.Log("Save Down");
     }
 }

