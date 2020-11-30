using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

public class CameraLabeler : MonoBehaviour
{
    //Variables for Video Recording///
    float captureDelay = 1f;
    float captureDuration = 15f;
    bool _done;
    bool isRecordingToPrepare;
    bool isRecordingDone;
    bool isRecorderOn;
    float _timeOut;

    //Variables for common use
    string outputPathFolder;
    public int takeNum;
    public bool printHookBoundingBox;

    //GameObjects to detect
    public Transform OriginPoint;
    public GameObject Hook;
    public Transform Workers;
    public GameObject Forklift;
    public GameObject Truck;

    //Variables for Text Output
    string textPath;
    string outputContent;

    int frame;
    string takeNumString;
    Vector3 hookPos;
    Rect boundBox;
    //////////////////////////////////

    RecorderController recorderController;
    RecorderControllerSettings recorderControlSettings;
    MovieRecorderSettings movieRecorder;

    // Start is called before the first frame update
    void Start()
    {
        //MANUALLY SET TAKE NUMBER
        PlayerPrefs.SetInt("Take", takeNum); 

        /*OLD
        MainCar = transform.parent.gameObject;
        DetectionBox = TargetPedestrian.transform.Find("DetectionBox");
        Top_Left_Detector = DetectionBox.Find("TopLeft");
        Bottom_Right_Detector = DetectionBox.Find("BottomRight");

        //Get variable values for text output
        //carDirection = MainCar.GetComponent<CarState>().MovingDirection();
        //pedestrianBehavior = TargetPedestrian.GetComponent<PedestrianState>().Behavior();
        //viewTarget = TargetPedestrian.GetComponent<PedestrianState>().ViewTarget();
        OLD */

        frame = 0;
        _done = false;
        isRecordingToPrepare = true;
        isRecordingDone = false;
        isRecorderOn = false;
        _timeOut = captureDelay;    

        takeNum = PlayerPrefs.GetInt("Take", 0);
        //Debug.Log("First Take : " + takeNum);

        outputPathFolder = Application.dataPath + "/Recordings/";       // ~Assets/Recordings

        var renderTexture = new RenderTexture(1920, 1080, 32);
        GetComponent<Camera>().targetTexture = renderTexture;

        //GLOBAL UNITY RECORDER SETTINGS
        RecorderOptions.VerboseMode = true;
        recorderControlSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        //recorderControlSettings.SetRecordModeToTimeInterval(0f, 20f);
        recorderControlSettings.FrameRatePlayback = FrameRatePlayback.Constant;
        recorderControlSettings.FrameRate = 30;
        recorderControlSettings.CapFrameRate = true;

        //MOVIE RECORDER SETTINGS
        movieRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        movieRecorder.Enabled = true;
        movieRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        movieRecorder.OutputFile = outputPathFolder + DefaultWildcard.Take; //  Assets/Recordings/<Take>
        movieRecorder.VideoBitRateMode = VideoBitrateMode.High;
        movieRecorder.Take = takeNum;
        movieRecorder.AudioInputSettings.PreserveAudio = false;
        movieRecorder.ImageInputSettings = new RenderTextureInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080,
            RenderTexture = renderTexture,
        };

        recorderControlSettings.AddRecorderSettings(movieRecorder);

        recorderController = new RecorderController(recorderControlSettings);

        //Generate text file to be exported
        if(movieRecorder.Take < 10)
        {
            takeNumString = "00" + movieRecorder.Take;
        }
        else if(movieRecorder.Take < 100)
        {
            takeNumString = "0" + movieRecorder.Take;
        }
        else
        {
            takeNumString = movieRecorder.Take.ToString();
        }
        textPath = outputPathFolder + takeNumString + ".txt";
        if (File.Exists(textPath)) File.Delete(textPath); //Delete file if text file already exists
        File.WriteAllText(textPath, "");

        //////////////////////////////Experiment Zone//////////////////////////////
        


        //////////////////////////////Experiment Zone//////////////////////////////

        /*
        if(!File.Exists(textPath))
        {
            File.WriteAllText(textPath, "");
        }
        */

        /*
        //Path of the file to export
        outputPath = Application.dataPath + "/Recordings/Video_" + DefaultWildcard.Take + ".txt";

        //Create File if it doesn't exist
        if (!File.Exists(outputPath))
        {
            File.WriteAllText(outputPath, "start");
        }
        */
        //settings and manager for UNITY internal video generation API

