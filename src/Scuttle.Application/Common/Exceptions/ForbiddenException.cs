using System;

namespace Scuttle.Application.Common.Exceptions;
public class ForbiddenException : Exception
{
    public ForbiddenException() : base("you do not have permission to perform this action.")
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }
}