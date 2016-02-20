using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nine.MVVM
{
    /// <summary>
    /// BaseViewModel inherits of this Class
    /// It allows us to call "RaisePropertyChanged()" in all the ViewModels
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        ///     Occurs when a property is updated
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Raises the changed property to notify subscribers
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <example>
        /// How to use (without parameter)
        /// <code>
        /// private object _property;
        /// public object Property
        /// {
        ///     get
        ///     {
        ///         return _property;
        ///     }
        ///     set
        ///     {
        ///         _property = value;
        ///         RaisePropertyChanged(); // Raises the Current ("Property") Property 
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// How to use (with parameter)
        /// <code>
        /// private object _property;
        /// public object Property
        /// {
        ///     get
        ///     {
        ///         return _property;
        ///     }
        ///     set
        ///     {
        ///         _property = value;
        ///         RaisePropertyChanged(); // Raises the Current ("Property") Property 
        ///     }
        /// }
        /// private TYPE _subProperty;
        /// public TYPE SubProperty
        /// {
        ///     get
        ///     {
        ///         return Property.SubProperty;
        ///     }
        ///     set
        ///     {
        ///         Property.SubProperty = value;
        ///         RaisePropertyChanged(); // Raises the Current ("SubProperty") Property 
        ///         RaisePropertyChanged("Property"); // Raises the "Property" Property
        ///     }
        /// }
        /// </code>
        /// </example>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}