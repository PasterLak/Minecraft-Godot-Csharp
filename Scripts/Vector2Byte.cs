﻿using System;
using Godot;

public struct Vector2Byte
{
	public byte x;
	public byte y;

	public Vector2Byte(byte x, byte y)
	{
		this.x = x;
		this.y = y;
	}
	public Vector2Byte(Vector2I v)
	{
		this.x = (byte)v.X;
		this.y = (byte)v.Y;
	}
	public Vector2Byte(Vector2 v)
	{
		this.x = (byte)v.X;
		this.y = (byte)v.Y;
	}
	public static Vector2Byte Zero
	{
		get { return new Vector2Byte(0, 0); }
	}


	public static Vector2Byte operator + (Vector2Byte v1, Vector2Byte v2)
	{
		Vector2Byte result = new Vector2Byte();

		if (v1.x + v2.x <= Byte.MaxValue) result.x = (byte)(v1.x + v2.x);
		else result.x = Byte.MaxValue;

		if (v1.y + v2.y <= Byte.MaxValue) result.y = (byte)(v1.y + v2.y);
		else result.y = Byte.MaxValue;

		return result;
	}

	public static Vector2Byte operator -(Vector2Byte v1, Vector2Byte v2)
	{
		Vector2Byte result = new Vector2Byte();

		if (v1.x - v2.x >= 0) result.x = (byte)(v1.x - v2.x);
		else result.x = 0;

		if (v1.y - v2.y >= 0) result.y = (byte)(v1.y - v2.y);
		else result.y = 0;

		return result;
	}

	public static explicit operator Vector2I(Vector2Byte v)
	{
		return new Vector2I(v.x, v.y);
	}
	public static explicit operator Vector2Byte(Vector2I v)
	{
		return new Vector2Byte(v);
	}

	public static explicit operator Vector2(Vector2Byte v)
	{
		return new Vector2(v.x, v.y);
	}
	public static explicit operator Vector2Byte(Vector2 v)
	{
		return new Vector2Byte(v);
	}

	public static ushort Distance(Vector2Byte a, Vector2Byte b)
	{
		return (ushort)Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
	}
	
	public static ushort DistanceSquared(Vector2Byte a, Vector2Byte b)
	{
		return (ushort)((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
	}

	public override string ToString()
	{
		return string.Format("({0},{1})", x, y);
	}


}