﻿namespace Xylia.Preview.Common.Exceptions;
internal class BnsXmlReaderException : BnsException
{
    public BnsXmlReaderException(string message, string xml, int line, int pos) : base($"{message} in {xml} at line {line}:{pos}")
    {

    }

    public BnsXmlReaderException(string message, string xml, int line, int pos, Exception innerException) : base($"{message} in {xml} at line {line}:{pos}", innerException)
    {

    }
}