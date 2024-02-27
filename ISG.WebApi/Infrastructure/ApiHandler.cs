using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

public class ApiHandler//1 sn altında aynı kullanıcıdan gelen işlemleri duplikate etmesin diye yazıldı.
{
    private static Dictionary<string, DateTime> _lastRequestTimes = new Dictionary<string, DateTime>();
    private static readonly object _lockObject = new object();

    public void ProcessRequest(string userId, string parameter)
    {
        if (!IsDuplicateRequestAllowed())
        {
            lock (_lockObject)
            {
                if (_lastRequestTimes.ContainsKey(userId))
                {
                    DateTime lastRequestTime = _lastRequestTimes[userId];
                    TimeSpan elapsed = DateTime.Now - lastRequestTime;
                    if (elapsed.TotalSeconds < 1)
                    {
                        // İki talep arasında geçen süre 1 saniyeden az ise, işlemi engelle
                        return;
                    }
                }

                // İşlemi devam ettir ve _lastRequestTimes sözlüğünü güncelle
                _lastRequestTimes[userId] = DateTime.Now;
            }
        }

        // API talebini işle
        // ... işlemler
    }

    private bool IsDuplicateRequestAllowed()
    {
        StackTrace stackTrace = new StackTrace();
        MethodBase callerMethod = stackTrace.GetFrame(1).GetMethod();

        object[] attributes = callerMethod.GetCustomAttributes(typeof(ProcessRequestAttribute), true);
        return attributes.Length > 0 && ((ProcessRequestAttribute)attributes[0]).AllowDuplicateRequests;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProcessRequestAttribute : Attribute
{
    public bool AllowDuplicateRequests { get; }

    public ProcessRequestAttribute(bool allowDuplicateRequests)
    {
        AllowDuplicateRequests = allowDuplicateRequests;
    }
}