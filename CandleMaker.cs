using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class CandleMaker : MonoBehaviour {

    double myDate;
   
    public GameObject Wick, Candle_Red, Candle_Green, TextMe;    
    public List<string> high_list = new List<string>();
    public List<string> low_list = new List<string>();
    public List<string> open_list = new List<string>();
    public List<string> close_list = new List<string>();
    public List<string> date_list = new List<string>();
    public List<string> vol_list = new List<string>();

    string jsonData;
    int x = 2;
    int loopIncrementer = 0;
    float lenngthOfStick, widthOfStick, openList, closeList, volList, volList1, GreenLength, GreenWidth, RedLength, RedWidth;

    void Start()
    {
        StartCoroutine(MyCandleProgram());
    }



    // Use this for initialization
    IEnumerator MyCandleProgram()
    {
  
        // implememt WWW to get json data from any url
        string url = "https://poloniex.com/public?command=returnChartData&currencyPair=USDT_LTC&start=1483228801&end=1514764801&period=86400";
        WWW www = new WWW(url);
        yield return www;

        // store text in www to json string
        if (string.IsNullOrEmpty(www.error))
        {
            jsonData = www.text;
        }

        // use simpleJSON to get values stored in JSON data for different key value pair
        JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonData);

        // get values at the nodes, to get values at node either use the name directly or the position number of the node
        // here instead of "Department", you can also write jsonNode[0][0] 
        Debug.Log("Department 0 " + jsonNode[0].ToString());
        Debug.Log("Department 1 " + jsonNode[1].ToString());


        // get individual values from Department 0 to 40
        
        while (loopIncrementer < 365)
        {
            loopIncrementer++;

            high_list.Add(jsonNode[loopIncrementer]["high"].ToString());
            low_list.Add(jsonNode[loopIncrementer]["low"].ToString());
            open_list.Add(jsonNode[loopIncrementer]["open"].ToString());
            close_list.Add(jsonNode[loopIncrementer]["close"].ToString());
            date_list.Add(jsonNode[loopIncrementer]["date"].ToString());
            vol_list.Add(jsonNode[loopIncrementer]["quoteVolume"].ToString());

        }
       
        // First make a System.DateTime equivalent to the UNIX Epoch.
        System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

        // Add the number of seconds in UNIX timestamp to be converted.
        dateTime = dateTime.AddSeconds(myDate);

        // The dateTime now contains the right date/time so to format the string,
        // use the standard formatting methods of the DateTime object.
        string printDate = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();

        // Print the date and time
        Debug.Log(printDate);

        

        for (int i = 0, j = 0; i < 365; i++, j++)
        {
            lenngthOfStick = float.Parse(high_list[i]) - float.Parse(low_list[i]);
            widthOfStick = float.Parse(vol_list[i]);
            Wick.transform.localScale = new Vector3(0.3f, lenngthOfStick * 3, 0.2f);
            Instantiate(Wick, new Vector3(j * x, float.Parse(low_list[i]), 0), Quaternion.identity);
           // TextMe.GetComponent<TextMesh>().text = i + "";
            //Instantiate(TextMe, new Vector3(j * x, 0, -2.0f), Quaternion.identity);
            openList = float.Parse(open_list[i]);
            closeList = float.Parse(close_list[i]);
            volList = float.Parse(vol_list[i]);
            volList1 = float.Parse(vol_list[i]);


            if (openList < closeList) // green
            {
                GreenLength = closeList - openList;
                GreenWidth = volList;
                Candle_Green.transform.localScale = new Vector3(1, GreenLength * 3, GreenWidth / 10000);
                Instantiate(Candle_Green, new Vector3(j * x, float.Parse(open_list[i]), 0), Quaternion.identity);
            }

            if (openList > closeList) // red
            {
                RedLength = openList - closeList;
                RedWidth = volList1;
                Candle_Red.transform.localScale = new Vector3(1, RedLength * 3, RedWidth / 10000);
                Instantiate(Candle_Red, new Vector3(j * x, float.Parse(close_list[i]), 0), Quaternion.identity);
            }

        }
    }

   
   
}
