using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;
using ZXing;

public class QRCodeRecenter : MonoBehaviour
{
    [SerializeField]
    private ARSession session;

    [SerializeField]
    private XROrigin sessionOrigin;

    [SerializeField]
    private ARCameraManager cameraManager;

     [SerializeField]
    private GameObject qrCodeRecenter;

    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();

    private Texture2D cameraImageTexture;
    private IBarcodeReader reader = new BarcodeReader();

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            SetQrCodeRecenterTarget("KitchenPoint");
        }
    }

    private void OnEnable(){
        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable(){
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs){
        if(!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image)){
            return;
        }

    var conversionParams = new XRCpuImage.ConversionParams {
        // Get entire Image
        inputRect = new RectInt(0, 0, image.width, image.height),

        //Downsample by 2.
        outputDimensions = new Vector2Int(image.width/2, image.height/2),

        //choose RGBA format
        outputFormat = TextureFormat.RGBA32,

        transformation = XRCpuImage.Transformation.MirrorY
    };

    // see how many bytes you need to store the final image.
    int size = image.GetConvertedDataSize(conversionParams);

    // Allocate a buffer to store the image
    var buffer = new NativeArray<byte>(size, Allocator.Temp);

    //Extract the image data
    image.Convert(conversionParams, buffer);

    //The image was converted to RGBA32 format and written into the provided buffer
    //so you can dispose of the XRCpuImage. You must do this or it will leak resources

    image.Dispose();

    //At this point you can process the image, pass it to a computer vision algorithm, etc.
    // In this example, you apply it to a texture to visualize it
    // You've got the data; let's put it into a texture so you can visualize it

    cameraImageTexture = new Texture2D(
        conversionParams.outputDimensions.x,
        conversionParams.outputDimensions.y,
        conversionParams.outputFormat,
        false);

    cameraImageTexture.LoadRawTextureData(buffer);
    cameraImageTexture.Apply();
    // done with your temporary data
    buffer.Dispose();

    var result = reader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);

    //Do something with the result
    if( result != null){
        SetQrCodeRecenterTarget(result.Text);
    }

    //this.gameObject.SetActive(false);
    qrCodeRecenter.gameObject.SetActive(false);
}

private void SetQrCodeRecenterTarget(string targetText){
    Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(targetText.ToLower()));
    if (currentTarget != null){
        //Reset position and rotation of AR Session
        session.Reset();

        sessionOrigin.transform.position = currentTarget.PositionObject.transform.position;
        sessionOrigin.transform.rotation = currentTarget.PositionObject.transform.rotation;
    }
}
}

