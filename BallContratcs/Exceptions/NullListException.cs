﻿namespace BallContratcs.Exceptions;

public class NullListException : Exception
{
	public NullListException() : base("The returned list is null") { }
}