        //Debug.Log("Take : " + DefaultWildcard.Take);

    }

    // Update is called once per frame
    void Update()
    {
        if (_timeOut > 0)
        {
            _timeOut -= Time.deltaTime;

            //Only write when the camera is on
            if (isRecorderOn)
            {
                //UpdateVariables
                /*carSpeed = MainCar.GetComponent<CarAIController>().Speed();*/
                //Get TopLeft/BottomRight Coordinates
                //topLeft = Camera.main.WorldToScreenPoint(Top_Left_Detector.position);
                //bottomRight = Camera.main.WorldToScreenPoint(Bottom_Right_Detector.position);

                //Write Content in txt file
                //outputContent = takeNumString + " " + frame + " " + topLeft.x + " " + (1080f - topLeft.y) + " " + bottomRight.x + " " + (1080f - bottomRight.y) + " " + pedestrianBehavior + " " + viewTarget + "\n";
                outputContent = takeNumString + " " + frame + " ";

                //If Hook(Crane) exists
                if (Hook)
                { 
                    hookPos = Hook.transform.position - OriginPoint.position;
                    outputContent += hookPos.x + " " + hookPos.y + " " + hookPos.z + " ";

                    /*
                    boundBox = BoundingBox(Hook);

                    //Check if object is within screen
                    if (boundBox.xMin >= 0f && boundBox.xMax <= 1920f && boundBox.yMin >= 0f && boundBox.yMax <= 1080f) 
                    {
                        outputContent += boundBox.xMin + " " + (1080f - boundBox.yMin) + " " + boundBox.xMax + " " + (1080f - boundBox.yMax) + " ";
                    }
                    */
                    if (printHookBoundingBox)
                    {
                        outputContent += BoundingBox(Hook);
                    }
                }
                else
                {
                    outputContent += "- - - ";      //Crane Coordinates
                }

                if (Workers)
                {
                    for (int i = 0; i < Workers.childCount; i++)
                    {
                        /*
                        boundBox = BoundingBox(Workers.GetChild(i).gameObject);

                        //Check if object is within screen
                        if (boundBox.xMin >= 0f && boundBox.xMax <= 1920f && boundBox.yMin >= 0f && boundBox.yMax <= 1080f)
                        {
                            outputContent += boundBox.xMin + " " + (1080f - boundBox.yMin) + " " + boundBox.xMax + " " + (1080f - boundBox.yMax) + " ";
                        }
                        */
                        outputContent += BoundingBox(Workers.GetChild(i).gameObject);
                    }
                }

                //If Forklift exists
                if (Forklift)
                {
                    /*
                    boundBox = BoundingBox(Forklift);

                    //Check if object is within screen
                    if (boundBox.xMin >= 0f && boundBox.xMax <= 1920f && boundBox.yMin >= 0f && boundBox.yMax <= 1080f)
                    {
                        outputContent += boundBox.xMin + " " + (1080f - boundBox.yMin) + " " + boundBox.xMax + " " + (1080f - boundBox.yMax) + " ";
                    }
                    */
                    outputContent += BoundingBox(Forklift);
                }

                //If Truck exists
                if (Truck)
                {
                    /*
                    boundBox = BoundingBox(Truck);

                    //Check if object is within screen
                    if (boundBox.xMin >= 0f && boundBox.xMax <= 1920f && boundBox.yMin >= 0f && boundBox.yMax <= 1080f)
                    {
                        outputContent += boundBox.xMin + " " + (1080f - boundBox.yMin) + " " + boundBox.xMax + " " + (1080f - boundBox.yMax) + " ";
                    }
                    */
                    outputContent += BoundingBox(Truck);
                }

                outputContent += "\n";
                File.AppendAllText(textPath, outputContent);
                frame++;
            }
        }
        else
        {
            if (isRecordingToPrepare)
            {
                //once the time set in the shutterdelay has passed start the recording and set the timer to the duration specified
                _timeOut = captureDuration;
                recorderController.PrepareRecording();
                recorderController.StartRecording();
                Debug.Log("Starting Recording");
                isRecorderOn = true;
                isRecordingToPrepare = false;
            }
            else if (!isRecordingDone)
            {
                _timeOut = 1;
                recorderController.StopRecording();
                isRecordingDone = true;
                Debug.Log("Stopped Recording");
                PlayerPrefs.SetInt("Take", takeNum + 1);
                //Debug.Log("Later Take : " + PlayerPrefs.GetInt("Take"));
            }
            else
            {
                EditorApplication.isPlaying = false;
                return;
            }
            /*
            //once everything is done automatically exit the simulation
            if (_done)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            if (!recorderController.IsRecording())
            {
                //once the time set in the shutterdelay has passed start the recording and set the timer to the duration specified
                _timeOut = captureDuration;
                recorderController.PrepareRecording();
                recorderController.StartRecording();
                Debug.Log("Starting Recording");
            }
            else
            {
                //set another 1 second timer once the recording is finished
                //this is required due to internal postproccessing and saving of the video files.
                _timeOut = 1;
                recorderController.StopRecording();
                _done = true;
                Debug.Log("Stopped Recording");
                PlayerPrefs.SetInt("Take", takeNum+1);
                //Debug.Log("Later Take : " + PlayerPrefs.GetInt("Take"));
            }
            */
        }
        
        /*
        if (!_recorderController.IsRecording())
        {
            //once the time set in the shutterdelay has passed start the recording and set the timer to the duration specified
            _recorderController.PrepareRecording();
            _recorderController.StartRecording();
            Debug.Log("Starting Recording");
        }

        //Set Output Format here
        outputFormat = DefaultWildcard.Take + " " + frame;

        //Add text to file
        File.AppendAllText(outputPath, outputFormat);

        frame++;
        */
    }

    string BoundingBox(GameObject obj)
    {
        float min_x = 1920f, min_y = 1080f, max_x = 0f, max_y = 0f;
        BoxCollider box = obj.GetComponent<BoxCollider>();
        Vector2 screenPos;

        for(int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                for (int k = -1; k <= 1; k += 2)
                {
                    screenPos = GetComponent<Camera>().WorldToScreenPoint(obj.transform.position + new Vector3(box.center.x + (i * (box.size.x / 2)), box.center.y + (j * (box.size.y / 2)), box.center.z + (k * (box.size.z / 2))));

                    if (screenPos.x < 0f || screenPos.x > 1920f || screenPos.y < 0f || screenPos.y > 1080f)
                    {
                        return "";
                    }

                    if(min_x > screenPos.x)
                    {
                        min_x = screenPos.x;
                    }
                    if(min_y > (1080f - screenPos.y))
                    {
                        min_y = 1080f - screenPos.y;
                    }
                    if(max_x < screenPos.x)
                    {
                        max_x = screenPos.x;
                    }
                    if(max_y < (1080f - screenPos.y))
                    {
                        max_y = 1080f - screenPos.y;
                    }
                }
            }
        }

        string outputContent = min_x + " " + min_y + " " + max_x + " " + max_y + " ";
        return outputContent;

        /*
        Vector2 pos1 = GetComponent<Camera>().WorldToScreenPoint(new Vector3(box.center.x + (box.size.x / 2), box.center.y + (box.size.y / 2), box.center.z + (box.size.z / 2)));
        Vector2 pos2 = GetComponent<Camera>().WorldToScreenPoint(new Vector3(box.center.x - (box.size.x / 2), box.center.y + (box.size.y / 2), box.center.z + (box.size.z / 2)));
        Vector2 pos3 = GetComponent<Camera>().WorldToScreenPoint(new Vector3(box.center.x + (box.size.x / 2), box.center.y - (box.size.y / 2), box.center.z + (box.size.z / 2)));
        Vector2 pos4 = GetComponent<Camera>().WorldToScreenPoint(new Vector3(box.center.x - (box.size.x / 2), box.center.y - (box.size.y / 2), box.center.z + (box.size.z / 2)));
        Vector2 pos5 = GetComponent<Camera>().WorldToScreenPoint(new Vector3(box.center.x + (box.size.x / 2), box.center.y + (box.size.y / 2), box.center.z - (box.size.z / 2)));
        Vector2 pos6 = GetComponent<Camera>().WorldToScreenPoint(new Vector3(box.center.x - (box.size.x / 2), box.center.y + (box.size.y / 2), box.center.z - (box.size.z / 2)));
        Vector2 pos7 = GetComponent<Camera>().WorldToScreenPoint(new Vector3(box.center.x + (box.size.x / 2), box.center.y - (box.size.y / 2), box.center.z - (box.size.z / 2)));
        Vector2 pos8 = GetComponent<Camera>().WorldToScreenPoint(new Vector3(box.center.x - (box.size.x / 2), box.center.y - (box.size.y / 2), box.center.z - (box.size.z / 2)));

        min_x = Mathf.Min(pos1.x, pos2.x, pos3.x, pos4.x, pos5.x, pos6.x, pos7.x, pos8.x);
        max_x = Mathf.Max(pos1.x, pos2.x, pos3.x, pos4.x, pos5.x, pos6.x, pos7.x, pos8.x);
        min_y = Mathf.Min(pos1.y, pos2.y, pos3.y, pos4.y, pos5.y, pos6.y, pos7.y, pos8.y);
        max_y = Mathf.Max(pos1.y, pos2.y, pos3.y, pos4.y, pos5.y, pos6.y, pos7.y, pos8.y);

        Rect r = Rect.MinMaxRect(min_x, min_y, max_x, max_y);
        return r;
        */
    }
}
