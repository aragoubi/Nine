using LinkIO.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkIOcsharp.model
{
    public class User
    {
        public String Login { get; set; }
        public String ID { get; set; }
        public ERole Role { get; set; }
        public bool IsTeacher {
            get { return Role == ERole.Teacher; }
        }

        public string CompleteName {
            get { return Login; }
        }

        public string FirstName {
            get { return Login; }
        }

        public User() {
            Role = ERole.Student;
        }

        private static List<User> _users;
        public static List<User> getTestUsers()
        {
            if (_users == null)
            {
                _users = new List<User>();
                _users.Add(new User() { Login = "Bastien", ID = "0" });
                _users.Add(new User() { Login = "Léo", ID = "1" });
                _users.Add(new User() { Login = "Francois", ID = "2" });
                _users.Add(new User() { Login = "Valentin", ID = "3" });
                _users.Add(new User() { Login = "Florent", ID = "4" });
                _users.Add(new User() { Login = "William", ID = "5" });

                _users[0].Role = ERole.Teacher;
            }

            return _users;
        }
    }
}
