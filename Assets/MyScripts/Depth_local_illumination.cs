using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Depth_local_illumination")]
public sealed class Depth_local_illumination : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter depthDistance = new ClampedFloatParameter(0f, 0f, 32f);

    [Tooltip("Width of the output image.")]
    public IntParameter imageWidth = new IntParameter(600);
    [Tooltip("Height of the output image.")]
    public IntParameter imageHight = new IntParameter(342);


    public int width { get { return imageWidth.value; } }
    public int height { get { return imageHight.value; } }

    Material m_Material;
    float m_FieldOfView;


    internal Boolean socketReady = false;
    
    private TcpListener tcpListener;	
    private Thread tcpListenerThread;
    private Thread tcpWriterThread;
    private TcpClient connectedTcpClient;

    
    //protected RTHandle _rt;
    protected RenderTexture depthColorRT;
    protected Texture2D depthColorT2D;
    
    private Camera cam;
    private MoveCamera movecam;



    public bool IsActive() => m_Material != null && depthDistance.value > 0f;

    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    private int count;
    private int path_tracing_wait;


    public override void Setup()
    {


        if (Shader.Find("Hidden/Shader/Depth_local_illumination") != null)
           m_Material = new Material(Shader.Find("Hidden/Shader/Depth_local_illumination"));
        else
            Debug.LogError("Unable to find shader. Post Process Volume DataCollectorPostProcess is unable to load.");

       cam = Camera.main;
       cam.renderingPath = RenderingPath.DeferredShading;      // use deferred rendering path
       cam.depthTextureMode = DepthTextureMode.Depth;

       movecam = cam.transform.parent.GetComponent<MoveCamera>();

        CreateTextures();

        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
        
        count = Camera.allCameras.Length;
        Debug.Log("We've got " + count + " cameras");
        Debug.Log("We've got " + cam.name);

        path_tracing_wait = 0;

    }


    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        // Packing multiple float paramters into one float4 uniform
        Vector4 parameters = new Vector4(0, 0, 0, cam.fieldOfView);
        m_Material.SetVector("_Params", parameters);
        m_Material.SetTexture("_InputTexture", source);
    
        cmd.Blit(source.rt, depthColorRT, m_Material);
        HDUtils.DrawFullScreen(cmd, m_Material, destination);


        //path_tracing_wait = path_tracing_wait + 1;

        if (Application.isPlaying)// && (path_tracing_wait == 100))
        {
            ReadPixels(depthColorRT, depthColorT2D);
            SaveRaw(depthColorT2D);
            if (movecam != null) movecam.theTrigger = false;
            //path_tracing_wait = 0;
        }

        


    }

    public override void Cleanup() => CoreUtils.Destroy(m_Material);


    protected void CreateTextures()
    {
        //_rt = RTHandles.Alloc(width, height, 1, DepthBits.Depth32, GraphicsFormat.R32G32B32A32_SFloat);
        //depthColorRT = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        //depthColorT2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
        depthColorRT = new RenderTexture(width, height, 24, RenderTextureFormat.ARGBFloat);
        depthColorT2D = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
        cam.targetTexture = depthColorRT;
    }


        protected void ReadPixels(RenderTexture rt, Texture2D t)
    {
        RenderTexture activeRT = RenderTexture.active;
        RenderTexture.active = rt;
        t.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        t.Apply();
        //GL.Clear(true, true, Color.clear);
        RenderTexture.active = activeRT;
    }

    protected void SaveRaw(Texture2D texture)
    {



        byte[] rawTextureData = texture.GetRawTextureData();
        int rawTextureData_length = rawTextureData.Length;
        //byte[] send = Enumerable.Repeat((byte)0x00, (rawTextureData_length / 2)+4).ToArray();
        byte[] send = Enumerable.Repeat((byte)0x00, (rawTextureData_length / 4) *3).ToArray();

        for (int i = 0; i < rawTextureData_length; i = i + 16)
        {
            send[(i / 4)*3] = rawTextureData[i];
            send[(i / 4) * 3 + 1] = rawTextureData[i + 1];
            send[(i / 4) * 3 + 2] = rawTextureData[i + 2];
            send[(i / 4) * 3 + 3] = rawTextureData[i + 3];
            send[(i / 4) * 3 + 4] = rawTextureData[i + 4];
            send[(i / 4) * 3 + 5] = rawTextureData[i + 5];
            send[(i / 4) * 3 + 6] = rawTextureData[i + 6];
            send[(i / 4) * 3 + 7] = rawTextureData[i + 7];
            send[(i / 4) * 3 + 8] = rawTextureData[i + 8];
            send[(i / 4) * 3 + 9] = rawTextureData[i + 9];
            send[(i / 4) * 3 + 10] = rawTextureData[i + 10];
            send[(i / 4) * 3 + 11] = rawTextureData[i + 11];
        }

        /*send[rawTextureData_length/2] = rawTextureData[8];
        send[rawTextureData_length/2 + 1] = rawTextureData[9];
        send[rawTextureData_length/2 + 2] = rawTextureData[10];
        send[rawTextureData_length/2 + 3] = rawTextureData[11];*/
       
        if (socketReady == true)
        {
            socketReady = false;
            tcpWriterThread = new Thread(SendMessage);
            tcpWriterThread.Start(send);
          
        }
    }


        private void ListenForIncommingRequests()
    {
        try
        {
            // Create listener on localhost port 8052. 			
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052);
            tcpListener.Start();
            Debug.Log("Server is listening");
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    // Get a stream object for reading 					
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte arrary. 						
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            // Convert byte array to string message. 							
                            string clientMessage = Encoding.ASCII.GetString(incommingData);
                            Debug.Log("client message received as: " + clientMessage);
                            if(clientMessage == "READY")
                            {
                                socketReady = true;
                            }
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }

	
    private void SendMessage(object send)
    {
        byte[] bytes = (byte[])send;
        if (connectedTcpClient == null)
        {
            return;
        }

        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = connectedTcpClient.GetStream();
            if (stream.CanWrite)
            {

                int len = bytes.Length;
                Debug.Log("Length: " + len);
                byte[] bytes_len = BitConverter.GetBytes(len);

                stream.Write(bytes_len, 0, bytes_len.Length);
                Debug.Log("length sent");

                // Write byte array to socketConnection stream.
                // 
                stream.Write(bytes, 0, bytes.Length);
                Debug.Log("Server sent his message - should be received by client");
                if (movecam != null) movecam.theTrigger = true;
                //System.Threading.Thread.Sleep(200);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }







}


