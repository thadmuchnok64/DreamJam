using Godot;
using System;
using System.Collections.Generic;
using static Godot.TextServer;

public static class StaticHelpers
{

	public static List<T> GetChildrenOfType<T>(Node3D root, bool recursive = false) where T : Node3D
	{
		var result = new List<T>();

		foreach (Node3D child in root.GetChildren())
		{
			if (child is T typedChild)
				result.Add(typedChild);

			if (recursive)
				result.AddRange(GetChildrenOfTypeRecursive<T>(child));
		}

		return result;
	}


	// Helper for recursion
	private static List<T> GetChildrenOfTypeRecursive<T>(Node root) where T : Node
	{
		var result = new List<T>();

		foreach (Node child in root.GetChildren())
		{
			if (child is T typedChild)
				result.Add(typedChild);

			result.AddRange(GetChildrenOfTypeRecursive<T>(child));
		}

		return result;
	}

	public static T GetRandomElement<T>(this List<T> list)
	{
		Random rand = new Random();
		return list[rand.Next(list.Count)];
	}

	public static Variant GetRandomElement<variant>(this Godot.Collections.Array<Variant> list)
	{
		Random rand = new Random();
		return list[rand.Next(list.Count)];
	}

	public static Vector3 RandomVector( )
	{
		Random rand = new Random();
		var x = (float)(rand.NextDouble() - .5f);
        var y = (float)(rand.NextDouble() - .5f);
        var z = (float)(rand.NextDouble() - .5f);
		return new Vector3(x, y, z).Normalized();

    }

	public static Vector2 RandomVector2D()
	{
		Random rand = new Random();
		var x = (float)(rand.NextDouble() - .5f);
		var y = (float)(rand.NextDouble() - .5f);
		return new Vector2(x, y).Normalized();

	}

	public static Vector3 AddSpreadToDirection(this Vector3 vec, float spreadRadians)
	{
		Random rand = new Random();
		var direction = vec.Rotated(Vector3.Up, spreadRadians* (float)((rand.NextDouble() - .5f) * 2));
		direction = direction.Rotated(Vector3.Right, spreadRadians* (float)((rand.NextDouble() - .5f) * 2));
		direction = direction.Rotated(Vector3.Forward, spreadRadians* (float)((rand.NextDouble() - .5f) * 2));

		return direction;
	}

	public static Vector3 ReplaceX(this Vector3 vec,float val)
	{
		return new Vector3(val, vec.Y, vec.Z);
	}

    public static Vector3 ReplaceY(this Vector3 vec, float val)
    {
        return new Vector3(vec.X, val, vec.Z);
    }
    public static Vector3 ReplaceZ(this Vector3 vec, float val)
    {
        return new Vector3(vec.X, vec.Y, val);
    }

	public static float Clamp01(this float val)
	{
		return Mathf.Clamp(val, 0, 1);
	}
}
