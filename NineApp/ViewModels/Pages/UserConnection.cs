using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Nine.MVVM;
using Nine.Roles;
using Nine.Tools;
using LinkIOcsharp.model;

namespace Nine.ViewModels.Pages
{
    /// <summary>
    /// DataContext of the 1st page to connect an User from the FallbackUsers.
    /// </summary>
    public class UserConnection : BaseViewModel
    {
        private ICommand _connection;
        private bool _isBusy;
        private ObservableCollection<KeyValuePair<string, User>> _users;

        /// <summary>
        ///     Get the users from the UserMocking class (YAPI)
        /// </summary>
        /// <value>
        ///     The users.
        /// </value>
        public ObservableCollection<KeyValuePair<string, User>> Users
        {
            get
            {
                if (_users != null) return _users;

                Users = new ObservableCollection<KeyValuePair<string, User>>();
                var listUsers = User.getTestUsers();
                for (var i = 0; i < listUsers.Count; i++)
                    Users.Add(new KeyValuePair<string, User>(i + "", listUsers[i]));
                return _users;
            }
            private set { _users = value; }
        }

        /// <summary>
        ///     Connection command
        /// </summary>
        /// <value>
        ///     The connection.
        /// </value>
        public ICommand Connection
        {
            get
            {
                if (_connection == null)
                    Connection = new ParametrizedRelayCommand(user => ConnectUser(int.Parse((string)user)));
                return _connection;
            }
            private set { _connection = value; }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsNotBusy");
            }
        }

        public bool IsNotBusy
        {
            get { return !_isBusy; }
            set
            {
                _isBusy = !value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsBusy");
            }
        }

        /// <summary>
        ///     Connects the user with the specified user.
        /// </summary>
        /// <param name="offset">The offset.</param>
        private void ConnectUser(int offset, bool redirect = true)
        {
            var worker = new BackgroundWorker();
            //this is where the long running process should go
            worker.DoWork += (o, ea) =>
            {
                var user = Users[offset].Value;
                Data.Instance.User = user;
                Data.Instance.Role = user.IsTeacher ? Role.TEACHER : Role.STUDENT;

                LinkIOcsharp.LinkIOImp.Instance
                    .connectTo("bastienbaret.com:8080")
                    .withUser(user.Login)
                    .withID(user.ID)
                    .connect(() => {
                        LinkIOcsharp.LinkIOImp.Instance.joinRoom("NineTest", (id, users) => { });
                    });

                /*try
                {
                    YAPI.Instance.ConnectUser(user);
                }
                catch (Exception)
                {
                    MessageBox.Show("Le service d'identification est temporairement injoignable.\r\nL'application va maintenant se terminer.");
                    App.Current.Dispatcher.BeginInvoke(
                        new Action(
                            () =>
                            {
                                App.Current.Shutdown();
                            }
                        ),
                        DispatcherPriority.Send;
                    );
                }*/
#if !START_ON_MAINPAGE
                Application.Current.Dispatcher.BeginInvoke(
                    new Action(
                        () => { Catalog.Instance.NavigateTo("HomePage"); }
                        ),
                    DispatcherPriority.Normal
                    );
#endif
            };
            worker.RunWorkerCompleted += (o, ea) =>
            {
                //work has completed. we can now interact with the UI
                IsBusy = false;
            };
            //set the IsBusy before start the thread
            IsBusy = true;
            worker.RunWorkerAsync();
        }
    }
}