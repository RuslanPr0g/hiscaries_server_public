using HC.Domain;
using System;

public abstract class Entity<T> : IEquatable<Entity<T>> where T : Identity
{
    public T Id { get; init; }

    protected Entity(T id)
    {
        Id = id;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Entity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<T> other)
            return false;

        return Equals(other);
    }

    public bool Equals(Entity<T>? other)
    {
        if (other is null)
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity<T>? left, Entity<T>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity<T>? left, Entity<T>? right)
    {
        return !(left == right);
    }
}