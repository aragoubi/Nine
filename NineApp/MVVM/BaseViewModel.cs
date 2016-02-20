namespace Nine.MVVM
{
    /// <summary>
    /// All the ViewModels class must extends this class
    /// It allows us to have common behaviors for all of them (Such as "RaisePropertyChanged()")
    /// </summary>
    public class BaseViewModel : NotifyPropertyChanged
    {
    }
}