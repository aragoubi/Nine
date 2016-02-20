using System;

namespace Nine.Roles
{
    /// <summary>
    ///     Safe enum that describe the two differents roles existing in the application.
    /// </summary>
    [Serializable]
    public sealed class Role : BaseModel
    {
        public static readonly Role TEACHER = new Role("Professeur");
        public static readonly Role STUDENT = new Role("Étudiant");

        private Role(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}