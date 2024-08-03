using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System;
using System.Globalization;

public static class DateTimeExtensions
{
    public static string ToKLTimeString(this DateTime dt)
    {
        return dt.ToString("G", CultureInfo.CreateSpecificCulture("en-my"));
    }

    public static DateTime ParseToKLDateTime(this string date)
    {
        return DateTime.ParseExact(date, "G", CultureInfo.CreateSpecificCulture("en-my"));
    }

    public static DateTime ConvertToKLDateTime(this DateTime currentDateTime)
    {
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        CultureInfo klCulture = CultureInfo.CreateSpecificCulture("en-my");
        string currentDateTimeString = currentDateTime.ToString("G", currentCulture);
        DateTime klDateTime = DateTime.ParseExact(currentDateTimeString, "G", klCulture);

        return klDateTime;
    }
}

/// <summary>
/// Attach this class in LoginScene and its variants
/// </summary>
public class WorldTimer : MonoBehaviour
{
    /// <summary>
    /// For more APIs, visit https://worldtimeapi.org/api
	/// For more timezones, visit https://worldtimeapi.org/api/timezone
    /// </summary>
    private const string URL_REALTIME = "https://worldtimeapi.org/api/timezone/Asia/Kuala_Lumpur";

    // initilise with current time in case of network connection error
    private static DateTime _currentDateTime = DateTime.Now;

    /// <summary>
    /// All listeners are cleared onDestroy()
    /// </summary>
    public static bool IsInitialized = false;



    public static DateTime UtcNow
    {
        get
        {
            // haven't manage to fetch real time, use system time first
            if (!IsInitialized)
            {
                return DateTime.Now;
            }

            // here we don't need to get the datetime from the server again
            // just add elapsed time since the game start to _currentDateTime
            return _currentDateTime.AddSeconds(Time.realtimeSinceStartup);
        }
    }


    #region Singleton
    public static WorldTimer Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }
    #endregion

    private void Init()
    {
        StartCoroutine(GetRealTimeFromAPI());
    }


    private IEnumerator GetRealTimeFromAPI()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(URL_REALTIME);
        yield return webRequest.SendWebRequest();


        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            // error
            Debug.Log($"Error: {webRequest.error}");
        }
        else
        {
            TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);

            _currentDateTime = ParseDateTime(timeData.utc_datetime);
            IsInitialized = true;

            //TestPrint(_currentDateTime, timeData);
        }
        webRequest.Dispose();
    }


    public static DateTime ParseDateTime(string datetime)
    {
        // timeData.datetime example value is : 2020-08-14T15:54:04+01:00
        string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;
        string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

        return DateTime.Parse($"{date} {time}");
    }

    /// <summary>
    /// Use this for a quick way to view all available timezones
    /// </summary>
    private static void ViewTimeZones()
    {
        System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo> zones = TimeZoneInfo.GetSystemTimeZones();
        Debug.Log($"The local system has the following {0} time zones {zones.Count}");
        foreach (TimeZoneInfo zone in zones)
            Debug.Log(zone.Id);
    }

    private void TestPrint(DateTime dateTime, TimeData timeData)
    {
        Debug.Log(_currentDateTime);
        //   Debug.Log(timeData.utc_offset);
    }
    /* API (json)
	{
	"abbreviation" : "+01",
	"client_ip"    : "190.107.125.48",
	"datetime"     : "2020-08-14T15:544:04+01:00",
	"dst"          : false,
	"dst_from"     : null,
	"dst_offset"   : 0,
	"dst_until"    : null,
	"raw_offset"   : 3600,
	"timezone"     : "Asia/Brunei",
	"unixtime"     : 1595601262,
	"utc_datetime" : "2020-08-14T15:54:04+00:00",
	"utc_offset"   : "+01:00"
	}

We only need "datetime" property.
*/

    // json container
    struct TimeData
    {
        public string utc_datetime;
        public string utc_offset;
    }
}

