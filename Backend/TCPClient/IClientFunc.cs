﻿namespace TCPClient;

public interface IClientFunc
{
    Task<int> SendMessageAsync(string message,Guid clockId);
}