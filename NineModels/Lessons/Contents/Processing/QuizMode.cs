using System;

namespace Nine.Lessons.Contents.Processing
{
    /// <summary>
    ///     Define the two modes of Quiz (QCM / QCU).
    /// </summary>
    [Serializable]
    public enum QuizMode
    {
        QCM, // A Checkbox Quiz allow several corrects answers
        QCU // A Radio Quiz allow only one correct answer
    }
}