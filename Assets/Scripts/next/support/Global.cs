namespace th.nx
{
    public enum Errno
    {
        OK = 0,
        Unknown = int.MinValue,
        General,
        InvalidArg,
        NoFound,
        Busy,
        Already,
        Timeout,
        Format,
    }

    public static class Global
    {
        
    }
}
