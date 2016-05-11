using System;

#pragma warning disable 1591

namespace System.Tools.Extra
{
    #region  Interfaces 

    /// <summary>
    /// Supporta la clonazione, ovvero la creazione di una nuova istanza di una classe con lo stesso valore di un'istanza esistente.
    /// </summary>
    /// <typeparam name="T">Tipo di dato clonabile</typeparam>
    public interface ICloneable<T> : ICloneable
    {
        /// <summary>
        /// Crea un nuovo oggetto che è una copia dell'istanza corrente.
        /// </summary>
        /// <returns>Nuovo oggetto che è una copia dell'istanza corrente.</returns>
        new T Clone();
    }

    #endregion  Interfaces 

    #region Abstract Classes 

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CloneableBase<T> : ICloneable<T> where T : CloneableBase<T>
    {
        public abstract T Clone();

        object ICloneable.Clone()
        {
            return Clone();
        }
    }

    public abstract class CloneableExBase<T> : CloneableBase<T> where T : CloneableExBase<T>
    {
        protected abstract T CreateClone();
        protected abstract void FillClone( T clone );

        public override T Clone()
        {
            T clone = CreateClone();
            if( ReferenceEquals( clone, null ) )
            {
                throw new NullReferenceException( "Clone was not created." );
            }
            return clone;
        }
    }

    internal abstract class PersonBase<T> : CloneableExBase<T> where T : PersonBase<T>
    {
        public string Name { get; set; }

        protected override void FillClone( T clone )
        {
            clone.Name = Name;
        }
    }

    internal abstract class EmployeeBase<T> : PersonBase<T> where T : EmployeeBase<T>
    {
        public string Department { get; set; }

        protected override void FillClone( T clone )
        {
            base.FillClone( clone );
            clone.Department = Department;
        }
    }

    #endregion Abstract Classes

    #region Sealed Classes 

    internal sealed class Person : PersonBase<Person>
    {
        protected override Person CreateClone()
        {
            return new Person();
        }
    }

    internal sealed class Employee : EmployeeBase<Employee>
    {
        protected override Employee CreateClone()
        {
            return new Employee();
        }
    }

    #endregion Sealed Classes
}

#pragma warning restore 1591