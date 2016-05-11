/*
    These interfaces can be used to make types that support arithmetic operations
    usable for generics. You might want to use explicit interface implementations.

    All basic data types such as sbyte,short,int,long,float,double,decimal should
    implement the IArithmetic<T> interface. Types like string that only support the 
    + operator should only implement IAddable<T> and maybe IHasZero<T>. 
*/
#pragma warning disable 1591

namespace System.Tools.Extra
{
    /// <summary>
    /// Every class that defines a + operation 
    /// like T operator + (T a,T b) should consider
    /// implementing this interface. 
    /// </summary>
    public interface IAddable<T>
    {
        /// <summary>
        /// Returns the sum of the object and <typeparamref name="T"/>.
        /// It must not modify the value of the object
        /// </summary>
        /// <param name="a">The second operand</param>
        /// <returns>the sum</returns>
        T Add(T a);
    }
    /// <summary>
    /// Every class that defines a - operation 
    /// like T operator - (T a,T b) should consider
    /// implementing this interface.
    /// </summary>
    public interface ISubtractable<T>
    {
        /// <summary>
        /// Returns the difference of the object and <typeparamref name="T"/>.
        /// It must not modify the value of the object
        /// </summary>
        /// <param name="a">The second operand</param>
        /// <returns>the difference</returns>
        T Subtract(T a);
    }
    /// <summary>
    /// Every class that has a + operation and an element
    /// such that <code>x+e = e+x = x</code> for all x should consider implementing this 
    /// interface. If T is a ValueType, e should be the default value. 
    /// </summary>
    public interface IHasZero<T> : IAddable<T>
    {
        /// <summary>
        /// Returns the neutral element of addition
        /// </summary>
        /// <value>e</value>
        T Zero { get; }
    }
    /// <summary>
    /// Every class that defines a unary - operation
    /// such that x+(-x)=e and -(-x)=x should consider
    /// implementing this interface.
    /// </summary>
    public interface INegatable<T> : IAddable<T>, ISubtractable<T>, IHasZero<T>
    {
        /// <summary>
        /// Returns the negative of the object. Must not modify the object itself.
        /// </summary>
        /// <returns>The negative</returns>
        T Negative();
    }
    /// <summary>
    /// Every class that defines a * operation 
    /// like T operator * (T a,T b) should consider
    /// implementing this interface.
    /// </summary>
    public interface IMultipliable<T>
    {
        /// <summary>
        /// Returns the product of the object and <typeparamref name="T"/>.
        /// It must not modify the value of the object
        /// </summary>
        /// <param name="a">The second operand</param>
        /// <returns>the product</returns>
        T Multiply(T a);
    }
    /// <summary>
    /// Every class that defines a / operation 
    /// like T operator / (T a,T b) should consider
    /// implementing this interface.
    /// </summary>
    public interface IDivisible<T>
    {
        /// <summary>
        /// Returns the quotient of the object and <typeparamref name="T"/>.
        /// It must not modify the value of the object
        /// </summary>
        /// <param name="a">The second operand</param>
        /// <returns>the quotient</returns>
        T Divide(T a);
    }
    /// <summary>
    /// Every class that has a * operation and an element
    /// such that <code>x*e = e*x = x</code> for all x should consider implementing
    /// this interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasOne<T> : IMultipliable<T>
    {
        /// <summary>
        /// Returns the neutral element of multiplication
        /// </summary>
        /// <value>e</value>
        T One { get; }
    }
    /// <summary>
    /// Every class that defines an unary inverse operation such that
    /// x*inverse(x)~=e and inverse(inverse(x))~=x for all x except Zero
    /// should consider implementing this interface. 
    /// Types that might want to implement this would be float, double, decimal,
    /// but not short,int,long.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInvertible<T> : IMultipliable<T>, IDivisible<T>, IHasOne<T>
    {
        /// <summary>
        /// Returns the inverse
        /// </summary>
        /// <returns>the inverse</returns>
        T Inverse();
    }
    /// <summary>
    /// Every class that defines the standard arithmetic operations +,-,*,/ should
    /// consider implementing this interface. This interface does not contain
    /// IInvertible because some arithmetic types such as int, long etc. do not have
    /// an inverse as defined in IInvertible. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IArithmetic<T> :
        IAddable<T>, ISubtractable<T>, IHasZero<T>, INegatable<T>,
        IMultipliable<T>, IDivisible<T>, IHasOne<T>
    { }
}