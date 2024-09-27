using System.Globalization;

namespace AutoMonitoring.BLL.Exceptions;

public class UserBlockedException : Exception
{
    public UserBlockedException(string message) : base(message) { }
    public UserBlockedException(DateTime blockUntil,string userName):base($"User with username blocked until {blockUntil.ToString(CultureInfo.CurrentCulture)}"){}
}