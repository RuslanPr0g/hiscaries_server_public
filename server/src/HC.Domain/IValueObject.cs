namespace HC.Domain;

public interface IValueObject<T>
{
    T Value { get; }
}