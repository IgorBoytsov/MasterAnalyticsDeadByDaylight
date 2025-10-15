namespace Shared.Kernel.Results
{
    public enum ErrorCode
    {
        None = 0,

        /*Доменные*/

        Domain = 1000,

        /*Объекты*/
        
        Null = 2000,
        InvalidRequest = 2001,
        InvalidResponse = 2002,

        /*Серверные*/
       
        Create = 3000,
        Save = 3001,
        Update = 3002, 
        Delete = 3003,
        Connection = 3004,
        Server = 3005,

        /*Ожидаемые*/

        Validation = 4000,
        NotFound = 4001,
        Conflict = 4002,
        Exist = 4003,
        ApiError = 4004,
        InvalidIdentifier = 4005,
    }
}